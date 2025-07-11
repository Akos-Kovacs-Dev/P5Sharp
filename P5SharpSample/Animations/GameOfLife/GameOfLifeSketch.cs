using P5Sharp;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace P5SharpSample.Animations.GameOfLife
{



    public partial class GameOfLifeSketch : SketchBase
    {
        int cellSize = 40;
        List<Cell> _cells;



        public GameOfLifeSketch()
        {
            SketchActions.Add("randomize", (inp) =>
            {

                _cells = new List<Cell>();
                for (int x = 0; x * cellSize + cellSize < Width; x++)
                {
                    for (int y = 0; y * cellSize + cellSize < Height; y++)
                    {
                        _cells.Add(new Cell(x, y, cellSize, cellSize));
                    }
                }

                for (int i = 0; i < 500; i++)
                {
                    var c = _cells[Random(0, _cells.Count)];
                    c.live = true;
                }
            });

            SketchActions.Add("changeCellSize", (inp) =>
            {

                cellSize = int.Parse(inp);

                _cells = new List<Cell>();
                for (int x = 0; x * cellSize + cellSize < Width; x++)
                {
                    for (int y = 0; y * cellSize + cellSize < Height; y++)
                    {
                        _cells.Add(new Cell(x, y, cellSize, cellSize));
                    }
                }


                for (int i = 0; i < 500; i++)
                {
                    var c = _cells[Random(0, _cells.Count)];
                    c.live = true;
                }

            });
        }


        protected override void Setup()
        {
            Stroke("black");
            strokeWeight(1);
            NoSmooth();

            _cells = new List<Cell>();
            for (int x = 0; x * cellSize + cellSize < Width; x++)
            {
                for (int y = 0; y * cellSize + cellSize < Height; y++)
                {
                    _cells.Add(new Cell(x, y, cellSize, cellSize));
                }
            }


            for (int i = 0; i < 500; i++)
            {
                var c = _cells[Random(0, _cells.Count)];
                c.live = true;
            }


        }
        float angle = 0;



        protected override void Draw()
        {

            Background(200);

            // Step 1: Store current states
            foreach (var cell in _cells)
            {
                cell.previousLive = cell.live;
            }

            // Step 2: Calculate next state based on previous states
            foreach (var cell in _cells)
            {
                cell.calculateLife(_cells);
            }

            // Step 3: Commit new states
            foreach (var cell in _cells)
            {
                cell.CommitLife();
            }

            // Step 4: Render cells
            foreach (var cell in _cells)
            {
                cell.OnDraw(this);
            }


            push();
            translate(Width / 2, Height / 2);
            rotate(angle);
            //  rect(-50, -25, 100, 50);  // Draws a rotated rectangle
            pop();
            smooth();
            angle += 0.05f;

        }
    }

}