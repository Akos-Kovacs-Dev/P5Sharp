using SkiaSharp;
using System.Reflection;

namespace P5Sharp
{
    public class P5Shapes : P5Math
    {
        #region Fill   
        /// <summary>
        /// Disables filling by setting the fill color to fully transparent.
        /// Subsequent shapes will only be drawn with a stroke (if enabled), and no interior fill.
        /// </summary>
        public void NoFill()
        {
            _fillPaint.Color = SKColors.Transparent;
            _fillPaint.Shader = null;
        }
        /// <summary>
        /// Disables filling by setting the fill color to fully transparent.
        /// Subsequent shapes will only be drawn with a stroke (if enabled), and no interior fill.
        /// </summary>
        public void noFill() => NoFill();
        /// <summary>
        /// Sets fill color using RGB float values.
        /// </summary>
        public void Fill(float r, float g, float b)
        {
            _fillPaint.Color = new SKColor((byte)r, (byte)g, (byte)b);
            _fillPaint.Shader = null;
        }
        /// <summary>
        /// Sets fill color using RGB float values.
        /// </summary>
        public void fill(float r, float g, float b) => Fill(r, g, b);
        /// <summary>
        /// Sets fill color using RGBA float values.
        /// </summary>
        public void Fill(float r, float g, float b, float a)
        {
            _fillPaint.Color = new SKColor((byte)r, (byte)g, (byte)b, (byte)a);
            _fillPaint.Shader = null;
        }
        /// <summary>
        /// Sets fill color using RGBA float values.
        /// </summary>
        public void fill(float r, float g, float b, float a) => Fill(r, g, b, a);
        /// <summary>
        /// Sets fill color using RGB int values.
        /// </summary>
        public void Fill(int r, int g, int b)
        {
            _fillPaint.Color = new SKColor((byte)r, (byte)g, (byte)b);
            _fillPaint.Shader = null;
        }
        /// <summary>
        /// Sets fill color using RGB int values.
        /// </summary>
        public void fill(int r, int g, int b) => Fill(r, g, b);
        /// <summary>
        /// Sets grayscale fill using int value.
        /// </summary>
        public void Fill(int c)
        {
            _fillPaint.Color = new SKColor((byte)c, (byte)c, (byte)c);
            _fillPaint.Shader = null;
        }
        /// <summary>
        /// Sets grayscale fill using int value.
        /// </summary>
        public void fill(int c) => Fill(c);
        /// <summary>
        /// Sets RGBA fill color using int values.
        /// </summary>
        public void Fill(int r, int g, int b, int a)
        {
            _fillPaint.Color = new SKColor((byte)r, (byte)g, (byte)b, (byte)a);
            _fillPaint.Shader = null;
        }
        /// <summary>
        /// Sets RGBA fill color using int values.
        /// </summary>
        public void fill(int r, int g, int b, int a) => Fill(r, g, b, a);
        /// <summary>
        /// Sets fill color using RGB byte values.
        /// </summary>
        public void Fill(byte r, byte g, byte b)
        {
            _fillPaint.Color = new SKColor(r, g, b);
            _fillPaint.Shader = null;
        }
        /// <summary>
        /// Sets fill color using RGB byte values.
        /// </summary>
        public void fill(byte r, byte g, byte b) => Fill(r, g, b);
        /// <summary>
        /// Sets fill color using RGBA byte values.
        /// </summary>
        public void Fill(byte r, byte g, byte b, byte a)
        {
            _fillPaint.Color = new SKColor(r, g, b, a);
            _fillPaint.Shader = null;
        }
        /// <summary>
        /// Sets fill color using RGBA byte values.
        /// </summary>
        public void fill(byte r, byte g, byte b, byte a) => Fill(r, g, b, a);
        /// <summary>
        /// Sets grayscale fill using byte value.
        /// </summary>
        public void Fill(byte c)
        {
            _fillPaint.Color = new SKColor(c, c, c);
            _fillPaint.Shader = null;
        }
        /// <summary>
        /// Sets grayscale fill using byte value.
        /// </summary>
        public void fill(byte c) => Fill(c);
        /// <summary>
        /// Sets grayscale fill with alpha using byte values.
        /// </summary>
        public void Fill(byte c, byte a)
        {
            _fillPaint.Color = new SKColor(c, c, c, a);
            _fillPaint.Shader = null;
        }
        /// <summary>
        /// Sets grayscale fill with alpha using byte values.
        /// </summary>
        public void fill(byte c, byte a) => Fill(c, a);
        /// <summary>
        /// Sets fill color directly using SKColor.
        /// </summary>
        public void Fill(SKColor color)
        {
            _fillPaint.Color = color;
            _fillPaint.Shader = null;
        }
        /// <summary>
        /// Sets fill color directly using SKColor.
        /// </summary>
        public void fill(SKColor color) => Fill(color);
        /// <summary>
        /// Sets fill color using a named color from dictionary.
        /// </summary>
        public void Fill(string colorName)
        {
            if (NamedColors.TryGetValue(colorName.ToLowerInvariant(), out var color))
            {
                _fillPaint.Color = color;
                _fillPaint.Shader = null;
            }
            else
            {
                throw new ArgumentException($"Unknown color name: {colorName}");
            }
        }
        /// <summary>
        /// Sets fill color using a named color from dictionary.
        /// </summary>
        public void fill(string colorName) => Fill(colorName);
        /// <summary>
        /// Sets a gradient fill for subsequent shapes, using either a linear or radial gradient.
        /// </summary>
        /// <param name="colors">An array of SKColors to use in the gradient.</param>
        /// <param name="colorStops">
        /// Optional array of color stop positions (ranging from 0.0 to 1.0).
        /// If null or mismatched in length, stops will be evenly distributed.
        /// </param>
        /// <param name="gradientType">Specifies whether the gradient is linear or radial.</param>
        /// <param name="angleDegrees">
        /// Angle (in degrees) for linear gradients, defining the direction of the gradient line.
        /// Ignored for radial gradients.
        /// </param>
        /// <param name="customCenter">
        /// Optional center point (x, y) for radial gradients.
        /// Defaults to the center of the canvas if null.
        /// </param>
        /// <param name="radialRadius">
        /// Radius for radial gradients. If zero or less, defaults to half the canvas's smallest dimension.
        /// </param>
        /// <exception cref="ArgumentException">Thrown if the colors array is null or empty.</exception>
        public void FillGradient(SKColor[] colors, float[]? colorStops = null, GradientType gradientType = GradientType.Linear, float angleDegrees = 0f, (float, float)? customCenter = null, float radialRadius = 0)
        {
            if (colors == null || colors.Length == 0)
                throw new ArgumentException("Colors array cannot be null or empty.");

            if (colorStops == null || colorStops.Length != colors.Length)
            {
                colorStops = new float[colors.Length];
                for (int i = 0; i < colors.Length; i++)
                    colorStops[i] = i / (float)(colors.Length - 1);
            }

            switch (gradientType)
            {
                case GradientType.Linear:
                    // Convert angle to radians for trig
                    double angleRad = angleDegrees * Math.PI / 180.0;

                    // Calculate half-diagonal length for gradient line (to cover whole canvas)
                    float halfDiagonal = (float)(Math.Sqrt(Width * Width + Height * Height) / 2);

                    // Center point
                    var center = new SKPoint(Width / 2f, Height / 2f);

                    // Calculate start and end points based on angle
                    var start = new SKPoint(
                        center.X + halfDiagonal * (float)Math.Cos(angleRad + Math.PI),
                        center.Y + halfDiagonal * (float)Math.Sin(angleRad + Math.PI)
                    );
                    var end = new SKPoint(
                        center.X + halfDiagonal * (float)Math.Cos(angleRad),
                        center.Y + halfDiagonal * (float)Math.Sin(angleRad)
                    );

                    _fillPaint.Shader = SKShader.CreateLinearGradient(
                        start,
                        end,
                        colors,
                        colorStops,
                        SKShaderTileMode.Clamp);
                    break;

                case GradientType.Radial:
                    var c = customCenter is null ? new SKPoint(Width / 2f, Height / 2f) : new SKPoint(customCenter.Value.Item1, customCenter.Value.Item2);
                    var radius = radialRadius > 0 ? radialRadius : Math.Min(Width, Height) / 2f;

                    _fillPaint.Shader = SKShader.CreateRadialGradient(
                        c,
                        radius,
                        colors,
                        colorStops,
                        SKShaderTileMode.Clamp);
                    break;
            }
        }
        /// <summary>
        /// Sets a gradient fill for subsequent shapes, using either a linear or radial gradient.
        /// </summary>
        /// <param name="colors">An array of SKColors to use in the gradient.</param>
        /// <param name="colorStops">
        /// Optional array of color stop positions (ranging from 0.0 to 1.0).
        /// If null or mismatched in length, stops will be evenly distributed.
        /// </param>
        /// <param name="gradientType">Specifies whether the gradient is linear or radial.</param>
        /// <param name="angleDegrees">
        /// Angle (in degrees) for linear gradients, defining the direction of the gradient line.
        /// Ignored for radial gradients.
        /// </param>
        /// <param name="customCenter">
        /// Optional center point (x, y) for radial gradients.
        /// Defaults to the center of the canvas if null.
        /// </param>
        /// <param name="radialRadius">
        /// Radius for radial gradients. If zero or less, defaults to half the canvas's smallest dimension.
        /// </param>
        /// <exception cref="ArgumentException">Thrown if the colors array is null or empty.</exception>
        public void fillGradient(SKColor[] colors, float[]? colorStops = null, GradientType gradientType = GradientType.Linear, float angleDegrees = 0f, (float, float)? customCenter = null, float radialRadius = 0) => FillGradient(colors, colorStops, gradientType, angleDegrees, customCenter, radialRadius);
        #endregion
        #region Stroke 
        /// <summary>
        /// Disables stroke by setting the stroke color to fully transparent.
        /// Any subsequent shapes will not have an outline until stroke is re-enabled.
        /// </summary>
        public void NoStroke()
        {
            _strokePaint.Color = SKColors.Transparent;
            _strokePaint.Shader = null;
        }
        /// <summary>
        /// Disables stroke by setting the stroke color to fully transparent.
        /// Any subsequent shapes will not have an outline until stroke is re-enabled.
        /// </summary>
        public void noStroke() => NoStroke();
        /// <summary>
        /// Sets stroke color as grayscale using float.
        /// </summary>
        public void Stroke(float c)
        {
            _strokePaint.Color = new SKColor((byte)c, (byte)c, (byte)c);
            _strokePaint.Shader = null;
        }
        /// <summary>
        /// Sets stroke color as grayscale using float.
        /// </summary>
        public void stroke(float c) => Stroke(c);
        /// <summary>
        /// Sets stroke color using RGB float values.
        /// </summary>
        public void Stroke(float r, float g, float b)
        {
            _strokePaint.Color = new SKColor((byte)r, (byte)g, (byte)b);
            _strokePaint.Shader = null;
        }
        /// <summary>
        /// Sets stroke color using RGB float values.
        /// </summary>
        public void stroke(float r, float g, float b) => Stroke(r, g, b);

        /// <summary>
        /// Sets stroke color using RGBA float values.
        /// </summary>
        public void Stroke(float r, float g, float b, float a)
        {
            _strokePaint.Color = new SKColor((byte)r, (byte)g, (byte)b, (byte)a);
            _strokePaint.Shader = null;
        }
        /// <summary>
        /// Sets stroke color using RGBA float values.
        /// </summary>
        public void stroke(float r, float g, float b, float a) => Stroke(r, g, b, a);
        /// <summary>
        /// Sets stroke color as grayscale using int.
        /// </summary>
        public void Stroke(int c)
        {
            _strokePaint.Color = new SKColor((byte)c, (byte)c, (byte)c);
            _strokePaint.Shader = null;
        }
        /// <summary>
        /// Sets stroke color as grayscale using int.
        /// </summary>
        public void stroke(int c) => Stroke(c);
        /// <summary>
        /// Sets stroke color using RGB int values.
        /// </summary>
        public void Stroke(int r, int g, int b)
        {
            _strokePaint.Color = new SKColor((byte)r, (byte)g, (byte)b);
            _strokePaint.Shader = null;
        }
        /// <summary>
        /// Sets stroke color using RGB int values.
        /// </summary>
        public void stroke(int r, int g, int b) => Stroke(r, g, b);
        /// <summary>
        /// Sets stroke color using RGBA int values.
        /// </summary>
        public void Stroke(int r, int g, int b, int a)
        {
            _strokePaint.Color = new SKColor((byte)r, (byte)g, (byte)b, (byte)a);
            _strokePaint.Shader = null;
        }
        /// <summary>
        /// Sets stroke color using RGBA int values.
        /// </summary>
        public void stroke(int r, int g, int b, int a) => Stroke(r, g, b, a);
        /// <summary>
        /// Sets stroke color using RGB byte values.
        /// </summary>
        public void Stroke(byte r, byte g, byte b)
        {
            _strokePaint.Color = new SKColor(r, g, b);
            _strokePaint.Shader = null;
        }
        /// <summary>
        /// Sets stroke color using RGB byte values.
        /// </summary>
        public void stroke(byte r, byte g, byte b) => Stroke(r, g, b);
        /// <summary>
        /// Sets stroke color using RGBA byte values.
        /// </summary>
        public void Stroke(byte r, byte g, byte b, byte a)
        {
            _strokePaint.Color = new SKColor(r, g, b, a);
            _strokePaint.Shader = null;
        }
        /// <summary>
        /// Sets stroke color using RGBA byte values.
        /// </summary>
        public void stroke(byte r, byte g, byte b, byte a) => Stroke(r, g, b, a);

        /// <summary>
        /// Sets stroke color as grayscale using byte.
        /// </summary>
        public void Stroke(byte c)
        {
            _strokePaint.Color = new SKColor(c, c, c);
            _strokePaint.Shader = null;
        }
        /// <summary>
        /// Sets stroke color as grayscale using byte.
        /// </summary>
        public void stroke(byte c) => Stroke(c);
        /// <summary>
        /// Sets stroke color as grayscale with alpha using bytes.
        /// </summary>
        public void Stroke(byte c, byte a)
        {
            _strokePaint.Color = new SKColor(c, c, c, a);
            _strokePaint.Shader = null;
        }
        /// <summary>
        /// Sets stroke color as grayscale with alpha using bytes.
        /// </summary>
        public void stroke(byte c, byte a) => Stroke(c, a);
        /// <summary>
        /// Sets stroke color using an SKColor.
        /// </summary>
        public void Stroke(SKColor color)
        {
            _strokePaint.Color = color;
            _strokePaint.Shader = null;
        }
        /// <summary>
        /// Sets stroke color using an SKColor.
        /// </summary>
        public void stroke(SKColor color) => Stroke(color);
        /// <summary>
        /// Sets stroke color from a named color string.
        /// </summary>
        public void Stroke(string colorName)
        {
            if (NamedColors.TryGetValue(colorName.ToLowerInvariant(), out var color))
            {
                _strokePaint.Color = color;
                _strokePaint.Shader = null;
            }
            else
            {
                throw new ArgumentException($"Unknown color name: {colorName}");
            }
        }
        /// <summary>
        /// Sets stroke color from a named color string.
        /// </summary>
        public void stroke(string colorName) => Stroke(colorName);
        /// <summary>
        /// Sets the thickness of the stroke used to draw outlines of shapes.
        /// A larger value results in a thicker stroke.
        /// </summary>
        /// <param name="weight">The stroke width in pixels.</param>
        public void StrokeWeight(float weight)
        {
            _strokePaint.StrokeWidth = weight;
        }
        /// <summary>
        /// Sets the thickness of the stroke used to draw outlines of shapes.
        /// A larger value results in a thicker stroke.
        /// </summary>
        /// <param name="weight">The stroke width in pixels.</param>
        public void strokeWeight(float weight) => StrokeWeight(weight);
        /// <summary>
        /// Sets the thickness of the stroke used to draw outlines of shapes.
        /// A larger value results in a thicker stroke.
        /// </summary>
        /// <param name="weight">The stroke width in pixels.</param>
        public void StrokeWeight(int weight)
        {
            _strokePaint.StrokeWidth = weight;
        }
        /// <summary>
        /// Sets the thickness of the stroke used to draw outlines of shapes.
        /// A larger value results in a thicker stroke.
        /// </summary>
        /// <param name="weight">The stroke width in pixels.</param>
        public void strokeWeight(int weight) => StrokeWeight(weight);
        /// <summary>
        /// Applies a gradient to the stroke (outline) of subsequent shapes, using either a linear or radial gradient.
        /// </summary>
        /// <param name="colors">An array of <see cref="SKColor"/> values to define the gradient.</param>
        /// <param name="colorStops">
        /// Optional array of stop positions (from 0.0 to 1.0) for each color.
        /// If null or mismatched in length, stops are automatically distributed evenly across the gradient.
        /// </param>
        /// <param name="gradientType">
        /// Specifies the type of gradient to apply: <see cref="GradientType.Linear"/> or <see cref="GradientType.Radial"/>.
        /// </param>
        /// <param name="angleDegrees">
        /// The angle (in degrees) for the direction of the linear gradient. Ignored for radial gradients.
        /// </param>
        /// <param name="customCenter">
        /// Optional center point (x, y) for radial gradients. If not provided, defaults to the canvas center.
        /// </param>
        /// <param name="radialRadius">
        /// The radius for radial gradients. If zero or negative, the radius defaults to half the canvas's smallest dimension.
        /// </param>
        /// <exception cref="ArgumentException">Thrown if the colors array is null or empty.</exception>
        public void StrokeGradient(SKColor[] colors, float[]? colorStops = null, GradientType gradientType = GradientType.Linear, float angleDegrees = 0f, (float, float)? customCenter = null, float radialRadius = 0)
        {
            if (colors == null || colors.Length == 0)
                throw new ArgumentException("Colors array cannot be null or empty.");

            if (colorStops == null || colorStops.Length != colors.Length)
            {
                colorStops = new float[colors.Length];
                for (int i = 0; i < colors.Length; i++)
                    colorStops[i] = i / (float)(colors.Length - 1);
            }

            switch (gradientType)
            {
                case GradientType.Linear:
                    // Convert angle to radians for trig
                    double angleRad = angleDegrees * Math.PI / 180.0;

                    // Calculate half-diagonal length for gradient line (to cover whole canvas)
                    float halfDiagonal = (float)(Math.Sqrt(Width * Width + Height * Height) / 2);

                    // Center point
                    var center = new SKPoint(Width / 2f, Height / 2f);

                    // Calculate start and end points based on angle
                    var start = new SKPoint(
                        center.X + halfDiagonal * (float)Math.Cos(angleRad + Math.PI),
                        center.Y + halfDiagonal * (float)Math.Sin(angleRad + Math.PI)
                    );
                    var end = new SKPoint(
                        center.X + halfDiagonal * (float)Math.Cos(angleRad),
                        center.Y + halfDiagonal * (float)Math.Sin(angleRad)
                    );

                    _strokePaint.Shader = SKShader.CreateLinearGradient(
                        start,
                        end,
                        colors,
                        colorStops,
                        SKShaderTileMode.Clamp);
                    break;

                case GradientType.Radial:
                    var c = customCenter is null ? new SKPoint(Width / 2f, Height / 2f) : new SKPoint(customCenter.Value.Item1, customCenter.Value.Item2);
                    var radius = radialRadius > 0 ? radialRadius : Math.Min(Width, Height) / 2f;

                    _strokePaint.Shader = SKShader.CreateRadialGradient(
                        c,
                        radius,
                        colors,
                        colorStops,
                        SKShaderTileMode.Clamp);
                    break;
            }
        }
        /// <summary>
        /// Applies a gradient to the stroke (outline) of subsequent shapes, using either a linear or radial gradient.
        /// </summary>
        /// <param name="colors">An array of <see cref="SKColor"/> values to define the gradient.</param>
        /// <param name="colorStops">
        /// Optional array of stop positions (from 0.0 to 1.0) for each color.
        /// If null or mismatched in length, stops are automatically distributed evenly across the gradient.
        /// </param>
        /// <param name="gradientType">
        /// Specifies the type of gradient to apply: <see cref="GradientType.Linear"/> or <see cref="GradientType.Radial"/>.
        /// </param>
        /// <param name="angleDegrees">
        /// The angle (in degrees) for the direction of the linear gradient. Ignored for radial gradients.
        /// </param>
        /// <param name="customCenter">
        /// Optional center point (x, y) for radial gradients. If not provided, defaults to the canvas center.
        /// </param>
        /// <param name="radialRadius">
        /// The radius for radial gradients. If zero or negative, the radius defaults to half the canvas's smallest dimension.
        /// </param>
        /// <exception cref="ArgumentException">Thrown if the colors array is null or empty.</exception>
        public void strokeGradient(SKColor[] colors, float[]? colorStops = null, GradientType gradientType = GradientType.Linear, float angleDegrees = 0f, (float, float)? customCenter = null, float radialRadius = 0) => StrokeGradient(colors, colorStops, gradientType, angleDegrees, customCenter, radialRadius);
        #endregion
        #region Stroke Cap
        /// <summary>
        /// Sets the style of the stroke cap, which defines how the ends of lines are drawn.
        /// </summary>
        /// <param name="cap">
        /// The cap style to apply:
        /// <list type="bullet">
        ///   <item><term><c>ROUND</c></term><description>Rounded ends.</description></item>
        ///   <item><term><c>SQUARE</c></term><description>Square ends that extend past the line end.</description></item>
        ///   <item><term><c>PROJECT</c></term><description>Flat ends (also known as 'butt' cap).</description></item>
        /// </list>
        /// </param>
        public void StrokeCap(StrokeCapMode cap)
        {
            switch (cap)
            {
                case StrokeCapMode.ROUND:
                    _strokePaint.StrokeCap = SKStrokeCap.Round;
                    break;
                case StrokeCapMode.SQUARE:
                    _strokePaint.StrokeCap = SKStrokeCap.Square;
                    break;
                case StrokeCapMode.PROJECT:
                    _strokePaint.StrokeCap = SKStrokeCap.Butt;
                    break;
            }
        }
        /// <summary>
        /// Sets the style of the stroke cap, which defines how the ends of lines are drawn.
        /// </summary>
        /// <param name="cap">
        /// The cap style to apply:
        /// <list type="bullet">
        ///   <item><term><c>ROUND</c></term><description>Rounded ends.</description></item>
        ///   <item><term><c>SQUARE</c></term><description>Square ends that extend past the line end.</description></item>
        ///   <item><term><c>PROJECT</c></term><description>Flat ends (also known as 'butt' cap).</description></item>
        /// </list>
        /// </param>
        public void strokeCap(StrokeCapMode cap) => StrokeCap(cap);
        #endregion
        #region Stroke Join
        /// <summary>
        /// Sets the style of the stroke join, which determines how corners are rendered where two lines meet.
        /// </summary>
        /// <param name="join">
        /// The join style to apply:
        /// <list type="bullet">
        ///   <item><term><c>MITER</c></term><description>Sharp corners with extended outer edges.</description></item>
        ///   <item><term><c>BEVEL</c></term><description>Flat, beveled corners by cutting off the corner.</description></item>
        ///   <item><term><c>ROUND</c></term><description>Rounded corners with a circular arc.</description></item>
        /// </list>
        /// </param>
        public void StrokeJoin(StrokeJoinMode join)
        {
            switch (join)
            {
                case StrokeJoinMode.MITER:
                    _strokePaint.StrokeJoin = SKStrokeJoin.Miter;
                    break;
                case StrokeJoinMode.BEVEL:
                    _strokePaint.StrokeJoin = SKStrokeJoin.Bevel;
                    break;
                case StrokeJoinMode.ROUND:
                    _strokePaint.StrokeJoin = SKStrokeJoin.Round;
                    break;
            }
        }
        /// <summary>
        /// Sets the style of the stroke join, which determines how corners are rendered where two lines meet.
        /// </summary>
        /// <param name="join">
        /// The join style to apply:
        /// <list type="bullet">
        ///   <item><term><c>MITER</c></term><description>Sharp corners with extended outer edges.</description></item>
        ///   <item><term><c>BEVEL</c></term><description>Flat, beveled corners by cutting off the corner.</description></item>
        ///   <item><term><c>ROUND</c></term><description>Rounded corners with a circular arc.</description></item>
        /// </list>
        /// </param>
        public void strokeJoin(StrokeJoinMode join) => StrokeJoin(join);

        #endregion
        #region Smoth
        /// <summary>
        /// Enables smoothing (anti-aliasing) for both fill and stroke paint,
        /// resulting in higher-quality edges and curves.
        /// </summary>
        /// <remarks>
        /// This improves visual rendering by reducing jagged edges,
        /// especially noticeable on diagonal lines and curves.
        /// </remarks>
        public void Smooth()
        {
            _isSmooth = true;
            _fillPaint.IsAntialias = true;
            _strokePaint.IsAntialias = true;
        }
        /// <summary>
        /// Enables smoothing (anti-aliasing) for both fill and stroke paint,
        /// resulting in higher-quality edges and curves.
        /// </summary>
        /// <remarks>
        /// This improves visual rendering by reducing jagged edges,
        /// especially noticeable on diagonal lines and curves.
        /// </remarks>
        public void smooth() => Smooth();
        /// <summary>
        /// Disables smoothing (anti-aliasing) for both fill and stroke paint,
        /// resulting in sharper but potentially jagged edges.
        /// </summary>
        /// <remarks>
        /// This may improve performance slightly, but visual quality can decrease,
        /// especially for curves and diagonal lines.
        /// </remarks>
        public void NoSmooth()
        {
            _isSmooth = false;
            _fillPaint.IsAntialias = false;
            _strokePaint.IsAntialias = false;
        }
        /// <summary>
        /// Disables smoothing (anti-aliasing) for both fill and stroke paint,
        /// resulting in sharper but potentially jagged edges.
        /// </summary>
        /// <remarks>
        /// This may improve performance slightly, but visual quality can decrease,
        /// especially for curves and diagonal lines.
        /// </remarks>
        public void noSmooth() => NoSmooth();
        #endregion
        #region Text
        /// <summary>
        /// Loads a font from the app package using its filename and sets it to the SKFont instance.
        /// Falls back to the default typeface if loading fails.
        /// </summary>
        /// <param name="fontRealName">The exact filename of the font in the app package (e.g., "MyFont.ttf").</param>
        public async Task LoadFont(string fontRealName)
        {
            using (Stream fileStream = await FileSystem.Current.OpenAppPackageFileAsync(fontRealName))
            {
                _font.Typeface = SKTypeface.FromStream(fileStream);

                if (_font.Typeface == null)
                    _font.Typeface = SKTypeface.CreateDefault();
            }
        }
        /// <summary>
        /// Loads a font from the app package using its filename and sets it to the SKFont instance.
        /// Falls back to the default typeface if loading fails.
        /// </summary>
        /// <param name="fontRealName">The exact filename of the font in the app package (e.g., "MyFont.ttf").</param>
        public async Task loadFont(string fontRealName) => LoadFont(fontRealName);
        /// <summary>
        /// Sets the horizontal alignment for text rendering.
        /// </summary>
        /// <param name="textAlignment">The desired text alignment (LEFT, CENTER, RIGHT).</param>
        public void TextAlign(TextAlignment textAlignment)
        {
            _textAlignment = textAlignment;
        }
        public void textAlign(TextAlignment textAlignment) => TextAlign(textAlignment);
        /// <summary>
        /// Draws the specified text centered within the entire canvas.
        /// </summary>
        /// <param name="txt">The text to draw.</param>
        public async void Text(string txt)
        {
            SKRect rect = new SKRect() { Left = 0, Bottom = Height, Top = 0, Right = Width };
           
            var bounds = new SKRect();
            _font.MeasureText(txt, out bounds);
            // Calculate coordinates to center the text inside the canvas
            float x = rect.Left + (rect.Width - bounds.Width) / 2 - bounds.Left;
            float y = rect.Top + (rect.Height + bounds.Height) / 2 - bounds.Bottom;

       

            _canvas.DrawText(txt, x, y,_font, _fillPaint);
        }
        /// <summary>
        /// Draws the specified text at the given (x, y) coordinates, 
        /// applying the current text alignment.
        /// </summary>
        /// <param name="txt">The text to draw.</param>
        /// <param name="x">The x-coordinate for text positioning.</param>
        /// <param name="y">The y-coordinate for text positioning.</param>
        public void Text(string txt, float x, float y)
        {
            
            var blob = SKTextBlob.Create(txt, _font);
            var bounds = new SKRect();
            _font.MeasureText(txt, out bounds);

            float adjustedX = x;

            // Adjust x-coordinate based on alignment
            if (_textAlignment == TextAlignment.CENTER)
                adjustedX -= bounds.Width / 2;
            else if (_textAlignment == TextAlignment.RIGHT)
                adjustedX -= bounds.Width;

            _canvas.DrawText(blob, adjustedX, y, _fillPaint);
        }
        public void text(string txt, float x, float y) => Text(txt, x, y);
        /// <summary>
        /// Draws the specified text at the given (x, y) coordinates, 
        /// applying the current text alignment.
        /// </summary>
        /// <param name="txt">The text to draw.</param>
        /// <param name="x">The x-coordinate for text positioning.</param>
        /// <param name="y">The y-coordinate for text positioning.</param>
        public void Text(string txt, int x, int y) => Text(txt, (float)x, (float)y);
        /// <summary>
        /// Draws the specified text at the given (x, y) coordinates, 
        /// applying the current text alignment.
        /// </summary>
        /// <param name="txt">The text to draw.</param>
        /// <param name="x">The x-coordinate for text positioning.</param>
        /// <param name="y">The y-coordinate for text positioning.</param>
        public void text(string txt, int x, int y) => Text(txt, (float)x, (float)y);
        /// <summary>
        /// Sets the font size used for text rendering (integer version).
        /// </summary>
        /// <param name="size">Font size in points.</param>
        public void TextSize(int size)
        {
            _font.Size = size;
        }
        /// <summary>
        /// Sets the font size used for text rendering (integer version).
        /// </summary>
        /// <param name="size">Font size in points.</param>
        public void textSize(int size) => TextSize(size);
        /// <summary>
        /// Sets the font size used for text rendering (float version).
        /// </summary>
        /// <param name="size">Font size in points.</param>
        public void TextSize(float size) => _font.Size = size;
        /// <summary>
        /// Sets the font size used for text rendering (float version).
        /// </summary>
        /// <param name="size">Font size in points.</param>
        public void textSize(float size) => TextSize(size);
        #endregion
        #region arc
        private float RadiansToDegrees(float radians) => radians * (180f / (float)Math.PI);
        public void Arc(float x, float y, float w, float h, float start, float stop, ArcMode mode = ArcMode.PIE)
        {
            // Convert to degrees and flip direction (CCW to CW), then shift by -90 degrees to align p5.js 0 at 3 o'clock
            float startDeg = -RadiansToDegrees(start);
            float sweepDeg = -RadiansToDegrees(stop - start);

            var rect = new SKRect(x - w / 2, y - h / 2, x + w / 2, y + h / 2);
            bool useCenter = mode == ArcMode.PIE;

            if (_fillPaint.Color.Alpha > 0)
                _canvas.DrawArc(rect, startDeg, sweepDeg, useCenter, _fillPaint);

            if (_strokePaint.Color.Alpha > 0)
                _canvas.DrawArc(rect, startDeg, sweepDeg, false, _strokePaint);
        }

        public void arc(float x, float y, float w, float h, float start, float stop, ArcMode mode = ArcMode.PIE) => Arc(x, y, w, h, start, stop, mode);

        #endregion
        #region Circle
        /// <summary>
        /// Draws a filled circle at the specified (x, y) coordinates with the given radius.
        /// If stroke paint has a visible alpha, it also draws the stroke around the circle.
        /// </summary>
        /// <param name="x">The x-coordinate of the circle's center.</param>
        /// <param name="y">The y-coordinate of the circle's center.</param>
        /// <param name="radius">The radius of the circle.</param>
        public void Circle(float x, float y, float radius)
        {
            _canvas.DrawCircle(x, y, radius, _fillPaint);
            if (_strokePaint.Color.Alpha > 0)
                _canvas.DrawCircle(x, y, radius, _strokePaint);
        }
        /// <summary>
        /// Draws a filled circle at the specified (x, y) coordinates with the given radius.
        /// If stroke paint has a visible alpha, it also draws the stroke around the circle.
        /// </summary>
        /// <param name="x">The x-coordinate of the circle's center.</param>
        /// <param name="y">The y-coordinate of the circle's center.</param>
        /// <param name="radius">The radius of the circle.</param>
        public void circle(float x, float y, float radius) => Circle(x, y, radius);
        /// <summary>
        /// Draws a filled circle at the specified (x, y) coordinates with the given radius.
        /// If stroke paint has a visible alpha, it also draws the stroke around the circle.
        /// </summary>
        /// <param name="x">The x-coordinate of the circle's center.</param>
        /// <param name="y">The y-coordinate of the circle's center.</param>
        /// <param name="radius">The radius of the circle.</param>
        public void Circle(int x, int y, int radius) => Circle((float)x,(float)y,(float)radius);
        /// <summary>
        /// Draws a filled circle at the specified (x, y) coordinates with the given radius.
        /// If stroke paint has a visible alpha, it also draws the stroke around the circle.
        /// </summary>
        /// <param name="x">The x-coordinate of the circle's center.</param>
        /// <param name="y">The y-coordinate of the circle's center.</
        public void circle(int x, int y, int radius) => Circle(x, y, radius);
        #endregion
        #region Ellipse
        /// <summary>
        /// Sets the current ellipse drawing mode which affects how ellipse parameters are interpreted.
        /// </summary>
        /// <param name="mode">The ellipse mode (CENTER, RADIUS, CORNER, or CORNERS).</param>
        public void EllipseMode(EllipseMode mode)
        {
            _ellipseMode = mode;
        }
        /// <summary>
        /// Sets the current ellipse drawing mode which affects how ellipse parameters are interpreted.
        /// </summary>
        /// <param name="mode">The ellipse mode (CENTER, RADIUS, CORNER, or CORNERS).</param>
        public void ellipseMode(EllipseMode mode) => EllipseMode(mode);
        /// <summary>
        /// Draws an ellipse at (x, y) with width w and height h, interpreted according to the current ellipse mode.
        /// </summary>
        /// <param name="x">X coordinate or reference point depending on ellipse mode.</param>
        /// <param name="y">Y coordinate or reference point depending on ellipse mode.</param>
        /// <param name="w">Width or related parameter depending on ellipse mode.</param>
        /// <param name="h">Height or related parameter depending on ellipse mode.</param>
        public void Ellipse(float x, float y, float w, float h)
        {
            float drawX = x, drawY = y, drawW = w, drawH = h;

            switch (_ellipseMode)
            {
                case P5Variables.EllipseMode.CENTER:
                    drawX = x - w / 2;
                    drawY = y - h / 2;
                    break;

                case P5Variables.EllipseMode.RADIUS:
                    drawX = x - w;
                    drawY = y - h;
                    drawW = w * 2;
                    drawH = h * 2;
                    break;

                case P5Variables.EllipseMode.CORNER:
                    // Coordinates already represent top-left corner and size
                    break;

                case P5Variables.EllipseMode.CORNERS:
                    drawW = w - x;
                    drawH = h - y;
                    break;
            }

            // Draw filled ellipse
            _canvas.DrawOval(drawX + drawW / 2, drawY + drawH / 2, drawW / 2, drawH / 2, _fillPaint);

            // Draw stroke ellipse if stroke alpha > 0
            if (_strokePaint.Color.Alpha > 0)
                _canvas.DrawOval(drawX + drawW / 2, drawY + drawH / 2, drawW / 2, drawH / 2, _strokePaint);
        }
        /// <summary>
        /// Draws an ellipse at (x, y) with width w and height h, interpreted according to the current ellipse mode.
        /// </summary>
        /// <param name="x">X coordinate or reference point depending on ellipse mode.</param>
        /// <param name="y">Y coordinate or reference point depending on ellipse mode.</param>
        /// <param name="w">Width or related parameter depending on ellipse mode.</param>
        /// <param name="h">Height or related parameter depending on ellipse mode.</param>
        public void ellipse(float x, float y, float w, float h) => Ellipse(x, y, w, h);
        /// <summary>
        /// Draws an ellipse at (x, y) with width w and height h, interpreted according to the current ellipse mode.
        /// </summary>
        /// <param name="x">X coordinate or reference point depending on ellipse mode.</param>
        /// <param name="y">Y coordinate or reference point depending on ellipse mode.</param>
        /// <param name="w">Width or related parameter depending on ellipse mode.</param>
        /// <param name="h">Height or related parameter depending on ellipse mode.</param>
        public void ellipse(int x, int y, int w, int h) => Ellipse(x, y, w, h);
        /// <summary>
        /// Draws an ellipse at (x, y) with width w and height h, interpreted according to the current ellipse mode.
        /// </summary>
        /// <param name="x">X coordinate or reference point depending on ellipse mode.</param>
        /// <param name="y">Y coordinate or reference point depending on ellipse mode.</param>
        /// <param name="w">Width or related parameter depending on ellipse mode.</param>
        /// <param name="h">Height or related parameter depending on ellipse mode.</param>
        public void Ellipse(int x, int y, int w, int h) => Ellipse((float)x, (float)y, (float)w, (float)h);
        /// <summary>
        /// Draws an ellipse at (x, y) with width w and height h, interpreted according to the current ellipse mode.
        /// </summary>
        /// <param name="x">X coordinate or reference point depending on ellipse mode.</param>
        /// <param name="y">Y coordinate or reference point depending on ellipse mode.</param>
        /// <param name="w">Width or related parameter depending on ellipse mode.</param>
        /// <param name="h">Height or related parameter depending on ellipse mode.</param>
        public void Ellipse(double x, double y, double w, double h) => Ellipse((float)x, (float)y, (float)w, (float)h);
        /// <summary>
        /// Draws an ellipse at (x, y) with width w and height h, interpreted according to the current ellipse mode.
        /// </summary>
        /// <param name="x">X coordinate or reference point depending on ellipse mode.</param>
        /// <param name="y">Y coordinate or reference point depending on ellipse mode.</param>
        /// <param name="w">Width or related parameter depending on ellipse mode.</param>
        /// <param name="h">Height or related parameter depending on ellipse mode.</param>
        public void ellipse(double x, double y, double w, double h) => Ellipse(x, y, w, h);
        #endregion
        #region Line

        /// <summary>
        /// Draws a line between two points (x1, y1) and (x2, y2) using the current stroke paint.
        /// </summary>
        /// <param name="x1">The x-coordinate of the start point.</param>
        /// <param name="y1">The y-coordinate of the start point.</param>
        /// <param name="x2">The x-coordinate of the end point.</param>
        /// <param name="y2">The y-coordinate of the end point.</param>
        public void Line(float x1, float y1, float x2, float y2)
        {
            _canvas.DrawLine(x1, y1, x2, y2, _strokePaint);
        }
        /// <summary>
        /// Draws a line between two points (x1, y1) and (x2, y2) using the current stroke paint.
        /// Alias for <see cref="Line(float, float, float, float)"/>.
        /// </summary>
        /// <param name="x1">The x-coordinate of the start point.</param>
        /// <param name="y1">The y-coordinate of the start point.</param>
        /// <param name="x2">The x-coordinate of the end point.</param>
        /// <param name="y2">The y-coordinate of the end point.</param>
        public void line(float x1, float y1, float x2, float y2) => Line(x1, y1, x2, y2);
        /// <summary>
        /// Draws a line between two points (x1, y1) and (x2, y2) using the current stroke paint.
        /// </summary>
        /// <param name="x1">The x-coordinate of the start point.</param>
        /// <param name="y1">The y-coordinate of the start point.</param>
        /// <param name="x2">The x-coordinate of the end point.</param>
        /// <param name="y2">The y-coordinate of the end point.</param>
        public void Line(int x1, int y1, int x2, int y2)
        {
            _canvas.DrawLine(x1, y1, x2, y2, _strokePaint);
        }
        /// <summary>
        /// Draws a line between two points (x1, y1) and (x2, y2) using the current stroke paint.
        /// Alias for <see cref="Line(int, int, int, int)"/>.
        /// </summary>
        /// <param name="x1">The x-coordinate of the start point.</param>
        /// <param name="y1">The y-coordinate of the start point.</param>
        /// <param name="x2">The x-coordinate of the end point.</param>
        /// <param name="y2">The y-coordinate of the end point.</param>
        public void line(int x1, int y1, int x2, int y2) => Line(x1, y1, x2, y2);
        #endregion
        #region Point
        /// <summary>
        /// Draws a point at the specified (x, y) coordinates using the current stroke paint.
        /// If stroke paint is fully transparent, no point is drawn.
        /// </summary>
        /// <param name="x">The x-coordinate of the point.</param>
        /// <param name="y">The y-coordinate of the point.</param>
        public void Point(float x, float y)
        {
            if (_strokePaint.Color.Alpha == 0) return;
            _canvas.DrawPoint(x, y, _strokePaint);
        }
        /// <summary>
        /// Draws a point at the specified (x, y) coordinates using the current stroke paint.
        /// Alias for <see cref="Point(float, float)"/>.
        /// </summary>
        /// <param name="x">The x-coordinate of the point.</param>
        /// <param name="y">The y-coordinate of the point.</param>
        public void point(float x, float y) => Point(x, y);
        /// <summary>
        /// Draws a point at the specified (x, y) coordinates using the current stroke paint.
        /// If stroke paint is fully transparent, no point is drawn.
        /// </summary>
        /// <param name="x">The x-coordinate of the point.</param>
        /// <param name="y">The y-coordinate of the point.</param>
        public void Point(int x, int y) => Point((float)x, (float)y);
        /// <summary>
        /// Draws a point at the specified (x, y) coordinates using the current stroke paint.
        /// If stroke paint is fully transparent, no point is drawn.
        /// </summary>
        /// <param name="x">The x-coordinate of the point.</param>
        /// <param name="y">The y-coordinate of the point.</param>
        public void point(int x, int y) => Point((float)x, (float)y);
        /// <summary>
        /// Draws a point at the coordinates specified by a <see cref="P5Vector"/> using the current stroke paint.
        /// </summary>
        /// <param name="p">The vector containing the point coordinates.</param>
        public void Point(P5Vector p) => Point(p.x, p.y);
        /// <summary>
        /// Draws a point at the coordinates specified by a <see cref="P5Vector"/> using the current stroke paint.
        /// Alias for <see cref="Point(P5Vector)"/>.
        /// </summary>
        /// <param name="p">The vector containing the point coordinates.</param>
        public void point(P5Vector p) => Point(p);
        #endregion
        #region Quad
        /// <summary>
        /// Draws a quadrilateral defined by four points using the current fill and stroke paints.
        /// Only draws fill if the fill paint's alpha is greater than 0.
        /// Only draws stroke if the stroke paint's alpha and stroke width are greater than 0.
        /// </summary>
        /// <param name="x1">The x-coordinate of the first point.</param>
        /// <param name="y1">The y-coordinate of the first point.</param>
        /// <param name="x2">The x-coordinate of the second point.</param>
        /// <param name="y2">The y-coordinate of the second point.</param>
        /// <param name="x3">The x-coordinate of the third point.</param>
        /// <param name="y3">The y-coordinate of the third point.</param>
        /// <param name="x4">The x-coordinate of the fourth point.</param>
        /// <param name="y4">The y-coordinate of the fourth point.</param>
        public void Quad(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {
            using var path = new SKPath();
            path.MoveTo(x1, y1);
            path.LineTo(x2, y2);
            path.LineTo(x3, y3);
            path.LineTo(x4, y4);
            path.Close();

            if (_fillPaint.Color.Alpha > 0)
            {
                _canvas.DrawPath(path, _fillPaint);
            }

            if (_strokePaint.Color.Alpha > 0 && _strokePaint.StrokeWidth > 0)
            {
                _canvas.DrawPath(path, _strokePaint);
            }
        }
        /// <summary>
        /// Draws a quadrilateral defined by four points using the current fill and stroke paints.
        /// Alias for <see cref="Quad(float, float, float, float, float, float, float, float)"/>.
        /// </summary>
        /// <param name="x1">The x-coordinate of the first point.</param>
        /// <param name="y1">The y-coordinate of the first point.</param>
        /// <param name="x2">The x-coordinate of the second point.</param>
        /// <param name="y2">The y-coordinate of the second point.</param>
        /// <param name="x3">The x-coordinate of the third point.</param>
        /// <param name="y3">The y-coordinate of the third point.</param>
        /// <param name="x4">The x-coordinate of the fourth point.</param>
        /// <param name="y4">The y-coordinate of the fourth point.</param>
        public void quad(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4) => Quad(x1, y1, x2, y2, x3, y3, x4, y4);
        /// <summary>
        /// Draws a quadrilateral defined by four points using the current fill and stroke paints.
        /// Alias for <see cref="Quad(float, float, float, float, float, float, float, float)"/>.
        /// </summary>
        /// <param name="x1">The x-coordinate of the first point.</param>
        /// <param name="y1">The y-coordinate of the first point.</param>
        /// <param name="x2">The x-coordinate of the second point.</param>
        /// <param name="y2">The y-coordinate of the second point.</param>
        /// <param name="x3">The x-coordinate of the third point.</param>
        /// <param name="y3">The y-coordinate of the third point.</param>
        /// <param name="x4">The x-coordinate of the fourth point.</param>
        /// <param name="y4">The y-coordinate of the fourth point.</param>
        public void Quad(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4) => Quad((float)x1, (float)y1, (float)x2, (float)y2, (float)x3, (float)y3, (float)x4, (float)y4);
        /// <summary>
        /// Draws a quadrilateral defined by four points using the current fill and stroke paints.
        /// Alias for <see cref="Quad(float, float, float, float, float, float, float, float)"/>.
        /// </summary>
        /// <param name="x1">The x-coordinate of the first point.</param>
        /// <param name="y1">The y-coordinate of the first point.</param>
        /// <param name="x2">The x-coordinate of the second point.</param>
        /// <param name="y2">The y-coordinate of the second point.</param>
        /// <param name="x3">The x-coordinate of the third point.</param>
        /// <param name="y3">The y-coordinate of the third point.</param>
        /// <param name="x4">The x-coordinate of the fourth point.</param>
        /// <param name="y4">The y-coordinate of the fourth point.</param>
        public void quad(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4) => Quad((float)x1, (float)y1, (float)x2, (float)y2, (float)x3, (float)y3, (float)x4, (float)y4);
        #endregion
        #region Rect
        /// <summary>
        /// Sets the current rectangle drawing mode.
        /// </summary>
        /// <param name="mode">The rectangle mode to use (CORNER, CORNERS, CENTER, RADIUS).</param>
        public void RectMode(Rectmode mode) => _rectMode = mode;
        /// <summary>
        /// Sets the current rectangle drawing mode.
        /// Alias for <see cref="RectMode"/>.
        /// </summary>
        /// <param name="mode">The rectangle mode to use (CORNER, CORNERS, CENTER, RADIUS).</param>
        public void rectMode(Rectmode mode) => RectMode(mode);
        /// <summary>
        /// Draws a rectangle at position (x, y) with width w and height h according to the current rectangle mode.
        /// Fills and strokes the rectangle.
        /// </summary>
        /// <param name="x">The x-coordinate of the rectangle position.</param>
        /// <param name="y">The y-coordinate of the rectangle position.</param>
        /// <param name="w">The width of the rectangle (interpretation depends on mode).</param>
        /// <param name="h">The height of the rectangle (interpretation depends on mode).</param>
        public void Rect(float x, float y, float w, float h)
        {
            SKRect rect;

            switch (_rectMode)
            {
                case P5Variables.Rectmode.CORNERS:
                    rect = new SKRect(x, y, w, h);  // x/y are top-left, w/h are bottom-right
                    break;

                case P5Variables.Rectmode.CENTER:
                    rect = new SKRect(x - w / 2, y - h / 2, x + w / 2, y + h / 2);
                    break;

                case P5Variables.Rectmode.RADIUS:
                    rect = new SKRect(x - w, y - h, x + w, y + h);
                    break;

                case P5Variables.Rectmode.CORNER:
                default:
                    rect = new SKRect(x, y, x + w, y + h);
                    break;
            }

            _canvas.DrawRect(rect, _fillPaint);
            _canvas.DrawRect(rect, _strokePaint);
        }
        /// <summary>
        /// Draws a rectangle at position (x, y) with width w and height h according to the current rectangle mode.
        /// Alias for <see cref="Rect(float, float, float, float)"/>.
        /// </summary>
        /// <param name="x">The x-coordinate of the rectangle position.</param>
        /// <param name="y">The y-coordinate of the rectangle position.</param>
        /// <param name="w">The width of the rectangle.</param>
        /// <param name="h">The height of the rectangle.</param>
        public void rect(float x, float y, float w, float h) => Rect(x, y, w, h);
        /// <summary>
        /// Draws a rectangle with integer parameters.
        /// Alias for <see cref="Rect(float, float, float, float)"/> after conversion.
        /// </summary>
        /// <param name="x">The x-coordinate of the rectangle position.</param>
        /// <param name="y">The y-coordinate of the rectangle position.</param>
        /// <param name="w">The width of the rectangle.</param>
        /// <param name="h">The height of the rectangle.</param>
        public void Rect(int x, int y, int w, int h) => Rect((float)x, (float)y, (float)w, (float)h);  
        /// <summary>
        /// Draws a rectangle with integer parameters.
        /// Alias for <see cref="Rect(float, float, float, float)"/> after conversion.
        /// </summary>
        /// <param name="x">The x-coordinate of the rectangle position.</param>
        /// <param name="y">The y-coordinate of the rectangle position.</param>
        /// <param name="w">The width of the rectangle.</param>
        /// <param name="h">The height of the rectangle.</param>
        public void rect(int x, int y, int w, int h) => Rect(x, y, w, h);
        /// <summary>
        /// Draws a rounded rectangle at position (x, y) with width w, height h, and corner radius.
        /// Fills and strokes the rounded rectangle.
        /// </summary>
        /// <param name="x">The x-coordinate of the rectangle position.</param>
        /// <param name="y">The y-coordinate of the rectangle position.</param>
        /// <param name="w">The width of the rectangle.</param>
        /// <param name="h">The height of the rectangle.</param>
        /// <param name="radius">The radius of the corners.</param>
        public void Rect(float x, float y, float w, float h, float radius)
        {
            SKRect rect;

            switch (_rectMode)
            {
                case P5Variables.Rectmode.CORNERS:
                    rect = new SKRect(x, y, w, h);  // x/y are top-left, w/h are bottom-right
                    break;

                case P5Variables.Rectmode.CENTER:
                    rect = new SKRect(x - w / 2, y - h / 2, x + w / 2, y + h / 2);
                    break;

                case P5Variables.Rectmode.RADIUS:
                    rect = new SKRect(x - w, y - h, x + w, y + h);
                    break;

                case P5Variables.Rectmode.CORNER:
                default:
                    rect = new SKRect(x, y, x + w, y + h);
                    break;
            }
            _canvas.DrawRoundRect(rect, radius, radius, _fillPaint);
            _canvas.DrawRoundRect(rect, radius, radius, _strokePaint);
        }
        /// <summary>
        /// Draws a rounded rectangle at position (x, y) with width w, height h, and corner radius.
        /// Alias for <see cref="Rect(float, float, float, float, float)"/>.
        /// </summary>
        /// <param name="x">The x-coordinate of the rectangle position.</param>
        /// <param name="y">The y-coordinate of the rectangle position.</param>
        /// <param name="w">The width of the rectangle.</param>
        /// <param name="h">The height of the rectangle.</param>
        /// <param name="radius">The radius of the corners.</param>
        public void rect(float x, float y, float w, float h, float radius) => Rect(x, y, w, h, radius);
        /// <summary>
        /// Draws a rectangle at position (x, y) with width w and height h, with individually specified corner radii.
        /// The corners are drawn in the order: top-left, top-right, bottom-right, bottom-left.
        /// Fills and strokes the path.
        /// </summary>
        /// <param name="x">The x-coordinate of the rectangle position.</param>
        /// <param name="y">The y-coordinate of the rectangle position.</param>
        /// <param name="w">The width of the rectangle.</param>
        /// <param name="h">The height of the rectangle.</param>
        /// <param name="tl">Radius of the top-left corner.</param>
        /// <param name="tr">Radius of the top-right corner.</param>
        /// <param name="br">Radius of the bottom-right corner.</param>
        /// <param name="bl">Radius of the bottom-left corner.</param>
        public void Rect(float x, float y, float w, float h, float tl, float tr, float br, float bl)
        {
            var path = new SKPath();

            path.MoveTo(x + tl, y);
            path.LineTo(x + w - tr, y);
            path.QuadTo(x + w, y, x + w, y + tr);

            path.LineTo(x + w, y + h - br);
            path.QuadTo(x + w, y + h, x + w - br, y + h);

            path.LineTo(x + bl, y + h);
            path.QuadTo(x, y + h, x, y + h - bl);

            path.LineTo(x, y + tl);
            path.QuadTo(x, y, x + tl, y);
            path.Close();

            _canvas.DrawPath(path, _fillPaint);
            _canvas.DrawPath(path, _strokePaint);
        }
        /// <summary>
        /// Draws a rectangle at position (x, y) with width w and height h, with individually specified corner radii.
        /// Alias for <see cref="Rect(float, float, float, float, float, float, float, float)"/>.
        /// </summary>
        /// <param name="x">The x-coordinate of the rectangle position.</param>
        /// <param name="y">The y-coordinate of the rectangle position.</param>
        /// <param name="w">The width of the rectangle.</param>
        /// <param name="h">The height of the rectangle.</param>
        /// <param name="tl">Radius of the top-left corner.</param>
        /// <param name="tr">Radius of the top-right corner.</param>
        /// <param name="br">Radius of the bottom-right corner.</param>
        /// <param name="bl">Radius of the bottom-left corner.</param>
        public void rect(float x, float y, float w, float h, float tl, float tr, float br, float bl) => Rect(x, y, w, h, tl, tr, br, bl);
        #endregion
        #region Square
        /// <summary>
        /// Draws a square at position (x, y) with side length s.
        /// Uses the current rectangle mode.
        /// </summary>
        /// <param name="x">The x-coordinate of the square position.</param>
        /// <param name="y">The y-coordinate of the square position.</param>
        /// <param name="s">The side length of the square.</param>
        public void Square(float x, float y, float s) => Rect(x, y, s, s);
        /// <summary>
        /// Draws a square at position (x, y) with side length s.
        /// Uses the current rectangle mode.
        /// </summary>
        /// <param name="x">The x-coordinate of the square position.</param>
        /// <param name="y">The y-coordinate of the square position.</param>
        /// <param name="s">The side length of the square.</param>
        public void Square(int x, int y, int s) => Square((float)x, (float)y, (float)s);
        /// <summary>
        /// Draws a square at position (x, y) with side length s.
        /// Alias for <see cref="Square(float, float, float)"/>.
        /// </summary>
        /// <param name="x">The x-coordinate of the square position.</param>
        /// <param name="y">The y-coordinate of the square position.</param>
        /// <param name="s">The side length of the square.</param>
        public void square(float x, float y, float s) => Rect(x, y, s, s);
        /// <summary>
        /// Draws a square at position (x, y) with side length s.
        /// Alias for <see cref="Square(int, int, int)"/>.
        /// </summary>
        /// <param name="x">The x-coordinate of the square position.</param>
        /// <param name="y">The y-coordinate of the square position.</param>
        /// <param name="s">The side length of the square.</param>
        public void square(int x, int y, int s) => square((float)x, (float)y, (float)s);
        /// <summary>
        /// Draws a rounded square at position (x, y) with side length s and corner radius.
        /// </summary>
        /// <param name="x">The x-coordinate of the square position.</param>
        /// <param name="y">The y-coordinate of the square position.</param>
        /// <param name="s">The side length of the square.</param>
        /// <param name="radius">The corner radius.</param>
        public void Square(float x, float y, float s, float radius) => Rect(x, y, s, s, radius);
        /// <summary>
        /// Draws a rounded square at position (x, y) with side length s and corner radius.
        /// </summary>
        /// <param name="x">The x-coordinate of the square position.</param>
        /// <param name="y">The y-coordinate of the square position.</param>
        /// <param name="s">The side length of the square.</param>
        /// <param name="radius">The corner radius.</param>
        public void Square(int x, int y, int s, int radius) => Square((float)x, (float)y, (float)s, (float)radius);
        /// <summary>
        /// Draws a rounded square at position (x, y) with side length s and corner radius.
        /// Alias for <see cref="Square(float, float, float, float)"/>.
        /// </summary>
        /// <param name="x">The x-coordinate of the square position.</param>
        /// <param name="y">The y-coordinate of the square position.</param>
        /// <param name="s">The side length of the square.</param>
        /// <param name="radius">The corner radius.</param>
        public void square(float x, float y, float s, float radius) => Rect(x, y, s, s, radius);
        /// <summary>
        /// Draws a rounded square at position (x, y) with side length s and corner radius.
        /// Alias for <see cref="Square(float, float, float, float)"/>.
        /// </summary>
        /// <param name="x">The x-coordinate of the square position.</param>
        /// <param name="y">The y-coordinate of the square position.</param>
        /// <param name="s">The side length of the square.</param>
        /// <param name="radius">The corner radius.</param>
        public void square(int x, int y, int s, int radius) => square((float)x, (float)y, (float)s, (float)radius);
        /// <summary>
        /// Draws a square at position (x, y) with side length s and individually specified corner radii.
        /// </summary>
        /// <param name="x">The x-coordinate of the square position.</param>
        /// <param name="y">The y-coordinate of the square position.</param>
        /// <param name="s">The side length of the square.</param>
        /// <param name="tl">Top-left corner radius.</param>
        /// <param name="tr">Top-right corner radius.</param>
        /// <param name="br">Bottom-right corner radius.</param>
        /// <param name="bl">Bottom-left corner radius.</param>
        public void Square(float x, float y, float s, float tl, float tr, float br, float bl) => Rect(x, y, s, s, tl, tr, br, bl);
        /// <summary>
        /// Draws a square at position (x, y) with side length s and individually specified corner radii.
        /// </summary>
        /// <param name="x">The x-coordinate of the square position.</param>
        /// <param name="y">The y-coordinate of the square position.</param>
        /// <param name="s">The side length of the square.</param>
        /// <param name="tl">Top-left corner radius.</param>
        /// <param name="tr">Top-right corner radius.</param>
        /// <param name="br">Bottom-right corner radius.</param>
        /// <param name="bl">Bottom-left corner radius.</param>
        public void Square(int x, int y, int s, int tl, int tr, int br, int bl) => Square((float)x, (float)y, (float)s, (float)tl, (float)tr, (float)br, (float)bl);
        /// <summary>
        /// Draws a square at position (x, y) with side length s and individually specified corner radii.
        /// Alias for <see cref="Square(float, float, float, float, float, float, float)"/>.
        /// </summary>
        /// <param name="x">The x-coordinate of the square position.</param>
        /// <param name="y">The y-coordinate of the square position.</param>
        /// <param name="s">The side length of the square.</param>
        /// <param name="tl">Top-left corner radius.</param>
        /// <param name="tr">Top-right corner radius.</param>
        /// <param name="br">Bottom-right corner radius.</param>
        /// <param name="bl">Bottom-left corner radius.</param>
        public void square(float x, float y, float s, float tl, float tr, float br, float bl) => Rect(x, y, s, s, tl, tr, br, bl);
        /// <summary>
        /// Draws a square at position (x, y) with side length s and individually specified corner radii.
        /// Alias for <see cref="Square(float, float, float, float, float, float, float)"/>.
        /// </summary>
        /// <param name="x">The x-coordinate of the square position.</param>
        /// <param name="y">The y-coordinate of the square position.</param>
        /// <param name="s">The side length of the square.</param>
        /// <param name="tl">Top-left corner radius.</param>
        /// <param name="tr">Top-right corner radius.</param>
        /// <param name="br">Bottom-right corner radius.</param>
        /// <param name="bl">Bottom-left corner radius.</param>
        public void square(int x, int y, int s, int tl, int tr, int br, int bl) => square((float)x, (float)y, (float)s, (float)tl, (float)tr, (float)br, (float)bl);
        #endregion
        #region Triangle
        /// <summary>
        /// Draws a triangle connecting the three points (x1, y1), (x2, y2), and (x3, y3).
        /// Fills the triangle with fill paint and optionally draws the stroke if enabled.
        /// </summary>
        /// <param name="x1">The x-coordinate of the first vertex.</param>
        /// <param name="y1">The y-coordinate of the first vertex.</param>
        /// <param name="x2">The x-coordinate of the second vertex.</param>
        /// <param name="y2">The y-coordinate of the second vertex.</param>
        /// <param name="x3">The x-coordinate of the third vertex.</param>
        /// <param name="y3">The y-coordinate of the third vertex.</param>
        public void Triangle(float x1, float y1, float x2, float y2, float x3, float y3)
        {
            using var path = new SKPath();
            path.MoveTo(x1, y1);
            path.LineTo(x2, y2);
            path.LineTo(x3, y3);
            path.Close();

            if (_fillPaint.Color.Alpha > 0)
            {
                _canvas.DrawPath(path, _fillPaint);
            }

            if (_strokePaint.Color.Alpha > 0 && _strokePaint.StrokeWidth > 0)
            {
                _canvas.DrawPath(path, _strokePaint);
            }
        }
        /// <summary>
        /// Draws a triangle connecting the three points (x1, y1), (x2, y2), and (x3, y3).
        /// Fills the triangle with fill paint and optionally draws the stroke if enabled.
        /// </summary>
        /// <param name="x1">The x-coordinate of the first vertex.</param>
        /// <param name="y1">The y-coordinate of the first vertex.</param>
        /// <param name="x2">The x-coordinate of the second vertex.</param>
        /// <param name="y2">The y-coordinate of the second vertex.</param>
        /// <param name="x3">The x-coordinate of the third vertex.</param>
        /// <param name="y3">The y-coordinate of the third vertex.</param>
        public void triangle(float x1, float y1, float x2, float y2, float x3, float y3) => Triangle(x1, y1, x2, y2, x3, y3);
        /// <summary>
        /// Draws a triangle connecting the three points with integer coordinates.
        /// Calls the float version after converting arguments.
        /// </summary>
        public void Triangle(int x1, int y1, int x2, int y2, int x3, int y3) => Triangle((float)x1, (float)y1, (float)x2, (float)y2, (float)x3, (float)y3);
        /// <summary>
        /// Draws a triangle connecting the three points with integer coordinates.
        /// Calls the float version after converting arguments.
        /// </summary>
        public void triangle(int x1, int y1, int x2, int y2, int x3, int y3) => Triangle(x1, y1, x2, y2, x3, y3);
        #endregion
        #region DrawShape
        /// <summary>
        /// Sets the point drawing mode used when drawing points with DrawPoints.
        /// </summary>
        /// <param name="pointmode">The SKPointMode to use.</param>
        public void PointMode(SKPointMode pointmode)
        {
            _pointMode = pointmode;
        }
        /// <summary>
        /// Alias for <see cref="PointMode(SKPointMode)"/>.
        /// </summary>
        public void pointMode(SKPointMode pointmode) => PointMode(pointmode);
        /// <summary>
        /// Begins recording vertices for a shape with the specified mode.
        /// Clears previous vertices and marks the shape as open.
        /// If mode is POINTS, disables filling.
        /// </summary>
        /// <param name="mode">The shape drawing mode, default is POLYGON.</param>
        public void BeginShape(ShapeMode mode = ShapeMode.POLYGON)
        {
            _currentShapeMode = mode;
            _shapeVertices.Clear();
            _isShapeOpen = true;
            _isShapeClosed = false;

            if (mode == ShapeMode.POINTS)
                NoFill();
        }
        /// <summary>
        /// Begins recording vertices for a shape with the specified mode.
        /// Clears previous vertices and marks the shape as open.
        /// If mode is POINTS, disables filling.
        /// </summary>
        /// <param name="mode">The shape drawing mode, default is POLYGON.</param>
        public void beginShape(ShapeMode mode = ShapeMode.POLYGON) => BeginShape(mode);
        /// <summary>
        /// Adds a vertex with integer coordinates to the current shape if the shape is open.
        /// Throws InvalidOperationException if called without an open shape.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        public void Vertex(int x, int y)
        {
            if (_isShapeOpen)
            {
                _shapeVertices.Add(new SKPoint(x, y));
            }
            else
            {
                throw new InvalidOperationException("beginShape() must be called before vertex().");
            }
        }
        /// <summary>
        /// Adds a vertex with integer coordinates to the current shape if the shape is open.
        /// Throws InvalidOperationException if called without an open shape.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        public void vertex(int x, int y) => Vertex(x, y);
        /// <summary>
        /// Adds a vertex with float coordinates to the current shape if the shape is open.
        /// Throws InvalidOperationException if called without an open shape.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        public void Vertex(float x, float y)
        {
            if (_isShapeOpen)
            {
                _shapeVertices.Add(new SKPoint(x, y));
            }
            else
            {
                throw new InvalidOperationException("beginShape() must be called before vertex().");
            }
        }
        /// <summary>
        /// Adds a vertex with float coordinates to the current shape if the shape is open.
        /// Throws InvalidOperationException if called without an open shape.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        public void vertex(float x, float y) => Vertex(x, y);
        /// <summary>
        /// Ends the current shape and draws it according to the current shape mode.
        /// Optionally closes the shape if mode is EndShapeMode.CLOSE.
        /// Clears vertices and marks the shape as closed.
        /// </summary>
        /// <param name="mode">The end shape mode, default is OPEN.</param>
        public void EndShape(EndShapeMode mode = EndShapeMode.OPEN)
        {
            if (!_isShapeOpen || _shapeVertices.Count < 1)
                return;

            switch (_currentShapeMode)
            {
                case ShapeMode.POINTS:
                    _canvas.DrawPoints(SKPointMode.Points, _shapeVertices.ToArray(), _strokePaint);
                    break;

                case ShapeMode.LINES:
                    _canvas.DrawPoints(SKPointMode.Lines, _shapeVertices.ToArray(), _strokePaint);
                    break;

                case ShapeMode.TRIANGLES:
                    for (int i = 0; i + 2 < _shapeVertices.Count; i += 3)
                        DrawTriangle(_shapeVertices[i], _shapeVertices[i + 1], _shapeVertices[i + 2]);
                    break;

                case ShapeMode.TRIANGLE_STRIP:
                    for (int i = 0; i + 2 < _shapeVertices.Count; i++)
                        DrawTriangle(_shapeVertices[i], _shapeVertices[i + 1], _shapeVertices[i + 2]);
                    break;

                case ShapeMode.TRIANGLE_FAN:
                    if (_shapeVertices.Count >= 3)
                    {
                        var center = _shapeVertices[0];
                        for (int i = 1; i + 1 < _shapeVertices.Count; i++)
                            DrawTriangle(center, _shapeVertices[i], _shapeVertices[i + 1]);
                    }
                    break;

                case ShapeMode.QUADS:
                    for (int i = 0; i + 3 < _shapeVertices.Count; i += 4)
                        DrawQuad(_shapeVertices[i], _shapeVertices[i + 1], _shapeVertices[i + 2], _shapeVertices[i + 3]);
                    break;

                case ShapeMode.QUAD_STRIP:
                    for (int i = 0; i + 3 < _shapeVertices.Count; i += 2)
                        DrawQuad(_shapeVertices[i], _shapeVertices[i + 1], _shapeVertices[i + 3], _shapeVertices[i + 2]);
                    break;

                case ShapeMode.POLYGON:
                    using (var path = new SKPath())
                    {
                        path.MoveTo(_shapeVertices[0]);
                        for (int i = 1; i < _shapeVertices.Count; i++)
                            path.LineTo(_shapeVertices[i]);

                        if (mode == EndShapeMode.CLOSE)
                            path.Close();

                        DrawPath(path, _fillPaint);
                        DrawPath(path, _strokePaint);
                    }
                    break;
            }

            _shapeVertices.Clear();
            _isShapeOpen = false;
        }
        /// <summary>
        /// Ends the current shape and draws it according to the current shape mode.
        /// Optionally closes the shape if mode is EndShapeMode.CLOSE.
        /// Clears vertices and marks the shape as closed.
        /// </summary>
        /// <param name="mode">The end shape mode, default is OPEN.</param>
        public void endShape(EndShapeMode mode = EndShapeMode.OPEN) => EndShape(mode);
        /// <summary>
        /// Draws a filled and stroked triangle using the three points.
        /// </summary>
        /// <param name="a">First point.</param>
        /// <param name="b">Second point.</param>
        /// <param name="c">Third point.</param>
        private void DrawTriangle(SKPoint a, SKPoint b, SKPoint c)
        {
            using var path = new SKPath();
            path.MoveTo(a);
            path.LineTo(b);
            path.LineTo(c);
            path.Close();

            _canvas.DrawPath(path, _fillPaint);
            _canvas.DrawPath(path, _strokePaint);
        }
        /// <summary>
        /// Draws a filled and stroked quadrilateral using the four points.
        /// </summary>
        /// <param name="a">First point.</param>
        /// <param name="b">Second point.</param>
        /// <param name="c">Third point.</param>
        /// <param name="d">Fourth point.</param>
        private void DrawQuad(SKPoint a, SKPoint b, SKPoint c, SKPoint d)
        {
            using var path = new SKPath();
            path.MoveTo(a);
            path.LineTo(b);
            path.LineTo(c);
            path.LineTo(d);
            path.Close();

            _canvas.DrawPath(path, _fillPaint);
            _canvas.DrawPath(path, _strokePaint);
        }
        /// <summary>
        /// Draws the given SKPath with the specified paint.
        /// </summary>
        /// <param name="path">The SKPath to draw.</param>
        /// <param name="paint">The paint to use.</param>
        public void DrawPath(SKPath path, SKPaint paint)
        {
            _canvas.DrawPath(path, paint);
        }
        /// <summary>
        /// Draws the given SKPath with the specified paint.
        /// </summary>
        /// <param name="path">The SKPath to draw.</param>
        /// <param name="paint">The paint to use.</param>
        public void drawPath(SKPath path, SKPaint paint) => DrawPath(path, paint);
        /// <summary>
        /// Draws points from the given SKPath using the current point mode and specified paint.
        /// </summary>
        /// <param name="path">The SKPath containing points.</param>
        /// <param name="paint">The paint to use.</param>
        public void DrawPoints(SKPath path, SKPaint paint)
        {
            _canvas.DrawPoints(_pointMode, path.Points, paint);
        }
        /// <summary>
        /// Draws points from the given SKPath using the current point mode and specified paint.
        /// </summary>
        /// <param name="path">The SKPath containing points.</param>
        /// <param name="paint">The paint to use.</param>
        public void drawPoints(SKPath path, SKPaint paint) => DrawPoints(path, paint);
        #endregion
    }
}
