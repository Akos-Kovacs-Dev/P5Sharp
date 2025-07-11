using P5Sharp;

namespace P5SharpSample.Views;

public partial class GameOfLifePage : ContentPage
{

    private P5Sketch _sketchLoader;
    public GameOfLifePage()
    {
        InitializeComponent();



        _sketchLoader = GameOfLifeP5View.P5Sketch;

    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        _sketchLoader.SketchActions["randomize"].Invoke("");


    }

    private void Slider_DragCompleted(object sender, EventArgs e)
    {
        int value = (int)(slider.Value);
        string newcellSize = value.ToString();
        _sketchLoader.SketchActions["changeCellSize"].Invoke(newcellSize);
    }
}