using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using P5Sharp;
using P5SharpSample.Helpers;
using P5SharpSample.Interfaces;
using P5SharpSample.ViewModels;
using P5SharpSample.Views;


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

            #region Views
            builder.Services.AddSingleton<ButtonsPage>();
            #endregion

            #region ViewModels
            builder.Services.AddSingleton<ButtonsPageViewModel>();
            #endregion

            #region Services
            builder.Services.AddSingleton<IMockAPIService, MockAPIService>();
            #endregion


            return builder.Build();
        }
    }
}
