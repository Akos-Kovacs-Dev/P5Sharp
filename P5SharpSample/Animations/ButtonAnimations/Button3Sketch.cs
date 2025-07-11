

using P5Sharp;
using SkiaSharp;
using System.Collections.Generic;

namespace P5SharpSample.Animations.ButtonAnimations
{


    public partial class Button3Sketch : SketchBase
    {





        float buttonWith;
        float buttonHeight;

        List<float> bounceScaleCurve = new List<float>()
{
    1.0f, 0.92f, 0.87f, 0.9f, 0.95f, 1.02f, 0.98f, 1.0f
};
        bool animate = false;

        protected override void Setup()
        {
            buttonWith = Width - 10;
            buttonHeight = Height - 10;

            OnMousePress = new Action<float, float>((x, y) =>
            {
                //buttonWith = 100;
                animate = true;
            });

        }

        float angle = 0f;
        float radius = 250f; 
        float radiusspeed = 0.5f;

        int i = 0;
        protected override void Draw()
        {
            Background(0,0);
            angle += 5f;
            radius += radiusspeed;

            if (radius > 300)
            {
                radiusspeed = radiusspeed * -1;
            }
            else if (radius < 240)
            {
                radiusspeed = radiusspeed * -1;
            }

            if (angle > 360)
                angle = 0;
            RectMode(Rectmode.CENTER);
            strokeWeight(5);
            StrokeGradient(new[] { SKColors.Red, SKColors.Blue }, angleDegrees: angle);
            FillGradient(new[] { SKColors.Red, SKColors.Blue }, angleDegrees: angle * -1, gradientType: GradientType.Radial, radialRadius: radius, customCenter: (0, 0));




            if (animate == true)
            {
                if (i < bounceScaleCurve.Count - 1)
                {
                    i++;
                    rect(Width / 2, Height / 2, (Width - 10) * bounceScaleCurve[i], (Height - 10) * bounceScaleCurve[i], 5);

                }
                else
                {

                    i = 0;
                    animate = false;


                    rect(Width / 2, Height / 2, (Width - 10) * bounceScaleCurve[i], (Height - 10) * bounceScaleCurve[i], 5);

                }

            }
            else
            {
                rect(Width / 2, Height / 2, Width - 10, Height - 10, 5);
            } 

            fill("white");
            textSize(50* bounceScaleCurve[i]);             
            Text("Button");

        }
    }


}
