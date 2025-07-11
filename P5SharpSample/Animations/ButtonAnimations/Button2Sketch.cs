

using P5Sharp;
using SkiaSharp;

namespace P5SharpSample.Animations.ButtonAnimations
{


    public partial class Button2Sketch : SketchBase
    {




        float x = -100;
        float y = -100;
        float r = 0;

        protected override void Setup()
        {
            

            OnMousePress = new Action<float, float>((x, y) =>
            {
                this.x = x;
                this.y = y;


            });

        }


        
        protected override void Draw()
        {

            Background(0, 0);
             
            if (MousePressed)
            {
                if(r < Width)
                r += 15;
            }
            else
            {
                if(r > 0)
                r -= 15;
                 
            }

                fill(251, 84, 49);
            rect(0, 0, Width, Height,5);


            NoStroke();
            fill(255,255,255,50);
            circle(MouseX, MouseY, r);


            fill("white");
            textSize(25);
            Text("BUTTON");

        }
    }


}
