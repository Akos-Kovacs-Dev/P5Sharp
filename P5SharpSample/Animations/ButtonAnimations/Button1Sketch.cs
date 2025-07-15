

using P5Sharp;
using SkiaSharp;

namespace P5SharpSample.Animations.ButtonAnimations
{


    public partial class Button1Sketch : SketchBase
    {



        bool animate = false;
        protected override void Setup()
        {
            NoStroke();             
            OnMousePress = new Action<float, float>((x, y) =>
            {
                   animate = true;                
                    x = 0;
               
                
            });

           
        }


        int x = 0;
  
        protected override void Draw()
        {
            
            Background(0, 0);            
            if (animate)
            if (x + 15 < Width+50)
            {
                x += 15;
            }
            else
            {
                x = 0;
                animate = false;
            }
          
            fill(0, 204, 255);
            Rect(0, 0, Width, Height);
            fill(0, 104, 105);
            Rect(0, 0, x, Height);
            fill("white");
            textSize(30);
            
            Text("Button" );
            
        }
    }


}
