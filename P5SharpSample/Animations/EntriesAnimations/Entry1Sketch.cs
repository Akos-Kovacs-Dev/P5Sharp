using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using P5Sharp;
using SkiaSharp;

namespace P5SharpSample.Animations.EntriesAnimations
{
    public partial class Entry1Sketch : SketchBase
    {
        private string text = string.Empty;
        int frameCount = 0;
        List<string> textList = new List<string>();
        public Entry1Sketch()
        {
            SketchActions.Add("Keystroke", (input) =>
            {
                text = input;
            });
        }

        protected override void Setup()
        {
            
          
        }

        protected override void Draw()
        {
            Background(255, 55, 50, 255);
            frameCount++;
            NoStroke();
            if(frameCount % 15 != 0)
            {
                rect(5, 5, 2, Height - 10);

            }
            else
            {
                frameCount = 0;
            }


            if (text is null)
                return;

            foreach(var i in text)
            {
                textList.Add(i.ToString());
            }
            
            text(text, 10, 20);
        }
    }
}
