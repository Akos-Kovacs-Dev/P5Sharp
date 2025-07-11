using SkiaSharp.Views.Maui.Controls.Hosting;
using System.Runtime.InteropServices;

namespace P5Sharp
{

     



    public static class P5SharpMauiExtensions
    {
        public static MauiAppBuilder UseP5Sharp(this MauiAppBuilder builder, P5SharpConfig config)
        {
            config.Validate();
            builder.UseSkiaSharp();  
            builder.Services.AddSingleton(config);
            return builder;
        }
    }
}
