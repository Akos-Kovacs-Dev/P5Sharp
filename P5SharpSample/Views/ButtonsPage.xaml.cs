namespace P5SharpSample.Views;

public partial class ButtonsPage : ContentPage
{
	public ButtonsPage()
	{
		InitializeComponent();
		BindingContext = new ViewModels.ButtonsPageViewModel();
	}
}