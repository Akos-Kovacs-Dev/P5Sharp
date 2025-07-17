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
        Entry1.InvokeSketchAction("Keystroke",e.NewTextValue);      
    }
}