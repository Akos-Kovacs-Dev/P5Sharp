using P5Sharp;
using SkiaSharp;
using System;
using System.Collections.Generic;

namespace P5SharpSample.Animations.LoadingAnimations
{
    public partial class Load3Sketch : SketchBase
    {

        int perc = 0;
        float fontSize; 
        protected override void Setup()
        {
            fontSize = Width / 3;
            LoadFont("HomeVideo-BLG6G.ttf"); //https://www.fontspace.com/ggbotnet
            TextSize(fontSize);
            TextAlign(TextAlignment.CENTER);
            Fill("black");
        }

        protected override void Draw()
        {
            perc++;
            if (perc > 100)
                perc = 0;


            Background("transparent");
            translate(Width / 2, Height / 2);
            Text($"{perc}%", 0, fontSize/2);

        }
    }
}
