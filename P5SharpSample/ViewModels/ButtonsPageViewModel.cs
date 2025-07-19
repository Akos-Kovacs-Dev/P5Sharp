
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using P5Sharp;
using P5SharpSample.Interfaces;

namespace P5SharpSample.ViewModels
{
    public partial class ButtonsPageViewModel(IMockAPIService mockAPIService) : BaseViewModel
    {


        [ObservableProperty]
        public bool _mockAPICallSuccess;

        private P5SketchView? _button4Sketch;
        public P5SketchView Button4Sketch
        {
            get => _button4Sketch!;
            set
            {
                if (_button4Sketch is not null)
                    throw new InvalidOperationException("Button4Sketch can only be set once.");

                _button4Sketch = value ?? throw new ArgumentNullException(nameof(value));
            }
        }


        private readonly IMockAPIService _mockAPIService = mockAPIService;

        [RelayCommand]
        public async Task Button1()
        {
            await App.Current.MainPage.DisplayAlert("Alert", "Button1", "Ok");
        }


        bool success = false;

        [RelayCommand]
        public async Task Button4()
        {
            var result = await _mockAPIService.GetDataAsync();

            success = !success;
            await App.Current.MainPage.DisplayAlert("API call completed", $"Results {string.Join(", ", result)}", "ok");


            if (success)
            {
                Button4Sketch.InvokeSketchAction("LoadCompleted", "");

            }
            else
            {
                Button4Sketch.InvokeSketchAction("LoadFailed", "");
            }





        }

    }
}
