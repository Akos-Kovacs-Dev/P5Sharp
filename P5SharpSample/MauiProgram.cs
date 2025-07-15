using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using P5Sharp;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace P5SharpSample
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()                 
                .UseP5Sharp(new P5SharpConfig
                {
                    IP = "192.168.0.7",
                    Port = 12345,                    
                    LocalTPCServer = true,
                })
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");                
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
