using P5SharpSample.ViewModels;

namespace P5SharpSample.Views;

public partial class ButtonsPage : ContentPage
{
	public ButtonsPage(ButtonsPageViewModel vm)
	{
		InitializeComponent();
		vm.Button4Sketch = Button4Sketch;
		BindingContext = vm;
	}
}