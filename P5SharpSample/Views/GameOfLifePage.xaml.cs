using P5Sharp;

namespace P5SharpSample.Views;

public partial class GameOfLifePage : ContentPage
{

    
    public GameOfLifePage()
    {
        InitializeComponent();



      

    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        GameOfLifeP5View.InvokeSketchAction("randomize","");


    }

    private void Slider_DragCompleted(object sender, EventArgs e)
    {
        int value = (int)(slider.Value);
        string newcellSize = value.ToString();
       

        GameOfLifeP5View.InvokeSketchAction("changeCellSize", newcellSize);
    }
}