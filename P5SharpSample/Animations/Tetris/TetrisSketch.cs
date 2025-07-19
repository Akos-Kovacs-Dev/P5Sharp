using Microsoft.Extensions.Options;
using P5Sharp;
 
using SkiaSharp;
using System;
using System.Collections.Generic;
 

namespace P5SharpSample.Animations.Tetris
{
    public static class Settings
    {
        public static readonly int cellSize = 20;
        public static readonly int gameCellWidth = 12;
        public static readonly int gameCellHeight = 20;
    }

    public partial class Cell(int xpos, int ypos, string color) : P5Object
    {
        public int xPos = xpos;
        public int yPos = ypos;
        public string color { get; set; } = color;
        public bool Solid { get; set; } =  false;

        protected override void Draw()
        {
            fill(color);
            rect(xPos*Settings.cellSize, yPos * Settings.cellSize, Settings.cellSize, Settings.cellSize);
        }
    }

    public partial class Shape : P5Object
    {
        public Cell[] cells = new Cell[4];
        public string color = "red";
        public Shape()
        {
            cells[0] = new Cell(3, 5,color);
            cells[1] = new Cell(4, 5, color);
            cells[2] = new Cell(5, 5, color);
            cells[3] = new Cell(6, 5, color);
        }       

      


        protected override void Draw()
        {

            Fall();
            foreach (Cell cell in cells)
            {
                 
                cell.OnDraw(this);
             
            }

      
            
           // rect(xPos * Settings.cellSize, yPos * Settings.cellSize, Settings.cellSize, Settings.cellSize);
        }
        private void Fall()
        {
            foreach (Cell cell in cells)
            {
                cell.yPos++; 
            }
        }
        public bool NexFrametoFallIsValid()
        {

            foreach (Cell cell in cells)
            {
                if (cell.yPos + 1 >= Settings.gameCellHeight)
                {

                    return false;
                }
            }

            return true;
        }
    }




    public partial class TetrisSketch : SketchBase
    {       
        List<Cell> Cells = new List<Cell>();
        Shape shape = new Shape();
        private float translateX;  
        private float translatey; 

        protected override void Setup()
        {

            translateX = (Width / 2) - (Settings.cellSize * Settings.gameCellWidth) / 2;
            translatey = (Height / 2) - (Settings.cellSize * Settings.gameCellHeight) / 2;
            for (int x = 0; x < Settings.gameCellWidth; x++)
            {
                for (int y = 0; y < Settings.gameCellHeight; y++)
                {
                    Cells.Add(new Cell(x, y,"white"));
                }
            }
            shape = new Shape();
        }

        protected override void Draw()
        {

            Background(51);
            translate(translateX,translatey);


            

          

                foreach (var c in Cells)
                {
                    c.OnDraw(this);
                }


            if (!shape.NexFrametoFallIsValid())
            {

                foreach (var sc in shape.cells)
                {
                    var cellToTurnSolid = Cells.Where(c => c.xPos == sc.xPos && c.yPos == sc.yPos).First();
                    if (cellToTurnSolid is not null)
                    {
                        cellToTurnSolid.color = shape.color;
                        cellToTurnSolid.Solid = true;
                    }

                }

                shape = new Shape();


            }
            else
            {
                shape.OnDraw(this);
            }



        }
    }
}
