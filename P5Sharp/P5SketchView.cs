namespace P5Sharp
{
    using Microsoft.Extensions.DependencyInjection;
    using SkiaSharp.Views.Maui.Controls;


    public class P5SketchView : ContentView
    {
        private readonly SKCanvasView _canvasView = new();
        public P5Sketch P5Sketch;
        private readonly P5SharpConfig _p5sharpConfig;
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
                view.InitializeInternal(sketch, view._p5sharpConfig);
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

        private void InitializeInternal(SketchBase sketch, P5SharpConfig p5SharpConfig)
        {
            var settings = new P5SCanvasSettings
            {
                IPAddress = p5SharpConfig.IP,
                Port = p5SharpConfig.Port,
                ProjectPath = p5SharpConfig.ProjectPath,
                Files = Files,
                Sketch = sketch,
                CanvasView = _canvasView,
                HotReloadInDebug = HotReloadInDebug,
                FrameRate = FrameRate,
                EnableTouchEvents = EnableTouchEvents,
                ReDrawOnSizeChanged = ReDrawOnSizeChanged,
                LocalTPCServer = p5SharpConfig.LocalTPCServer
            };

            if (Files is null)
            {
                throw new Exception("No Sketch file attached to P5SketchView.");
            }

            if (settings.LocalTPCServer)
            {

                // Ensure ProjectPath is provided
                if (string.IsNullOrWhiteSpace(settings.ProjectPath))
                {
                    throw new ArgumentException("ProjectPath is required when using LocalTPCServer.", nameof(settings.ProjectPath));
                }

                // Ensure directory exists
                if (!Directory.Exists(settings.ProjectPath))
                {
                    throw new DirectoryNotFoundException($"The directory at path '{settings.ProjectPath}' does not exist.");
                }

                if (Files != null && Files.Any())
                {
                    foreach (var file in Files)
                    {
               
                        string fullPath = Path.Combine(settings.ProjectPath, file)
                                         .Replace('/', Path.DirectorySeparatorChar)
                                         .Replace('\\', Path.DirectorySeparatorChar);
                                                      
                        if (!File.Exists(fullPath))
                        {
                            throw new FileNotFoundException($"The sketch file '{file}' does not exist.", file);
                        }
                    }
                }
            }

            P5Sketch = new P5Sketch(settings);
        }

        public void InvokeSketchAction(string actionName, string argument = "")
        {
            if (P5Sketch?.SketchActions?.TryGetValue(actionName, out var action) == true)
                action.Invoke(argument);
        }
    }



}
