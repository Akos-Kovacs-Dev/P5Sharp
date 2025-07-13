 
using P5Sharp;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace P5SharpSample.Animations.BackgroundAnimations
{
    public class Particle : P5Object
    {
        public float x;
        public float y;
        float xspeed;
        float yspeed;
        float r = 3;
        public Particle(float vw, float ch)
        {
            x = Random(0, vw);
            y = Random(0, ch);
            xspeed = Random((float)-1, 1);
            yspeed = Random((float)-1, 2);
           
        }
        protected override void Draw()
        {
            circle(x, y, r);
            x += xspeed;
            y += yspeed;

            if (x-r/2 < 0)
                xspeed *= -1;
            if (x+r/2 > Width)
                xspeed *= -1;
            if (y-r/2 < 0)
                yspeed *= -1;
            if (y+r/2 > Height)
                yspeed *= -1;

            if(random(0,100) == 1)
            {
                xspeed = Random((float)-1, 1);
                yspeed = Random((float)-1, 1);

            }
        }
        public (float,float) GetPosition()
        {
            return (x,y);
        }
    }
    public partial class LoginPageBackgroundSketch : SketchBase
    {
         float angle = 0f;
         float radius = 250f;
         float radiusspeed = 0.5f;
        List<Particle> Particles = new List<Particle>();

        protected override void Setup()
        {
            Particles.Clear();//on canvassizechange we want to remove them first
            Smooth();
            int particleCount = (Width * Height) / (100 * 100); // 1 per 100x100 px
            for (int i = 0; i < particleCount; i++)
            {
                Particles.Add(new Particle(Width,Height));
            }
        }

        protected override void Draw()
        {
            radius++;
           // Background(51);
            FillGradient(new[] { new SKColor(91,91,91), new SKColor(51,51,51) }, angleDegrees: 90, gradientType: GradientType.Linear, radialRadius: 0, customCenter: (0, 0));
            rect(0, 0, Width, Height);
            fill(237, 34, 93);
            noStroke();          
            foreach (var p in Particles)
                p.OnDraw(this);

            float maxDist = 100;

            for (int i = 0; i < Particles.Count; i++)
            {
                for (int j = i + 1; j < Particles.Count; j++)
                {
                    var posA = Particles[i].GetPosition();
                    var posB = Particles[j].GetPosition();

                    float d = dist(posA.Item1, posA.Item2, posB.Item1, posB.Item2);

                    if (d < maxDist)
                    {                         
                        float alpha = map(d, 0, maxDist, 255, 0);                         
                        stroke(255,255,255, alpha);
                        line(posA.Item1, posA.Item2, posB.Item1, posB.Item2);
                    }
                }

               

            }

        }
    }
}
