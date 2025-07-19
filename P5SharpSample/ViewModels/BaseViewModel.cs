using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace P5SharpSample.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {

        [ObservableProperty]
        private bool _isBusy;
        [ObservableProperty]
        private string _title; 

       
    }
}
