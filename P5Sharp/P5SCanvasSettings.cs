using SkiaSharp.Views.Maui.Controls;
using System.ComponentModel.DataAnnotations;

namespace P5Sharp
{
    public class P5SCanvasSettings()
    {
        [Required]
        public string IPAddress { get; set; }

        [Required]
        public int Port { get; set; }

        [Required]
        public List<string> Files { get; set; }

        [Required]
        public SKCanvasView CanvasView { get; set; }

        [Required]
        public SketchBase Sketch { get; set; }

        [Required]
        public bool HotReloadInDebug { get; set; } = true;
        public int FrameRate { get; set; } = 60;
        public bool EnableTouchEvents { get; set; } = true;
        public bool ReDrawOnSizeChanged { get; set; } = true;
        
        /// <summary>
        /// Run tpc server with out P5SharpSync extension. Windows only!
        /// </summary>
        public bool LocalTPCServer { get; set; } = true;
         

        
    }
}
