using P5Sharp;
using SkiaSharp;
using System;

namespace P5SharpSample.Animations.ButtonAnimations
{


   

   
    public partial class Button4Sketch : SketchBase
    {
        private bool ShowText = false;
        private bool Shrink = false;
        private bool Loading = false;
        private float ButtonWidth;
        private float ButtonHeight;
        private float ButtonRadius = 10;
        private bool Running = false;
        private bool Completed = false;
        private bool Failed = false;

        private float FrameCount = 0.1f;
        private float checkmarkProgress = 0f;
        private float checkmarkSpeed = 0.05f;

        private float xProgress = 0f;
        private float xDrawSpeed = 0.05f;

        private int resetTimer = 0;
        private bool resetting = false;
        private float fadeProgress = 0f;
        private float fadeSpeed = 1f / 60f; // 60 FPS = 1 second

        private bool restoring = false;
        private float restoreProgress = 0f;
        private float restoreSpeed = 1f / 60f;

        private float originalButtonWidth;
        private float originalButtonHeight;
        private float originalRadius = 10f;

        protected override void Setup()
        {
            ButtonWidth = Width - 20;
            ButtonHeight = Height - 20;
            originalButtonWidth = ButtonWidth;
            originalButtonHeight = ButtonHeight;
            ShowText = true;

            OnMousePress = new Action<float, float>((x, y) =>
            {
                if (Running) return;

                Running = true;
                if (SketchCommand.CanExecute(null))
                    SketchCommand.Execute(null);

                Shrink = true;
            });

            SketchActions.Add("LoadCompleted", (empty) =>
            {
                Completed = true;
                checkmarkProgress = 0f;
                resetTimer = 300; // 5s at 60fps
                resetting = false;
                fadeProgress = 0f;
            });

            SketchActions.Add("LoadFailed", (empty) =>
            {
                Failed = true;
                xProgress = 0f;
                resetTimer = 300;
                resetting = false;
                fadeProgress = 0f;
            });
        }

        protected override void Draw()
        {
            Background(0, 0);
            FrameCount += 0.3f;
            if (FrameCount == TWO_PI) FrameCount = 0.1f;

            translate(Width / 2, Height / 2);
            RectMode(Rectmode.CENTER);
            noStroke();

            // === COLOR and STATE HANDLING ===
            if ((Completed || Failed) && !restoring)
            {
                if (resetTimer > 0)
                {
                    fill(Completed ? "green" : "red");
                    resetTimer--;
                    if (resetTimer == 0)
                        resetting = true;
                }
                else if (resetting)
                {
                    fadeProgress += fadeSpeed;
                    SKColor faded = Completed
                        ? LerpColor(SKColors.Green, new SKColor(21, 153, 249), fadeProgress)
                        : LerpColor(SKColors.Red, new SKColor(21, 153, 249), fadeProgress);

                    fill(faded);

                    if (fadeProgress >= 1f)
                    {
                        resetting = false;
                        restoring = true;
                        restoreProgress = 0f;
                    }
                }
            }
            else if (restoring)
            {
                restoreProgress += restoreSpeed;

                // Grow button back
                ButtonWidth = Lerp(ButtonWidth, originalButtonWidth, restoreProgress);
                ButtonRadius = Lerp(ButtonRadius, originalRadius, restoreProgress);

                SKColor faded = LerpColor(new SKColor(21, 153, 249), new SKColor(21, 153, 249), restoreProgress);
                fill(faded);

                if (restoreProgress >= 1f)
                    ResetToStart();
            }
            else
            {
                fill(21, 153, 249);
            }

            // === DRAW BUTTON ===
            rect(0, 0, ButtonWidth, ButtonHeight, ButtonRadius);

            if (Shrink)
            {
                ShowText = false;
                ButtonRadius += 5;
                if (ButtonWidth - 15 > ButtonHeight)
                {
                    ButtonWidth -= 15;
                }
                else
                {
                    ButtonWidth = ButtonHeight;
                    Shrink = false;
                    Loading = true;
                }
            }

            // === TEXT ===
            if (ShowText)
            {
                TextAlign(TextAlignment.CENTER);
                fill("white");
                text("Send", 0, 5);
            }

            // === LOADING SPINNER ===
            if (!Completed && !Failed && Loading)
            {
                noFill();
                stroke("white");
                strokeWeight(3);

                float minArc = PI / 4;
                float maxArc = TWO_PI * 0.9f;

                float dynamicArcLength = map(sin(FrameCount * 0.5f), -1, 1, minArc, maxArc);
                float startAngle = FrameCount % TWO_PI;
                float endAngle = startAngle + dynamicArcLength;

                Arc(0, 0, 20, 20, startAngle, endAngle, ArcMode.PIE);
            }

            // === CHECKMARK ===
            if (Completed)
            {
                noFill();
                stroke("white");
                strokeWeight(4);

                SKPoint start = new SKPoint(-10, 0);
                SKPoint mid = new SKPoint(-2, 10);
                SKPoint end = new SKPoint(12, -8);

                if (checkmarkProgress < 1f)
                    checkmarkProgress += checkmarkSpeed;

                if (checkmarkProgress < 0.5f)
                {
                    float t = map(checkmarkProgress, 0f, 0.5f, 0f, 1f);
                    SKPoint current = Lerp(start, mid, t);
                    line(start.X, start.Y, current.X, current.Y);
                }
                else
                {
                    line(start.X, start.Y, mid.X, mid.Y);
                    float t = map(checkmarkProgress, 0.5f, 1f, 0f, 1f);
                    SKPoint current = Lerp(mid, end, t);
                    line(mid.X, mid.Y, current.X, current.Y);
                }
            }

            // === FAIL X ===
            if (Failed)
            {
                noFill();
                stroke("white");
                strokeWeight(4);

                SKPoint start1 = new SKPoint(-10, -10);
                SKPoint end1 = new SKPoint(10, 10);
                SKPoint start2 = new SKPoint(-10, 10);
                SKPoint end2 = new SKPoint(10, -10);

                if (xProgress < 1f)
                    xProgress += xDrawSpeed;

                if (xProgress < 0.5f)
                {
                    float t = map(xProgress, 0f, 0.5f, 0f, 1f);
                    SKPoint current = Lerp(start1, end1, t);
                    line(start1.X, start1.Y, current.X, current.Y);
                }
                else
                {
                    line(start1.X, start1.Y, end1.X, end1.Y);
                    float t = map(xProgress, 0.5f, 1f, 0f, 1f);
                    SKPoint current = Lerp(start2, end2, t);
                    line(start2.X, start2.Y, current.X, current.Y);
                }
            }
        }

        // === HELPERS ===
        private SKPoint Lerp(SKPoint a, SKPoint b, float t)
        {
            return new SKPoint(
                a.X + (b.X - a.X) * t,
                a.Y + (b.Y - a.Y) * t
            );
        }

        private float Lerp(float a, float b, float t)
        {
            return a + (b - a) * t;
        }

        private SKColor LerpColor(SKColor from, SKColor to, float t)
        {
            byte r = (byte)(from.Red + (to.Red - from.Red) * t);
            byte g = (byte)(from.Green + (to.Green - from.Green) * t);
            byte b = (byte)(from.Blue + (to.Blue - from.Blue) * t);
            byte a = (byte)(from.Alpha + (to.Alpha - from.Alpha) * t);
            return new SKColor(r, g, b, a);
        }

        private void ResetToStart()
        {
            Completed = false;
            Failed = false;
            Running = false;
            ShowText = true;
            ButtonWidth = originalButtonWidth;
            ButtonHeight = originalButtonHeight;
            ButtonRadius = originalRadius;
            Loading = false;
            Shrink = false;
            fadeProgress = 0f;
            resetTimer = 0;
            resetting = false;
            restoring = false;
            restoreProgress = 0f;
        }
    }
  
}
