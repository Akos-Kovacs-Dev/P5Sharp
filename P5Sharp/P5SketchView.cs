using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Input;

namespace P5Sharp
{
    public class P5SketchView : ContentView
    {
        #region Fields

        private readonly SKCanvasView _canvasView = new();
        private readonly P5SharpConfig _p5sharpConfig;

        private SketchBase _sketch;
        private Dictionary<string, Action<string>> _sketchActions = new();
        private Client _client;


        private bool _runSetup = true;
        private bool _isRunning = false;

        private int _frameRate = 60;

        private List<string> _files = new();
        private bool _hotReloadInDebug = true;
        private bool _reDrawOnSizeChanged = true;

        private string _serverIp;
        private int _serverPort;

        #endregion

        #region Constructor

        public P5SketchView()
        {
            Content = _canvasView;

            _p5sharpConfig = Application.Current?.Handler?.MauiContext?.Services?.GetService<P5SharpConfig>()
                               ?? throw new InvalidOperationException("Service provider not available. Ensure this is called after MAUI is initialized.");

            _canvasView.PaintSurface += OnCanvasViewPaintSurface;
            Loaded += (_, _) => StartLoop();
            Unloaded += (_, _) => StopLoop();
        }

        #endregion

        #region Bindable Properties

        public static readonly BindableProperty FilesProperty =
            BindableProperty.Create(nameof(Files), typeof(List<string>), typeof(P5SketchView), new List<string>());

        public static readonly BindableProperty P5ObjectsProperty =
            BindableProperty.Create(nameof(P5Objects), typeof(string), typeof(P5SketchView), "", propertyChanged: OnFilesCsvChanged);

        public static readonly BindableProperty SketchProperty =
            BindableProperty.Create(nameof(Sketch), typeof(SketchBase), typeof(P5SketchView), null, propertyChanged: OnSketchChanged);

        public static readonly BindableProperty HotReloadInDebugProperty =
            BindableProperty.Create(nameof(HotReloadInDebug), typeof(bool), typeof(P5SketchView), true);

        public static readonly BindableProperty FrameRateProperty =
            BindableProperty.Create(nameof(FrameRate), typeof(int), typeof(P5SketchView), 60);

        public static readonly BindableProperty EnableTouchEventsProperty =
            BindableProperty.Create(nameof(EnableTouchEvents), typeof(bool), typeof(P5SketchView), true);

        public static readonly BindableProperty ReDrawOnSizeChangedProperty =
            BindableProperty.Create(nameof(ReDrawOnSizeChanged), typeof(bool), typeof(P5SketchView), true);

        public static readonly BindableProperty SketchCommandProperty =
            BindableProperty.Create(nameof(SketchCommand), typeof(ICommand), typeof(P5SketchView), default(ICommand), propertyChanged: OnSketchChanged);


        #endregion

        #region Properties

        public List<string> Files
        {
            get => (List<string>)GetValue(FilesProperty);
            set => SetValue(FilesProperty, value);
        }

        public string P5Objects
        {
            get => (string)GetValue(P5ObjectsProperty);
            set => SetValue(P5ObjectsProperty, value);
        }

        public SketchBase Sketch
        {
            get => (SketchBase)GetValue(SketchProperty);
            set => SetValue(SketchProperty, value);
        }

        public bool HotReloadInDebug
        {
            get => (bool)GetValue(HotReloadInDebugProperty);
            set => SetValue(HotReloadInDebugProperty, value);
        }

        public int FrameRate
        {
            get => (int)GetValue(FrameRateProperty);
            set => SetValue(FrameRateProperty, value);
        }

        public bool EnableTouchEvents
        {
            get => (bool)GetValue(EnableTouchEventsProperty);
            set => SetValue(EnableTouchEventsProperty, value);
        }

        public bool ReDrawOnSizeChanged
        {
            get => (bool)GetValue(ReDrawOnSizeChangedProperty);
            set => SetValue(ReDrawOnSizeChangedProperty, value);
        }

        public ICommand SketchCommand
        {
            get => (ICommand)GetValue(SketchCommandProperty);
            set => SetValue(SketchCommandProperty, value);
        }
        #endregion

        #region Property Changed Handlers

        private static void OnSketchChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is P5SketchView view && newValue is SketchBase sketch)
            {
                //  Automatically add the sketch's file to Files if needed
                string inferredPath = view.InferSketchFilePath(sketch);
                if (!view.Files.Contains(inferredPath))
                    view.Files.Insert(0, inferredPath); // <-- put the main sketch file first


                view.InitializeInternal(sketch);
            }

        }
        private string InferSketchFilePath(SketchBase sketch)
        {
            var type = sketch.GetType();
            var fullPath = type.FullName;
            var index = fullPath.IndexOf('.');
            if (index >= 0)
            {
                fullPath = fullPath.Substring(index + 1);
            }
            fullPath = fullPath.Replace(".", "/");
            return $"{fullPath}.cs";
        }




        private static void OnFilesCsvChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is P5SketchView view && newValue is string csv)
            {
                view.Files = csv
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(path => path.Replace("\\", "/")) // ✅ No root namespace prepended
                    .ToList();
            }
        }



        #endregion

        #region Initialization

        private void InitializeInternal(SketchBase sketch)
        {
            _sketch = sketch;
            _runSetup = true;

            _frameRate = FrameRate;
            _files = Files;
            _hotReloadInDebug = HotReloadInDebug;
            _reDrawOnSizeChanged = ReDrawOnSizeChanged;
            _sketchActions = sketch?.SketchActions;

            _canvasView.EnableTouchEvents = EnableTouchEvents;
            _serverIp = _p5sharpConfig?.IP;
            _serverPort = _p5sharpConfig?.Port ?? 0;

            if (_files == null || !_files.Any())
                throw new Exception("No Sketch file attached to P5SketchView.");

            if (_hotReloadInDebug && IsDebug())
                InitializeSketchAndServer();

            AddEventsFromCanvasToSketch();
        }

        private void InitializeSketchAndServer()
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    if (string.IsNullOrEmpty(_serverIp))
                        throw new Exception("HotReload missing IP");

                    if (_serverPort == 0)
                        throw new Exception("HotReload missing port");

                    _client = new Client(_serverIp, _serverPort);
                    _ = _client.StartTcpCommunicationAsync(_files, GetSketchCallback());
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"TCP Client Failed: {ex.Message}");
                }
            });
        }

        private bool IsDebug()
        {
            var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(DebuggableAttribute), false);
            if (attributes.Length == 0) return false;
            return ((DebuggableAttribute)attributes[0]).IsJITTrackingEnabled;
        }

        #endregion

        #region Drawing

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            var info = e.Info;

            try
            {
                P5Variables memory = _sketch ?? new P5Variables();
                memory._canvas = canvas;
                memory._info = info;

                if (_runSetup)
                {
                    _sketch?.OnSetup(canvas, info);
                    _sketch?.clear();
                    _sketchActions = _sketch?.SketchActions;
                    _sketch.SketchCommand = SketchCommand;
                    _runSetup = false;
                }
                else
                {
                    _sketch?.OnDraw(memory);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during drawing: {ex}");
            }
        }

        #endregion

        #region Touch + Size Events

        private void AddEventsFromCanvasToSketch()
        {
            if (_canvasView.EnableTouchEvents)
            {
                _canvasView.Touch += (s, e) =>
                {
                    var pt = e.Location;
                    switch (e.ActionType)
                    {
                        case SKTouchAction.Pressed:
                            _sketch?.OnMouseDown(pt.X, pt.Y);
                            _sketch?.OnTouchStart(pt.X, pt.Y);
                            break;
                        case SKTouchAction.Moved:
                            _sketch?.OnMouseMove(pt.X, pt.Y);
                            _sketch?.OnTouchMove(pt.X, pt.Y);
                            break;
                        case SKTouchAction.Released:
                        case SKTouchAction.Cancelled:
                            _sketch?.OnMouseUp(pt.X, pt.Y);
                            _sketch?.OnTouchEnd(pt.X, pt.Y);
                            break;
                    }

                    e.Handled = true;
                };
            }

            if (_reDrawOnSizeChanged)
            {
                _canvasView.SizeChanged += (s, e) =>
                {
                    _runSetup = true;
                    _canvasView.InvalidateSurface();
                };
            }
        }

        #endregion

        #region Hot Reload / Script Evaluation

        private Func<string, Task> GetSketchCallback()
        {
            return async (code) =>
            {
                _sketch = await LoadSketchText(code);
                _runSetup = true;
                AddEventsFromCanvasToSketch();
            };
        }

        private async Task<SketchBase> LoadSketchText(string code)
        {
            var options = ScriptOptions.Default
                .AddReferences(typeof(SKCanvas).Assembly, typeof(SketchBase).Assembly)
                .AddImports("System", "SkiaSharp");

            if (string.IsNullOrEmpty(code))
                return await RunErrorSketch("Sketch data is empty!");

            if (HasSyntaxErrors(code))
                return await RunErrorSketch("Syntax error detected in the code.");

            try
            {
                return await CSharpScript.EvaluateAsync<SketchBase>(code, options);
            }
            catch (CompilationErrorException ex)
            {
                return await RunErrorSketch(string.Join(Environment.NewLine, ex.Diagnostics.Select(d => d.ToString())));
            }
            catch (Exception ex)
            {
                return await RunErrorSketch(ex.Message);
            }
        }

        private bool HasSyntaxErrors(string code)
        {
            var tree = CSharpSyntaxTree.ParseText(code);
            return tree.GetDiagnostics().Any(d => d.Severity == DiagnosticSeverity.Error);
        }

        private async Task<SketchBase> RunErrorSketch(string message)
        {
            var errorCode = SketchMessages.ErrorSketch(message);

            return await CSharpScript.EvaluateAsync<SketchBase>(
                errorCode,
                ScriptOptions.Default
                    .AddReferences(typeof(SKCanvas).Assembly, typeof(SketchBase).Assembly)
                    .AddImports("System", "SkiaSharp"));
        }

        #endregion

        #region Actions & Loop

        public void InvokeSketchAction(string actionName, string arg = "")
        {
            if (_sketchActions?.TryGetValue(actionName, out var action) == true)
                action?.Invoke(arg);
        }

        private void StartLoop()
        {
            _isRunning = true;
            _client?.ResumeAsync();

            _canvasView.Dispatcher.StartTimer(TimeSpan.FromMilliseconds(1000.0 / _frameRate), () =>
            {
                if (!_isRunning) return false;

                try
                {
                    _canvasView.InvalidateSurface();
                    return true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"CanvasView invalidate failed: {ex.Message}");
                    return false;
                }
            });
        }

        private void StopLoop()
        {
            _isRunning = false;
            _client?.Pause();

            _runSetup = true;
            _canvasView.InvalidateSurface();
        }

        #endregion
    }
}
