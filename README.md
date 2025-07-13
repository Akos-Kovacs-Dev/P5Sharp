![P5Sharp Logo](https://raw.githubusercontent.com/Akos-Kovacs-Dev/P5Sharp/master/Images/P5Sharp.png)

**P5Sharp** is a creative coding library for .NET and .NET MAUI, inspired by the popular p5.js framework.  
It brings the intuitive and expressive coding style of p5.js to the C# world — powered by **SkiaSharp** for rendering and built-in **HOT RELOAD** for Android, iOS, and Windows.  
Enjoy a smooth, real-time development experience.



https://github.com/Akos-Kovacs-Dev/P5Sharp

https://www.youtube.com/@P5Sharp

P5Sharp Sync Extension for Hot Reload on ios and android! 
https://marketplace.visualstudio.com/items?itemName=AkosKovacs.P5SharpSync



Setup Guide

Requirements:
- Visual Studio with .NET MAUI workload installed
- .NET 8 SDK

Installation:
1. Install the NuGet package: P5Sharp
2. Install the P5SharpSync extension from the Visual Studio Marketplace

------------------------------------------------------------

Configure MauiProgram.cs:

using P5Sharp;

UseP5Sharp(new P5SharpConfig
{
    IP = "xxx.xxx.x.x",   // Get this from the P5SharpSync extension
    Port = 0              // Get this from the extension
});

Make sure the P5SharpSync server is running in Visual Studio before launching the app.

------------------------------------------------------------

Creating a Sketch:

In Visual Studio:
- Right-click your project
- Select "Add" → "New File" → "P5Sketch"

This creates a class that inherits from SketchBase.

------------------------------------------------------------

Using your Sketch in XAML:

<?xml version="1.0" encoding="utf-8" ?>
<ContentPage
    x:Class="YourNamespace.MainPage"
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:p5="clr-namespace:P5Sharp;assembly=P5Sharp"
    xmlns:sketch="clr-namespace:YourSketchNamespace">

    <Grid>
        <p5:P5SketchView FilesCsv="Path/To/YourSketch.cs">
            <p5:P5SketchView.Sketch>
                <sketch:YourSketchClass />
            </p5:P5SketchView.Sketch>
        </p5:P5SketchView>
    </Grid>

</ContentPage>

You can include multiple files in FilesCsv, separated by commas.

------------------------------------------------------------

Running the App:

- Run the app in Debug mode.
- Your sketch files should appear in the P5SharpSync output window.
- When you save changes to the sketch, hot reload will automatically apply them.
- Logs and errors are shown in the Output panel.

------------------------------------------------------------

Faster Development on Windows (Optional):

Use the local TPC server for faster reloads and no sync extension needed:

builder.Services.AddSingleton(new P5SharpConfig
{
    LocalTPCServer = true,
    ProjectPath = @"C:\Path\To\Your\Project",  // Use path from P5SharpSync extension
    IP = "127.0.0.1",
    Port = 0
});

When using LocalTPCServer, stop the P5SharpSync server.

------------------------------------------------------------

P5SketchView Options:

- FilesCsv: Comma-separated list of .cs sketch files//Main Sketch file is not optional!
- FrameRate: Sets the sketch frame rate (default: 60)
- HotReloadInDebug: Enables or disables hot reload (default: true)
- EnableTouchEvents: Enables touch interaction (default: true)
- ReDrawOnSizeChanged: Redraws when canvas size changes (default: true)

------------------------------------------------------------

