using P5Sharp;

using SkiaSharp;
using System.Collections.Generic;
using System.Linq;
namespace P5SharpSample.Animations.LoadingAnimations
{



    public class LoadingParticle : P5Object
    {
        private float x;
        private float y;
        private float r;
        private float yspeed = -3;
        private float maxHeight = 30;
        private float yStart;
        private bool animating = false;
        private bool completed = false;
        private int delay;
        private int frameCount = 0;
        private SKColor color;

        public bool IsAnimating => animating;
        public bool IsCompleted => completed;

        public LoadingParticle(float x, float y, float r, SKColor color, int delay)
        {
            this.x = x;
            this.y = y;
            this.yStart = y;
            this.r = r;
            this.color = color;
            this.delay = delay;
        }

         

        public void Reset()
        {
            animating = false;
            completed = false;
            frameCount = 0;
            y = yStart;
            yspeed = -3;
        }

        protected override void Draw()
        {
            frameCount++;

            if (!animating && frameCount > delay)
            {
                animating = true;
            }

            fill(color);
            Circle(x, y, r);

            if (animating && !completed)
            {
                y += yspeed;

                if (y < yStart - maxHeight)
                {
                    yspeed *= -1;
                }
                else if (y > yStart)
                {
                    y = yStart;
                    yspeed *= -1;
                    animating = false;
                    completed = true;
                }
            }
        }
    }

    public partial class Load1Sketch : SketchBase
    {

        List<LoadingParticle> loadingParticles = new List<LoadingParticle>();

        protected override void Setup()
        {

            noStroke();

            float section = Width / 6;

            for (int i = 1; i < 6; i++)
            {
                loadingParticles.Add(new LoadingParticle(section * i, 50, 10, new SKColor( (byte)random(70,255), (byte)random(70, 255), (byte)random(70, 255)), i * 10));
            }

        }



        protected override void Draw()
        {

            Background("transparent");

            foreach (var p in loadingParticles)
            {
                p.OnDraw(this);
            }

            // If all particles have completed their cycle
            if (loadingParticles.All(p => p.IsCompleted))
            {
                foreach (var p in loadingParticles)
                {
                    p.Reset(); // Restart their animation
                }
            }


        }
    }


}
