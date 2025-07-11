namespace P5Sharp
{
    public static class SketchMessages
    {

        public static string ErrorSketch(string errorMessage)
        {

            
            // Break the error message into lines of max 50 characters
            var lines = WrapText(errorMessage, 50);
            // Start building the sketch code
            var sketch = $@"
                            using P5Sharp;
                            using SkiaSharp;
                            using System;
                            using System.Collections.Generic;
                            return new Sketch();
                            public partial class Sketch : SketchBase
                            {{
                                protected override void Setup() 
                                {{
                                  
                                         
                                }}

                                protected override void Draw()
                                {{
                                     
 
                                    Background(0);
                                    Fill(255, 0, 0);
                                    NoStroke();
                                    TextSize(Width/25);
                            ";

            // Add each line of text with vertical spacing
            int y = 50;
            foreach (var line in lines)
            {
                sketch += $"        Text(\"{EscapeForCode(line)}\", 0, {y});\n";
                y += 30; // spacing between lines
            }

            sketch += "    }\n}";

            return sketch;
        }

        // Splits a string into lines of max length `width`
        private static List<string> WrapText(string text, int width)
        {
            var result = new List<string>();
            for (int i = 0; i < text.Length; i += width)
            {
                int len = Math.Min(width, text.Length - i);
                result.Add(text.Substring(i, len));
            }
            return result;
        }

        // Escapes quotes and backslashes for code embedding
        private static string EscapeForCode(string input)
        {
            return input.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }


    }
}


