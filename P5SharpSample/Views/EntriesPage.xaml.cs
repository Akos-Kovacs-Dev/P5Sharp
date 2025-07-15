using P5Sharp;

namespace P5SharpSample.Views;

public partial class EntriesPage : ContentPage
{
   
    public EntriesPage()
	{
		InitializeComponent();


        



    }

    private void NativeEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
       // _p5Sketch.SketchActions["Keystroke"].Invoke(e.NewTextValue);
      
    }
}