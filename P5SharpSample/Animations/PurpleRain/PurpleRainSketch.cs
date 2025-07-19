using P5Sharp;


namespace P5SharpSample.Animations.PurpleRain
{
    public class RainDrop : P5Object
    {
        float x;
        float y;
        float w;
        float h;
        float speed = 10;
        float z;        
        float thick;
        public RainDrop(float ch, float cw)
        {
            x = Random(0, cw);
            y = Random(ch * -3 ,-20);
            z = Random(0, 20);
            h = map(z, 0, 20, 10, 50);
            w = map(z, 0, 20, 5, 15);
            speed = map(z, 0, 20, 4, 10);
            thick = map(z, 0, 20, 1, 2);
        }

        protected override void Draw()
        {
           speed += 0.91f;
            y += speed;

            
            strokeWeight(thick);
            rect(x, y, w, h);

            if(y > Height)
            {
                y = Random(Height * -3, -20);
                speed = map(z, 0, 20, 4, 10);
            }
           
        }

        

    }
    //Ref: https://thecodingtrain.com/challenges/4-purple-rain
    public class PurpleRainSketch : SketchBase
    {
        List<RainDrop> RainList = new List<RainDrop>();

        protected override void Setup()
        {

            RainList.Clear();//want to clear them first on canvas resize
            stroke("black");
            fill(138, 43, 225);
            for (int i = 0; i < 500; i++)
            {
                RainList.Add(new RainDrop(Height,Width));
            }

        }
        protected override void Draw()
        {
            Background(230, 230, 250);
         
            

            foreach(var d in RainList)
            {
                d.OnDraw(this);
            }
        }


    }
}
