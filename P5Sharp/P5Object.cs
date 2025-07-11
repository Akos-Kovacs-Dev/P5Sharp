using SkiaSharp;

namespace P5Sharp
{
    /// <summary>
    /// Base class for P5Sharp sketches. Inherit this to define setup and draw behavior.
    /// </summary>
    public abstract class P5Object : P5Shapes
    {
        #region Setup and Draw

       

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
        /// User-defined drawing logic called each frame.
        /// </summary>
        protected abstract void Draw();


      
        #endregion


    }
}
