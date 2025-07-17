using CommunityToolkit.Mvvm.Input;
using P5Sharp;
using P5SharpSample.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace P5SharpSample.ViewModels
{
    public partial class ButtonsPageViewModel(IMockAPIService mockAPIService) : BaseViewModel
    {

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
            await App.Current.MainPage.DisplayAlert("Alert","Button1","Ok");
        }


        [RelayCommand]
        public async Task Button4()
        {
            var result = await _mockAPIService.GetDataAsync();

            await App.Current.MainPage.DisplayAlert("API call completed", $"Results {string.Join(", ", result)}", "ok");

            Button4Sketch.InvokeSketchAction("LoadCompleted", "");
            //Button4Sketch.InvokeSketchAction("LoadFailed", "");


          

        }

    }
}
