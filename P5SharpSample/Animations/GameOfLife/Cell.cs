using P5Sharp;


namespace P5SharpSample.Animations.GameOfLife
{
    public partial class Cell : P5Object
    {
        private float x, y, w, h;

        public int XPos { get; private set; }
        public int YPos { get; private set; }

        public bool live { get; set; } = false;
        public bool previousLive { get; set; } = false; 

        private bool? nextLife = null;

        public Cell(int xGrid, int yGrid, float width, float height)
        {
            XPos = xGrid;
            YPos = yGrid;

            w = width;
            h = height;

            x = XPos * w;
            y = YPos * h;
        }

        public void calculateLife(List<Cell> allCells)
        {
            int neighborCount = 0;

            foreach (var c in allCells)
            {
                if (c == this) continue;

                if (Math.Abs(c.XPos - this.XPos) <= 1 &&
                    Math.Abs(c.YPos - this.YPos) <= 1 &&
                    c.previousLive)
                {
                    neighborCount++;
                }
            }

            if (this.previousLive)
            {
                nextLife = neighborCount == 2 || neighborCount == 3;
            }
            else
            {
                nextLife = neighborCount == 3;
            }
        }

        public void CommitLife()
        {
            if (nextLife.HasValue)
            {
                live = nextLife.Value;
            }
        }



        protected override void Draw()
        {
            if (live)
                fill("black");
            else
                fill("white");

            rect(x, y, w, h);
        }
    }

}