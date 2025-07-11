using P5Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P5SharpSample.Animations.Matrix
{
    public static class allChar
    {
        private static List<string> all = new List<string>()
        {
            "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z"
        };
        public static string GetRandom()
        {
            return all[new Random().Next(0, all.Count)];
        }
    }
    public class Letter(float x, float y, int speed) : P5Object
    {
        public float y = y;
        float x = x;
        string l = allChar.GetRandom();
        protected override void Draw()
        {

            text(l, x, y);
            Fall();
        }
        private void Fall()
        {
            this.y+= speed;
        }
    }

    public class FallingText : P5Object
    {
        float x = 0;
        public List<Letter> list = new List<Letter>();


        private int CanvasWitdh;
        private int CanvasHeight;
        private int fontSize;
        public FallingText(float _anvasWitdh, int _canvasHeight, int _fontSize)
        {
            CanvasWitdh = (int)_anvasWitdh;//can access Width and Height in draw
            CanvasHeight = _canvasHeight;
            fontSize = _fontSize;

            x = new Random().Next(0, (int)CanvasWitdh);
            float yOffset = new Random().Next(-CanvasHeight, CanvasHeight);
            int speed = new Random().Next(1, 10);
            for (int i = 0; i < new Random().Next(10,30); i++)
            {
                list.Add(new Letter(x, yOffset + i * fontSize, speed));
            }
        }

      
        protected override void Draw()
        {
            foreach (var l in list)
            {
                l.OnDraw(this);
            }

            if(list.First().y > Height)
            {
                list.Clear();
                x = new Random().Next(0, (int)CanvasWitdh);
                float yOffset = new Random().Next(-(CanvasHeight*10), 0);
                int speed = new Random().Next(1, 10);
                for (int i = 0; i < new Random().Next(10, 30); i++)
                {
                    list.Add(new Letter(x, yOffset + i * fontSize, speed));
                }
            }
        }

    }

    public class MatrixSketch : SketchBase
    {


        List<FallingText> allText = new List<FallingText>();
        int fontSize = 30;
        protected override void Setup()
        {
            LoadFont("matrix.ttf");            
            Fill(34, 184, 6);
            textSize(fontSize);

            for(int i = 0; i < Width/fontSize*5; i++)
            {
                allText.Add(new FallingText(Width, (int)Height, fontSize));
            }
            
        }

        protected override void Draw()
        {
            Background("black");

            
            foreach (var at in allText)
            {
                at.OnDraw(this);
            }
           
        }
       
    }
}
