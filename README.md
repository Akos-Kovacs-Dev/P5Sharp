
![P5Sharp Logo](https://raw.githubusercontent.com/Akos-Kovacs-Dev/P5Sharp/master/Images/P5Sharp.png)

**P5Sharp** is a creative coding library for .NET and .NET MAUI, inspired by the popular [p5.js](https://p5js.org/) framework.  
It brings the intuitive and expressive style of p5.js to the C# world — powered by **SkiaSharp** for rendering and built-in **HOT RELOAD** for Android, iOS, and Windows.  
Enjoy a smooth, real-time development experience.

🌐 GitHub: https://github.com/Akos-Kovacs-Dev/P5Sharp  
📺 YouTube: https://www.youtube.com/@P5Sharp  
🧩 P5Sharp Sync Extension (Hot Reload for iOS/Android):  
https://marketplace.visualstudio.com/items?itemName=AkosKovacs.P5SharpSync  

---

## 📽 Demo Videos

[![Demo 1](https://img.youtube.com/vi/iw2ZliGKVng/hqdefault.jpg)](https://www.youtube.com/watch?v=iw2ZliGKVng)  
[![Demo 2](https://img.youtube.com/vi/7EqMEnh99-Y/hqdefault.jpg)](https://www.youtube.com/watch?v=7EqMEnh99-Y)

---

## 🚀 Setup Guide

### Requirements:
- Visual Studio with .NET MAUI workload installed
- .NET 8 SDK

### Installation:
1. Install the NuGet package: `P5Sharp`
2. (Optional for mobile hot reload) Install the **P5SharpSync** extension from the Visual Studio Marketplace

---

## 🛠 Configure `MauiProgram.cs`

```csharp
using P5Sharp;

builder.UseP5Sharp(new P5SharpConfig
{
    IP = "xxx.xxx.x.x",     // Get from the P5SharpSync extension (or localhost)
    Port = 1234             // Get from the extension or set manually
});
```

**Note:** Make sure the **P5SharpSync server is running** before launching your app (unless using local TCP mode).

---

## 💻 Local TCP Server (Windows Only)

For a faster development experience on **Windows**, you can skip the Visual Studio extension and use a local TCP server:

```csharp
builder.UseP5Sharp(new P5SharpConfig
{
    LocalTPCServer = true,
    ProjectPath = @"C:\Path\To\Your\Project",  // Same folder where your `.csproj` lives
    IP = "127.0.0.1",
    Port = 1234                                // Use any available port
});
```

> ⚠️ Only available for Windows.  
> 🚫 Do NOT run P5SharpSync at the same time when using `LocalTPCServer`.

---

## ✏️ Creating a Sketch

In Visual Studio:
- Right-click your project
- Select **Add → New File → P5Sketch**

This generates a new class that inherits from `SketchBase`.  
This is your sketch entry point, like `setup()` and `draw()` in p5.js.

---

## 🧩 Splitting Code with `P5Objects`

You can break up your sketch into **multiple files** and classes.  
For example:


Update your XAML like this:

```xml
<p5:P5SketchView
    P5Objects="/Animations/MySketch/MyP5Object.cs"
    FrameRate="60"
    Sketch="{sketch:MySketch}" /> <!-- main Sketch> 
```

> ✅ `P5Sharp` will automatically resolve these classes to their file paths using reflection.
