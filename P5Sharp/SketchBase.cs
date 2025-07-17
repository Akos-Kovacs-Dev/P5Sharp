using SkiaSharp;

namespace P5Sharp
{
    public abstract class SketchBase : P5Shapes
    {


        public Dictionary<string, Action<string>> SketchActions { get; set; } = new Dictionary<string, Action<string>>();


        public SketchBase()
        {


        }

        #region Setup and Draw

        /// <summary>
        /// Initializes the canvas and image info, and calls the user-defined Setup method.
        /// </summary>
        public void OnSetup(SKCanvas canvas, SKImageInfo info)
        {
            _info = info;
            _canvas = canvas;
            Setup();
        }

        /// <summary>
        /// Applies state from P5Variables and invokes the Draw method.
        /// </summary>
        public void OnDraw(P5Variables p5Variables)
        {
            _canvas = p5Variables._canvas;
            _info = p5Variables._info;
            _fillPaint = p5Variables._fillPaint;
            _strokePaint = p5Variables._strokePaint;
            _pointMode = p5Variables._pointMode;
            // _angleMode = p5Variables._angleMode; // Keep commented 
            _rectMode = p5Variables._rectMode;
            _isSmooth = p5Variables._isSmooth;
            _textAlignment = p5Variables._textAlignment;
            _font = p5Variables._font;
            _ellipseMode = p5Variables._ellipseMode;
            _currentShapeMode = p5Variables._currentShapeMode;
            _shapeVertices = p5Variables._shapeVertices;
            _isShapeOpen = p5Variables._isShapeOpen;
            _isShapeClosed = p5Variables._isShapeClosed;

            MouseX = p5Variables.MouseX;
            MouseY = p5Variables.MouseY;
            MousePressed = p5Variables.MousePressed;
            TouchPressed = p5Variables.TouchPressed;
            TouchX = p5Variables.TouchX;
            TouchY = p5Variables.TouchY;

            Draw();
        }

        /// <summary>
        /// User-defined setup logic called once before drawing starts.
        /// </summary>
        protected abstract void Setup();

        /// <summary>
        /// User-defined drawing logic called each frame.
        /// </summary>
        protected abstract void Draw();

        #endregion

        public Action<float, float> OnMousePress { get; set; }

        public virtual void OnMouseMove(float x, float y)
        {
            MouseX = x;
            MouseY = y;
        }

        public virtual void OnMouseDown(float x, float y)
        {
            MousePressed = true;
            MouseX = x;
            MouseY = y;
        }

        public virtual void OnMouseUp(float x, float y)
        {
            MousePressed = false;
            MouseX = x;
            MouseY = y;
        }

        public virtual void OnTouchStart(float x, float y)
        {
            if (TouchPressed == false)
            {
                OnMousePress?.Invoke(x, y);
            }
            TouchPressed = true;
            TouchX = x;
            TouchY = y;
        }

        public virtual void OnTouchMove(float x, float y)
        {
            TouchX = x;
            TouchY = y;
        }

        public virtual void OnTouchEnd(float x, float y)
        {
            TouchPressed = false;
            TouchX = x;
            TouchY = y;
        }

        #region Background

        /// <summary>Sets the background using a named color.</summary>
        public void Background(string colorName)
        {
            if (NamedColors.TryGetValue(colorName, out var color))
            {
                _canvas.Clear(color);
            }
            else
            {
                throw new ArgumentException($"Unknown color name: {colorName}");
            }
        }

        /// <summary>Sets a grayscale background (0–255).</summary>
        public void Background(float c) => _canvas.Clear(new SKColor((byte)c, (byte)c, (byte)c));

        /// <summary>Sets a grayscale background with opacity.</summary>
        public void Background(float c, float opacity) =>
            _canvas.Clear(new SKColor((byte)c, (byte)c, (byte)c, (byte)opacity));

        /// <summary>Sets an RGB background (0–255).</summary>
        public void Background(float r, float g, float b) =>
            _canvas.Clear(new SKColor((byte)r, (byte)g, (byte)b));

        /// <summary>Sets an RGBA background (0–255).</summary>
        public void Background(float r, float g, float b, float a) =>
            _canvas.Clear(new SKColor((byte)r, (byte)g, (byte)b, (byte)a));

        /// <summary>Sets a grayscale background using int (0–255).</summary>
        public void Background(int c) => _canvas.Clear(new SKColor((byte)c, (byte)c, (byte)c));

        /// <summary>Sets a grayscale background with opacity using int.</summary>
        public void Background(int c, int opacity) =>
            _canvas.Clear(new SKColor((byte)c, (byte)c, (byte)c, (byte)opacity));

        /// <summary>Sets an RGB background using int values.</summary>
        public void Background(int r, int g, int b) =>
            _canvas.Clear(new SKColor((byte)r, (byte)g, (byte)b));

        /// <summary>Sets an RGBA background using int values.</summary>
        public void Background(int r, int g, int b, int a) =>
            _canvas.Clear(new SKColor((byte)r, (byte)g, (byte)b, (byte)a));

        /// <summary>Sets a grayscale background using byte.</summary>
        public void Background(byte c) => _canvas.Clear(new SKColor(c, c, c));

        /// <summary>Sets a grayscale background with opacity using byte.</summary>
        public void Background(byte c, byte opacity) => _canvas.Clear(new SKColor(c, c, c, opacity));

        /// <summary>Sets an RGB background using byte values.</summary>
        public void Background(byte r, byte g, byte b) => _canvas.Clear(new SKColor(r, g, b));

        /// <summary>Sets an RGBA background using byte values.</summary>
        public void Background(byte r, byte g, byte b, byte a) => _canvas.Clear(new SKColor(r, g, b, a));

        /// <summary>Sets the background using a specified <see cref="SKColor"/> value.</summary>
        public void Background(SKColor color) => _canvas.Clear(color);
        /// <summary>Clear canvas.</summary>
        public void Clear() => _canvas.Clear();
        /// <summary>Clear canvas.</summary>
        public void clear() => Clear();

        #endregion

        #region Transformations

        /// <summary>Rotates the canvas by an angle in radians.</summary>
        public void rotate(float angleRadians) => _canvas?.RotateRadians(angleRadians);

        /// <summary>Rotates the canvas by an angle in degrees.</summary>
        public void rotateDegrees(float angleDegrees) => _canvas?.RotateDegrees(angleDegrees);

        /// <summary>Translates the canvas by the specified amount.</summary>
        public void translate(float dx, float dy) => _canvas?.Translate(dx, dy);

        /// <summary>Scales the canvas uniformly by the given factor.</summary>
        public void scale(float s) => _canvas?.Scale(s);

        /// <summary>Scales the canvas non-uniformly in X and Y directions.</summary>
        public void scale(float sx, float sy) => _canvas?.Scale(sx, sy);

        /// <summary>Pushes the current transformation state to the stack.</summary>
        public void push() => _canvas?.Save();

        /// <summary>Pops the last saved transformation state from the stack.</summary>
        public void pop() => _canvas?.Restore();

        #endregion

    }
}
