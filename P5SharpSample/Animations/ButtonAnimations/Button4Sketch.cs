using P5Sharp;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace P5SharpSample.Animations.ButtonAnimations
{
    public partial class Button4Sketch : SketchBase
    {

        private bool ShowText = false;
        private bool Shrink = false;
        private bool Loading = false;
        private float ButtonWidth;
        private float ButtonHeight;
        private float ButtonRadius = 10;
 
        private float FrameCount = 0.1f;
        protected override void Setup()
        {
            ButtonWidth = Width - 20;
            ButtonHeight = Height - 20;
            ShowText = true;
            OnMousePress = new Action<float, float>((x, y) =>
            {
                Shrink = true;
            });

        }

        protected override void Draw()
        {
            Background(0,0);
             
            FrameCount += 0.3f;           
            if (FrameCount == TWO_PI)
                FrameCount = 0.1f;
            translate(Width / 2,Height/2);
            fill(21, 153, 249);
            RectMode(Rectmode.CENTER);
            noStroke();

            rect(0, 0, ButtonWidth, ButtonHeight, ButtonRadius);
           
            if (Shrink)
            {
                ShowText = false;
                ButtonRadius+=5;
                if (ButtonWidth-15 > ButtonHeight)
                {
                    ButtonWidth -= 15;
                }else if (ButtonWidth - 15 < ButtonHeight)
                {
                    ButtonWidth = ButtonHeight;
                    Shrink = false;
                    Loading = true;
                }                
                    
            }
            if(ShowText)
            {
                TextAlign(TextAlignment.CENTER);
                fill("white");
                text("Send", 0, 5);
            }

            if (Loading)
            {
                noFill();
                stroke("white");
                strokeWeight(3);

                float minArc = PI / 4;              // 45 degrees
                float maxArc = TWO_PI * 0.9f;       // ~324 degrees

                // Use sine to smoothly oscillate between minArc and maxArc
                float dynamicArcLength = map(sin(FrameCount * 0.5f), -1, 1, minArc, maxArc);

                float startAngle = FrameCount % TWO_PI;
                float endAngle = startAngle + dynamicArcLength;

                Arc(0, 0, 20, 20, startAngle, endAngle, ArcMode.PIE);
            }





        }
    }
}
