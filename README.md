P5Sharp is a creative coding library for .NET and .NET MAUI, inspired by the popular p5.js framework. It brings the intuitive and expressive coding style of p5.js to the C# world — using the power of SkiaSharp for rendering and built-in HOT RELOAD for Android, iOS, Windows for a smooth, real-time development experience.



https://github.com/Akos-Kovacs-Dev/P5Sharp

!!important!!
Add this line to your MauiProgram.cs
 .UseP5Sharp(new P5SharpConfig
                {
                    IP = "x.x.x.x",
                    Port = 12345,
                    //ProjectPath = @"C:\Users\YourUser\Documents\ProjectFolder", //needed when LocalTPCServer ture (only windows)
                    //LocalTPCServer = true // if is false use  https://marketplace.visualstudio.com/items?itemName=AkosKovacs.P5SharpSync
                })






