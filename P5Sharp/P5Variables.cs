using SkiaSharp;
using System.Diagnostics;

namespace P5Sharp
{
    /// <summary>
    /// Core configuration and state container for P5Sharp.
    /// </summary>
    public class P5Variables
    {
        /// <summary>Current drawing canvas.</summary>
        public SKCanvas? _canvas;

        /// <summary>Canvas image information such as width and height.</summary>
        public SKImageInfo _info;

        /// <summary>Fill paint used for shapes.</summary>
        public SKPaint _fillPaint = new() { IsAntialias = true, Style = SKPaintStyle.Fill, Color = SKColors.White };

        /// <summary>Stroke paint used for shapes.</summary>
        public SKPaint _strokePaint = new() { IsAntialias = true, Style = SKPaintStyle.Stroke, StrokeWidth = 1, StrokeCap = SKStrokeCap.Round };

        /// <summary>Point rendering mode.</summary>
        public SKPointMode _pointMode = SKPointMode.Lines;

        /// <summary>Current global angle mode (Radians or Degrees).</summary>
        public static AngleModeType _angleMode = AngleModeType.RADIANS;

        /// <summary>Rectangle drawing mode.</summary>
        public Rectmode _rectMode = Rectmode.CORNER;

        /// <summary>Whether anti-aliasing is enabled.</summary>
        public bool _isSmooth = true;

        /// <summary>Text alignment mode.</summary>
        public TextAlignment _textAlignment = TextAlignment.LEFT;

        /// <summary>SKFont</summary>
        public SKFont _font = new SKFont() { Size = 16 };

        /// <summary>Ellipse drawing mode.</summary>
        public EllipseMode _ellipseMode = EllipseMode.CENTER;

        /// <summary>Current shape mode for beginShape/endShape.</summary>
        public ShapeMode _currentShapeMode = ShapeMode.POLYGON;

        /// <summary>List of shape vertices for current custom shape.</summary>
        public List<SKPoint> _shapeVertices = new();

        /// <summary>Whether a shape is currently being defined with beginShape().</summary>
        public bool _isShapeOpen = false;

        /// <summary>Whether the shape will be closed when endShape() is called.</summary>
        public bool _isShapeClosed = false;

        /// <summary>Canvas width in pixels.</summary>
        public int Width => _info.Width;

        /// <summary>Canvas height in pixels.</summary>
        public int Height => _info.Height;

        /// <summary>Current mouse X position.</summary>
        public float MouseX { get; set; }

        /// <summary>Current mouse Y position.</summary>
        public float MouseY { get; set; }

        /// <summary>Whether the mouse is currently pressed.</summary>
        public bool MousePressed { get; set; }

        /// <summary>Whether the touch is currently pressed (if on touch device).</summary>
        public bool TouchPressed { get; set; }

        /// <summary>Current touch X position.</summary>
        public float TouchX { get; set; }

        /// <summary>Current touch Y position.</summary>
        public float TouchY { get; set; }

        /// <summary>Collection of predefined named colors accessible by string.</summary>
        public static readonly Dictionary<string, SKColor> NamedColors = new(StringComparer.OrdinalIgnoreCase)
        {
            // Transparent
            { "transparent", SKColors.Transparent },

            // Grayscale
            { "black", SKColors.Black },
            { "white", SKColors.White },
            { "gray", new SKColor(128, 128, 128) },
            { "grey", new SKColor(128, 128, 128) },
            { "lightgray", new SKColor(211, 211, 211) },
            { "darkgray", new SKColor(64, 64, 64) },

            // Reds
            { "red", SKColors.Red },
            { "darkred", new SKColor(139, 0, 0) },
            { "salmon", new SKColor(250, 128, 114) },
            { "crimson", new SKColor(220, 20, 60) },

            // Oranges
            { "orange", new SKColor(255, 165, 0) },
            { "darkorange", new SKColor(255, 140, 0) },
            { "coral", new SKColor(255, 127, 80) },

            // Yellows
            { "yellow", SKColors.Yellow },
            { "gold", new SKColor(255, 215, 0) },
            { "khaki", new SKColor(240, 230, 140) },

            // Greens
            { "green", SKColors.Green },
            { "darkgreen", new SKColor(0, 100, 0) },
            { "lime", new SKColor(0, 255, 0) },
            { "olive", new SKColor(128, 128, 0) },
            { "springgreen", new SKColor(0, 255, 127) },

            // Blues
            { "blue", SKColors.Blue },
            { "navy", new SKColor(0, 0, 128) },
            { "skyblue", new SKColor(135, 206, 235) },
            { "dodgerblue", new SKColor(30, 144, 255) },

            // Cyans / Teals
            { "cyan", SKColors.Cyan },
            { "aqua", new SKColor(0, 255, 255) },
            { "teal", new SKColor(0, 128, 128) },

            // Purples / Violets
            { "purple", new SKColor(128, 0, 128) },
            { "violet", new SKColor(238, 130, 238) },
            { "indigo", new SKColor(75, 0, 130) },

            // Pinks
            { "pink", new SKColor(255, 192, 203) },
            { "hotpink", new SKColor(255, 105, 180) },
            { "deeppink", new SKColor(255, 20, 147) },

            // Browns
            { "brown", new SKColor(139, 69, 19) },
            { "sienna", new SKColor(160, 82, 45) },
            { "chocolate", new SKColor(210, 105, 30) },
            { "tan", new SKColor(210, 180, 140) }
        };

        #region Enums

        /// <summary>Type of gradient to use in fill operations.</summary>
        public enum GradientType { Linear, Radial }

        /// <summary>Units for measuring angles.</summary>
        public enum AngleModeType { RADIANS, DEGREES }

        /// <summary>Style of stroke joints.</summary>
        public enum StrokeJoinMode { MITER, BEVEL, ROUND }

        /// <summary>Style of stroke end caps.</summary>
        public enum StrokeCapMode { ROUND, SQUARE, PROJECT }

        /// <summary>Rectangle drawing reference mode.</summary>
        public enum Rectmode { CORNER, CORNERS, CENTER, RADIUS }

        /// <summary>Arc drawing style.</summary>
        public enum ArcMode { OPEN, CHORD, PIE }

        /// <summary>End shape closure option.</summary>
        public enum EndShapeMode { OPEN, CLOSE }

        /// <summary>Horizontal text alignment.</summary>
        public enum TextAlignment { LEFT, RIGHT, CENTER }

        /// <summary>Mode for shape drawing.</summary>
        public enum ShapeMode
        {
            POLYGON,
            POINTS,
            LINES,
            TRIANGLES,
            TRIANGLE_STRIP,
            TRIANGLE_FAN,
            QUADS,
            QUAD_STRIP
        }

        /// <summary>Ellipse reference mode.</summary>
        public enum EllipseMode { CENTER, RADIUS, CORNER, CORNERS }

        #endregion
    }
}
