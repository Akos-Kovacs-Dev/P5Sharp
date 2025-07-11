using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
 

namespace P5Sharp
{
    public class P5Sketch
    {


        public Dictionary<string, Action<string>> SketchActions { get; set; }
        private SketchBase _sketch;
        private bool _runSetup = true;
        private SKCanvasView _canvasView;
        private int _frameRate = 16;        
        private readonly string _serverIp;
        private readonly int _serverPort;
        private readonly List<string> _files;       
        private bool _hotReloadInDebug = true;
        private bool _reDrawOnSizeChanged = true;
        public bool SKCanvasViewIsVisible => _canvasView.IsVisible;
        public bool SKCanvasViewVisible(bool isVisible) => _canvasView.IsVisible = isVisible;

        private Server server;

        public P5Sketch(P5SCanvasSettings settings)
        {

            _serverIp = settings.IPAddress;
            _serverPort = settings.Port;
            _files = settings.Files;
            _canvasView = settings.CanvasView;
            _hotReloadInDebug = settings.HotReloadInDebug;
            _frameRate = settings.FrameRate;
            SketchActions = settings.Sketch.SketchActions;
            _sketch = settings.Sketch;
            _canvasView.EnableTouchEvents = settings.EnableTouchEvents;
            _reDrawOnSizeChanged = settings.ReDrawOnSizeChanged;

       

            Initialize(settings);

        }

        private void Initialize(P5SCanvasSettings settings)
        {
            MainThread.BeginInvokeOnMainThread(async() =>
            {


                if (settings.LocalTPCServer)
                {
                    if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                    {

                        if (!Directory.Exists(settings.ProjectPath))
                        {
                            throw new Exception("Project path invalid!");
                        }
                        Func<string, Task> logCallback = async (message) =>
                        {
                            Console.WriteLine($"Log: {message}");
                            await Task.CompletedTask;
                        };

                        server = new Server(_serverPort, logCallback);
                        _=Task.Run(async() =>
                        {
                            await server.StartAsync(settings.ProjectPath);
                        });
                     
                    }
                }




                _canvasView.PaintSurface += OnCanvasViewPaintSurface;
                _canvasView.Dispatcher.StartTimer(TimeSpan.FromMilliseconds(1000.0 / _frameRate), () =>
                {
                    try
                    {
                        _canvasView?.InvalidateSurface();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"CanvasView invalidate failed: {ex.Message}");
                        return false; // Stop timer
                    }
                });


#if DEBUG 
                if (_hotReloadInDebug)
                {

                    try
                    {

                        if (string.IsNullOrEmpty(_serverIp))
                        {

                            throw new Exception("HotReload in debug missing IP");
                        }

                        if (_serverPort == 0)
                        {
                            throw new Exception("HotReload in debug missing port");
                        }

                        var server = new Client(_serverIp, _serverPort);
                        _ = server.StartTcpCommunicationAsync(_files, GetSketchCallback());
                    }
                    catch (Exception ex)
                    {

                        System.Diagnostics.Debug.WriteLine($"TCP Client Failed: {ex.Message}");
                    }

                }
#endif


            });

            AddEventsFromCanvasToSketch();

        }

        private async void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            var info = e.Info;

            try
            {
                if (_runSetup)
                {
                    _sketch?.OnSetup(canvas,info);
                    SketchActions = _sketch?.SketchActions;
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
                _canvasView.Touch += (s, e) =>
                {
                    var point = e.Location;

                    switch (e.ActionType)
                    {
                        case SKTouchAction.Pressed:
                            _sketch.OnMouseDown(point.X, point.Y);
                            _sketch.OnTouchStart(point.X, point.Y);
                            break;

                        case SKTouchAction.Moved:
                            _sketch.OnMouseMove(point.X, point.Y);
                            _sketch.OnTouchMove(point.X, point.Y);
                            break;

                        case SKTouchAction.Released:
                        case SKTouchAction.Cancelled:
                            _sketch.OnMouseUp(point.X, point.Y);
                            _sketch.OnTouchEnd(point.X, point.Y);
                            break;
                    }

                    e.Handled = true;
                };

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

            // Step 1: Validate Syntax First
            if (HasSyntaxErrors(code))
            {
                return await RunErrorSketch("Syntax error detected in the code.");
            }

            if (string.IsNullOrEmpty(code))
            {
                return await RunErrorSketch("Sketch data is empty!");
            }
            try
            {
                var r = await CSharpScript.EvaluateAsync<SketchBase>(code, options);
                return r;
            }
            catch (CompilationErrorException ex)
            {
                string errormessage = string.Join("",ex.Diagnostics.Select(d => d.ToString()));
                return await RunErrorSketch(errormessage);
                 
            }
            catch (Exception ex)
            {
                return await RunErrorSketch(ex.Message);
            }

        }
        // Syntax check using Roslyn
        private bool HasSyntaxErrors(string code)
        {
            var tree = CSharpSyntaxTree.ParseText(code);
            var diagnostics = tree.GetDiagnostics();
            return diagnostics.Any(d => d.Severity == DiagnosticSeverity.Error);
        }

        // Handles fallback to ErrorSketch
        private async Task<SketchBase> RunErrorSketch(string message)
        {
            var errorCode = SketchMessages.ErrorSketch(message);
            var options = ScriptOptions.Default
                .AddReferences(typeof(SKCanvas).Assembly, typeof(SketchBase).Assembly)
                .AddImports("System", "SkiaSharp");

            return await CSharpScript.EvaluateAsync<SketchBase>(errorCode, options);
        }


    }
}

