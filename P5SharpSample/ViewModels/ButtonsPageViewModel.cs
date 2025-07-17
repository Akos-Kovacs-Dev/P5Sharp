using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P5SharpSample.ViewModels
{
    public partial class ButtonsPageViewModel : BaseViewModel
    {

        [RelayCommand]
        public async Task Button1()
        {
            await App.Current.MainPage.DisplayAlert("Alert","Button1","Ok");
        }

    }
}
