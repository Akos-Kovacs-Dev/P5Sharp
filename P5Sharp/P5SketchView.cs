using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using P5Sharp.P5Sharp;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace P5Sharp
{
    public class P5SketchView : ContentView
    {
        private readonly SKCanvasView _canvasView = new();
        private readonly P5SharpConfig _p5sharpConfig;

        // Fields merged from P5Sketch:
        private SketchBase _sketch;
        private bool _runSetup = true;
        private int _frameRate = 16;
        private string _serverIp;
        private int _serverPort;
        private List<string> _files;
        private bool _hotReloadInDebug = true;
        private bool _reDrawOnSizeChanged = true;
        private Server _server;
        private Client _client;
        private Dictionary<string, Action<string>> _sketchActions = new Dictionary<string, Action<string>>();

        public P5SketchView()
        {
            Content = _canvasView;

            var serviceProvider = Application.Current?.Handler?.MauiContext?.Services;
            if (serviceProvider != null)
            {
                _p5sharpConfig = serviceProvider.GetService<P5SharpConfig>();
            }
            else
            {
                throw new InvalidOperationException("Service provider not available. Ensure this is called after MAUI is initialized.");
            }

            _canvasView.PaintSurface += OnCanvasViewPaintSurface;
            Loaded += (s, e) => StartLoop();
            Unloaded += (s, e) => StopLoop();
        }

        

        #region Bindable Properties

        public static readonly BindableProperty FilesProperty =
            BindableProperty.Create(nameof(Files), typeof(List<string>), typeof(P5SketchView), new List<string>());

        public static readonly BindableProperty FilesCsvProperty =
            BindableProperty.Create(nameof(FilesCsv), typeof(string), typeof(P5SketchView), "", propertyChanged: OnFilesCsvChanged);

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

        #endregion

        #region Properties

        public List<string> Files
        {
            get => (List<string>)GetValue(FilesProperty);
            set => SetValue(FilesProperty, value);
        }

        public string FilesCsv
        {
            get => (string)GetValue(FilesCsvProperty);
            set => SetValue(FilesCsvProperty, value);
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

        #endregion

        #region Property Changed Callbacks

        private static void OnSketchChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is P5SketchView view && newValue is SketchBase sketch)
            {
                view.InitializeInternal(sketch);
            }
        }

        private static void OnFilesCsvChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is P5SketchView view && newValue is string csv)
            {
                view.Files = csv
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .ToList();
            }
        }

        #endregion

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

            InitializeSketchAndServer();
            AddEventsFromCanvasToSketch();
        }

        private void InitializeSketchAndServer()
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (_p5sharpConfig.LocalTPCServer)
                {
                    if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                    {
                        if (!Directory.Exists(_p5sharpConfig.ProjectPath))
                        {
                            throw new Exception("Project path invalid!");
                        }

                        Func<string, Task> logCallback = async (message) =>
                        {
                            Console.WriteLine($"Log: {message}");
                            await Task.CompletedTask;
                        };

                        _server = new Server(_serverPort, logCallback);
                        _ = Task.Run(async () =>
                        {
                            await _server.StartAsync(_p5sharpConfig.ProjectPath);
                        });
                    }
                }
              

                if (_hotReloadInDebug)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(_serverIp))
                            throw new Exception("HotReload in debug missing IP");

                        if (_serverPort == 0)
                            throw new Exception("HotReload in debug missing port");

                          _client = new Client(_serverIp, _serverPort);
                        _ = _client.StartTcpCommunicationAsync(_files, GetSketchCallback());
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"TCP Client Failed: {ex.Message}");
                    }
                }
            });
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            var info = e.Info;

            try
            {
                if (_runSetup)
                {
                    _sketch?.OnSetup(canvas, info);
                    _sketchActions = _sketch?.SketchActions;
                    _runSetup = false;
                }

                P5Variables memory = _sketch ?? new P5Variables();
                memory._canvas = canvas;
                memory._info = info;

                _sketch?.OnDraw(memory);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error during drawing: {ex}");
            }
        }

        private void AddEventsFromCanvasToSketch()
        {
            if (_canvasView.EnableTouchEvents)
            {
                _canvasView.Touch += (s, e) =>
                {
                    var point = e.Location;

                    switch (e.ActionType)
                    {
                        case SKTouchAction.Pressed:
                            _sketch?.OnMouseDown(point.X, point.Y);
                            _sketch?.OnTouchStart(point.X, point.Y);
                            break;

                        case SKTouchAction.Moved:
                            _sketch?.OnMouseMove(point.X, point.Y);
                            _sketch?.OnTouchMove(point.X, point.Y);
                            break;

                        case SKTouchAction.Released:
                        case SKTouchAction.Cancelled:
                            _sketch?.OnMouseUp(point.X, point.Y);
                            _sketch?.OnTouchEnd(point.X, point.Y);
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
                    _canvasView?.InvalidateSurface();
                };
            }
        }

        public Func<string, Task> GetSketchCallback()
        {
            return async (string code) =>
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

            if (HasSyntaxErrors(code))
                return await RunErrorSketch("Syntax error detected in the code.");

            if (string.IsNullOrEmpty(code))
                return await RunErrorSketch("Sketch data is empty!");

            try
            {
                var r = await CSharpScript.EvaluateAsync<SketchBase>(code, options);
                return r;
            }
            catch (CompilationErrorException ex)
            {
                string errormessage = string.Join(Environment.NewLine, ex.Diagnostics.Select(d => d.ToString()));
                return await RunErrorSketch(errormessage);
            }
            catch (Exception ex)
            {
                return await RunErrorSketch(ex.Message);
            }
        }

        private bool HasSyntaxErrors(string code)
        {
            var tree = CSharpSyntaxTree.ParseText(code);
            var diagnostics = tree.GetDiagnostics();
            return diagnostics.Any(d => d.Severity == DiagnosticSeverity.Error);
        }

        private async Task<SketchBase> RunErrorSketch(string message)
        {
            var errorCode = SketchMessages.ErrorSketch(message);
            var options = ScriptOptions.Default
                .AddReferences(typeof(SKCanvas).Assembly, typeof(SketchBase).Assembly)
                .AddImports("System", "SkiaSharp");

            return await CSharpScript.EvaluateAsync<SketchBase>(errorCode, options);
        }

        /// <summary>
        /// Call this to invoke an action defined in the sketch (e.g. triggered from UI)
        /// </summary>
        public void InvokeSketchAction(string actionName, string arg = "")
        {
            if (_sketchActions != null && _sketchActions.TryGetValue(actionName, out var action))
            {
                action?.Invoke(arg);
            }
        }


        private bool _isRunning = false;

        /// <summary>
        /// Starts the redraw loop (if needed)
        /// </summary>
        private void StartLoop()
        {
            _isRunning = true;
            _client.ResumeAsync();
            _canvasView.Dispatcher.StartTimer(TimeSpan.FromMilliseconds(1000.0 / _frameRate), () =>
            {
                if (!_isRunning)
                    return false; // stops the timer

                try
                {
                    _canvasView?.InvalidateSurface();
                    return true; // keep running
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"CanvasView invalidate failed: {ex.Message}");
                    return false; // stop timer on error
                }
            });
        }

        /// <summary>
        /// Stops redraw loop
        /// </summary>
        private void StopLoop()
        {
            _isRunning = false;
            _client.Pause();
            
        }
    }
}
