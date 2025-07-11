using CommunityToolkit;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.Input;
using P5Sharp;
using SkiaSharp.Views.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P5SharpSample.ViewModels
{
    public partial class MainPageViewModel 
    {



        private SKCanvasView ButtonCanvas;
        private P5Sketch SketchLoader;
       // private WaterRippleSketch WaterRippleSketch;


        public MainPageViewModel(SKCanvasView buttonCanvas)
        {

         //   P5SCanvasSettings gameoflifesettings = new P5SCanvasSettings()
         //   {
         //       IPAddress = "192.168.0.7",
         //       Port = 12345,
         //       CanvasView = ButtonCanvas = buttonCanvas,
         //       Files = new List<string>() { "Animations/ButtonAnimations/WaterRippleSketch.cs" },
         //       Sketch = WaterRippleSketch = new WaterRippleSketch(),
         //       LocalTPCServer = false,
         //       //ProjectPath = "C:\\Users\\kKova\\Documents\\P5SharpSample"
         //   };

        //    SketchLoader = new P5Sketch(gameoflifesettings);

        }


       
    }
}
