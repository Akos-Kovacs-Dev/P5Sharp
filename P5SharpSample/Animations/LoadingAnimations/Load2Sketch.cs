using P5Sharp;
using SkiaSharp;
using System.Collections.Generic;
using System.Linq;
namespace P5SharpSample.Animations.LoadingAnimations
{

    
    public class Cell : P5Object
    {

        float x;
        float y;
        float s;
        int i = 0;
        float maxs;
        int delay = 0;
        int FrameCount = 0;
         
        List<float> bounceScaleCurve = new List<float>()
{
    // Smooth shrink (1.0 → 0.0)
    1.00f, 0.98f, 0.96f, 0.94f, 0.92f, 0.90f, 0.87f, 0.84f,
    0.80f, 0.76f, 0.72f, 0.68f, 0.63f, 0.58f, 0.52f, 0.46f,
    0.40f, 0.34f, 0.28f, 0.22f, 0.16f, 0.10f, 0.06f, 0.03f,
    0.01f,

    // Hold at 0.0
    0.00f, 0.00f, 0.00f, 0.00f, 0.00f, 0.00f, 0.00f, 0.00f,
    0.00f, 0.00f,

    // Fast grow back to 1.0
    0.05f, 0.10f, 0.17f, 0.25f, 0.35f, 0.45f, 0.57f, 0.68f,
    0.78f, 0.86f, 0.92f, 0.96f, 0.98f, 0.99f, 1.00f
};
        public Cell(float x, float y, float s, int d)
        {
            this.x = x;
            this.y = y;
            this.s = s;
            this.maxs = s;
            delay = d;
        }

         

        protected override void Draw()
        {

            FrameCount++;
            if (delay > FrameCount)
            {
                rect(x, y, s, s);
                return;
            }
            
                
            



                i++;
            if (i > bounceScaleCurve.Count - 1)
                i = 0;
           
          

            s = maxs * bounceScaleCurve[i];
            rect(x, y, s, s);


        }

    }


    public partial class Load2Sketch : SketchBase
    {


        List<Cell> cells = new List<Cell>();

  

        protected override void Setup()
        {
          
        
            noStroke();
            fill(4, 106, 175);
            RectMode(Rectmode.CENTER);
            float cellW = Width / 3;

            int delay = 0;
            for(int x = 0; x < 3; x++)
            {
                for (int y = 3; y >= 0; y--)
                {
                    delay += 3;
                    cells.Add(new Cell( (x* cellW)+cellW/2, (y* cellW)+cellW/2, cellW, delay));
                }
            }
            
        }

        protected override void Draw()
        {
            Background(0,0);

            foreach(var c in cells)
            {
                c.OnDraw(this);
            }
        


        }
    }


}
