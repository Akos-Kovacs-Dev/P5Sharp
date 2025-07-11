using System.Diagnostics;

namespace P5Sharp
{
    public class P5Math : P5Variables
    {
        #region Vars
        public static float PI = (float)Math.PI;
        public static float HALF_PI = (float)Math.PI / 2;
        public static float TWO_PI = (float)Math.PI * 2;
        #endregion
        #region Math

        /// <summary>Returns the absolute value of a float.</summary>
        public static float Abs(float n) => Math.Abs(n);
        /// <summary>Returns the absolute value of an int.</summary>
        public static int Abs(int n) => Math.Abs(n);
        /// <summary>Alias for <see cref="Abs(float)"/>.</summary>
        public static float abs(float n) => Abs(n);
        /// <summary>Alias for <see cref="Abs(int)"/>.</summary>
        public static int abs(int n) => Abs(n);

        /// <summary>Returns the smallest integer greater than or equal to the float.</summary>
        public static float Ceil(float n) => (float)Math.Ceiling(n);
        /// <summary>Returns the same integer (ceil of int is identity).</summary>
        public static int Ceil(int n) => n;
        /// <summary>Alias for <see cref="Ceil(float)"/>.</summary>
        public static float ceil(float n) => Ceil(n);
        /// <summary>Alias for <see cref="Ceil(int)"/>.</summary>
        public static int ceil(int n) => Ceil(n);

        /// <summary>Constrains a float value between min and max.</summary>
        public static float Constrain(float n, float min, float max) => Math.Max(min, Math.Min(n, max));
        /// <summary>Constrains an int value between min and max.</summary>
        public static int Constrain(int n, int min, int max) => Math.Max(min, Math.Min(n, max));
        /// <summary>Alias for <see cref="Constrain(float, float, float)"/>.</summary>
        public static float constrain(float n, float min, float max) => Constrain(n, min, max);
        /// <summary>Alias for <see cref="Constrain(int, int, int)"/>.</summary>
        public static int constrain(int n, int min, int max) => Constrain(n, min, max);

        /// <summary>Calculates the distance between two points (float).</summary>
        public static float Dist(float x1, float y1, float x2, float y2)
        {
            float dx = x2 - x1;
            float dy = y2 - y1;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }
        /// <summary>Calculates the distance between two points (int).</summary>
        public static float Dist(int x1, int y1, int x2, int y2)
        {
            float dx = x2 - x1;
            float dy = y2 - y1;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }
        /// <summary>Alias for <see cref="Dist(float, float, float, float)"/>.</summary>
        public static float dist(float x1, float y1, float x2, float y2) => Dist(x1, y1, x2, y2);
        /// <summary>Alias for <see cref="Dist(int, int, int, int)"/>.</summary>
        public static float dist(int x1, int y1, int x2, int y2) => Dist(x1, y1, x2, y2);

        /// <summary>Returns e raised to the power of the given float.</summary>
        public static float Exp(float n) => (float)Math.Exp(n);
        /// <summary>Alias for <see cref="Exp(float)"/>.</summary>
        public static float exp(float n) => Exp(n);

        /// <summary>Returns the largest integer less than or equal to the float.</summary>
        public static float Floor(float n) => (float)Math.Floor(n);
        /// <summary>Returns the same integer (floor of int is identity).</summary>
        public static int Floor(int n) => n;
        /// <summary>Alias for <see cref="Floor(float)"/>.</summary>
        public static float floor(float n) => Floor(n);
        /// <summary>Alias for <see cref="Floor(int)"/>.</summary>
        public static int floor(int n) => Floor(n);

        /// <summary>Returns the fractional part of the float.</summary>
        public static float Fract(float n) => n - (float)Math.Floor(n);
        /// <summary>Alias for <see cref="Fract(float)"/>.</summary>
        public static float fract(float n) => Fract(n);

        /// <summary>Linear interpolation between start and stop by amt (float).</summary>
        public static float Lerp(float start, float stop, float amt) => start + (stop - start) * amt;
        /// <summary>Linear interpolation between start and stop by amt (int).</summary>
        public static float Lerp(int start, int stop, float amt) => start + (stop - start) * amt;
        /// <summary>Alias for <see cref="Lerp(float, float, float)"/>.</summary>
        public static float lerp(float start, float stop, float amt) => Lerp(start, stop, amt);
        /// <summary>Alias for <see cref="Lerp(int, int, float)"/>.</summary>
        public static float lerp(int start, int stop, float amt) => Lerp(start, stop, amt);

        /// <summary>Returns the natural logarithm (base e) of the float.</summary>
        public static float Log(float n) => (float)Math.Log(n);
        /// <summary>Alias for <see cref="Log(float)"/>.</summary>
        public static float log(float n) => Log(n);

        /// <summary>Calculates the magnitude of a vector (x,y) float.</summary>
        public static float Mag(float x, float y) => (float)Math.Sqrt(x * x + y * y);
        /// <summary>Calculates the magnitude of a vector (x,y) int.</summary>
        public static float Mag(int x, int y) => (float)Math.Sqrt(x * x + y * y);
        /// <summary>Alias for <see cref="Mag(float, float)"/>.</summary>
        public static float mag(float x, float y) => Mag(x, y);
        /// <summary>Alias for <see cref="Mag(int, int)"/>.</summary>
        public static float mag(int x, int y) => Mag(x, y);

        /// <summary>Re-maps a number from one range to another (float).</summary>
        public static float Map(float value, float start1, float stop1, float start2, float stop2)
        {
            float outgoing = start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
            return outgoing;
        }
        /// <summary>Re-maps a number from one range to another (int).</summary>
        public static float Map(int value, int start1, int stop1, int start2, int stop2)
        {
            float outgoing = start2 + (stop2 - start2) * ((value - start1) / (float)(stop1 - start1));
            return outgoing;
        }
        /// <summary>Alias for <see cref="Map(float, float, float, float, float)"/>.</summary>
        public static float map(float value, float start1, float stop1, float start2, float stop2) => Map(value, start1, stop1, start2, stop2);
        /// <summary>Alias for <see cref="Map(int, int, int, int, int)"/>.</summary>
        public static float map(int value, int start1, int stop1, int start2, int stop2) => Map(value, start1, stop1, start2, stop2);

        /// <summary>Returns the maximum float value from the array.</summary>
        public static float Max(params float[] values)
        {
            if (values.Length == 0) throw new ArgumentException("Max requires at least one value.");
            float max = values[0];
            foreach (float val in values) if (val > max) max = val;
            return max;
        }
        /// <summary>Returns the maximum int value from the array.</summary>
        public static int Max(params int[] values)
        {
            if (values.Length == 0) throw new ArgumentException("Max requires at least one value.");
            int max = values[0];
            foreach (int val in values) if (val > max) max = val;
            return max;
        }
        /// <summary>Alias for <see cref="Max(float[])"/>.</summary>
        public static float max(params float[] values) => Max(values);
        /// <summary>Alias for <see cref="Max(int[])"/>.</summary>
        public static int max(params int[] values) => Max(values);
        /// <summary>Returns the minimum float value from the array.</summary>
        public static float Min(params float[] values)
        {
            if (values.Length == 0) throw new ArgumentException("Min requires at least one value.");
            float min = values[0];
            foreach (float val in values) if (val < min) min = val;
            return min;
        }
        /// <summary>Returns the minimum int value from the array.</summary>
        public static int Min(params int[] values)
        {
            if (values.Length == 0) throw new ArgumentException("Min requires at least one value.");
            int min = values[0];
            foreach (int val in values) if (val < min) min = val;
            return min;
        }
        /// <summary>Alias for <see cref="Min(float[])"/>.</summary>
        public static float min(params float[] values) => Min(values);
        /// <summary>Alias for <see cref="Min(int[])"/>.</summary>
        public static int min(params int[] values) => Min(values);
        /// <summary>Normalizes a value within a range (float).</summary>
        public static float Norm(float value, float start, float stop)
        {
            return (value - start) / (stop - start);
        }
        /// <summary>Alias for <see cref="Norm(float, float, float)"/>.</summary>
        public static float norm(float value, float start, float stop) => Norm(value, start, stop);
        /// <summary>Returns baseNum raised to the power of exponent (float).</summary>
        public static float Pow(float baseNum, float exponent) => (float)Math.Pow(baseNum, exponent);
        /// <summary>Returns baseNum raised to the power of exponent (int baseNum).</summary>
        public static float Pow(int baseNum, float exponent) => (float)Math.Pow(baseNum, exponent);
        /// <summary>Alias for <see cref="Pow(float, float)"/>.</summary>
        public static float pow(float baseNum, float exponent) => Pow(baseNum, exponent);
        /// <summary>Alias for <see cref="Pow(int, float)"/>.</summary>
        public static float pow(int baseNum, float exponent) => Pow(baseNum, exponent);
        /// <summary>Rounds a float to the nearest integer value.</summary>
        public static float Round(float n) => (float)Math.Round(n);
        /// <summary>Rounds an int (identity).</summary>
        public static int Round(int n) => n;
        /// <summary>Alias for <see cref="Round(float)"/>.</summary>
        public static float round(float n) => Round(n);
        /// <summary>Alias for <see cref="Round(int)"/>.</summary>
        public static int round(int n) => Round(n);
        /// <summary>Returns the square of a float.</summary>
        public static float Sq(float n) => n * n;
        /// <summary>Returns the square of an int.</summary>
        public static int Sq(int n) => n * n;
        /// <summary>Alias for <see cref="Sq(float)"/>.</summary>
        public static float sq(float n) => Sq(n);
        /// <summary>Alias for <see cref="Sq(int)"/>.</summary>
        public static int sq(int n) => Sq(n);
        /// <summary>Returns the square root of a float.</summary>
        public static float Sqrt(float n) => (float)Math.Sqrt(n);
        /// <summary>Returns the square root of an int (float result).</summary>
        public static float Sqrt(int n) => (float)Math.Sqrt(n);
        /// <summary>Alias for <see cref="Sqrt(float)"/>.</summary>
        /// public static float sqrt(float n) => Sqrt(n);
        /// <summary>Alias for <see cref="Sqrt(int)"/>.</summary>
        public static float sqrt(int n) => Sqrt(n);
        #endregion
        #region Random        
        private static Random rng = new Random();

        /// <summary>Returns a random float between 0.0 (inclusive) and 1.0 (exclusive).</summary>
        public static float Random()
        {
            return rng.NextSingle();
        }
        /// <summary>Returns a random float between 0.0 (inclusive) and max (exclusive).</summary>
        public static float Random(float max)
        {
            return rng.NextSingle() * max;
        }
        /// <summary>Returns a random byte between 0 (inclusive) and max (exclusive), max capped at 255.</summary>
        public static byte Random(byte max)
        {
            if (max > 255)
                max = 255;

            return (byte)rng.Next(0, max);
        }
        /// <summary>Returns a random integer between 0 (inclusive) and max (exclusive).</summary>
        public static int Random(int max)
        {
            return rng.Next(0, max);
        }
        /// <summary>Returns a random double between min (inclusive) and max (exclusive).</summary>
        public static double Random(double min, double max)
        {
            return min + rng.NextDouble() * (max - min);
        }
        /// <summary>Returns a random float between min (inclusive) and max (exclusive).</summary>
        public static float Random(float min, float max)
        {
            return min + rng.NextSingle() * (max - min);
        }
        /// <summary>Returns a random integer between min (inclusive) and max (exclusive).</summary>
        public static int Random(int min, int max)
        {
            return rng.Next(min, max);
        }
        /// <summary>Returns a random byte between min (inclusive) and max (exclusive).</summary>
        public static byte Random(byte min, byte max)
        {
            return (byte)rng.Next(min, max);
        }
        /// <summary>Returns a random element from the provided array.</summary>
        public static T Random<T>(T[] array)
        {
            if (array == null || array.Length == 0)
                throw new ArgumentException("Array cannot be null or empty.");

            int index = rng.Next(array.Length);
            return array[index];
        }
        /// <summary>Sets the random seed to produce a repeatable sequence.</summary>
        public static void RandomSeed(int seed)
        {
            rng = new Random(seed);
        }
        /// <summary>Returns a random float between 0.0 (inclusive) and 1.0 (exclusive).</summary>
        public static float random() => Random();
        /// <summary>Returns a random float between 0.0 (inclusive) and max (exclusive).</summary>
        public static float random(float max) => Random(max);
        /// <summary>Returns a random byte between 0 (inclusive) and max (exclusive), max capped at 255.</summary>
        public static byte random(byte max) => Random(max);
        /// <summary>Returns a random integer between 0 (inclusive) and max (exclusive).</summary>
        public static int random(int max) => Random(max);
        /// <summary>Returns a random double between min (inclusive) and max (exclusive).</summary>
        public static double random(double min, double max) => Random(min, max);
        /// <summary>Returns a random float between min (inclusive) and max (exclusive).</summary>
        public static float random(float min, float max) => Random(min, max);
        /// <summary>Returns a random integer between min (inclusive) and max (exclusive).</summary>
        public static int random(int min, int max) => Random(min, max);
        /// <summary>Returns a random byte between min (inclusive) and max (exclusive).</summary>
        public static byte random(byte min, byte max) => Random(min, max);
        /// <summary>Returns a random element from the provided array.</summary>
        public static T random<T>(T[] array) => Random(array);
        /// <summary>Sets the random seed to produce a repeatable sequence.</summary>
        public static void randomSeed(int seed) => RandomSeed(seed);
        #endregion
        #region Perlin

        private static int[] p; // Permutation array
        private static int octaves = 4;
        private static float falloff = 0.5f;
        /// <summary>Sets the seed for noise generation.</summary>
        public static void NoiseSeed(int seed)
        {
            Random rng = new Random(seed);
            p = new int[512];
            int[] permutation = new int[256];

            for (int i = 0; i < 256; i++)
                permutation[i] = i;

            for (int i = 255; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                int temp = permutation[i];
                permutation[i] = permutation[j];
                permutation[j] = temp;
            }

            for (int i = 0; i < 512; i++)
                p[i] = permutation[i % 256];
        }
        /// <summary>Sets the level of detail and falloff factor for noise generation.</summary>
        public static void NoiseDetail(int lod, float falloffFactor)
        {
            octaves = Math.Max(1, lod);
            falloff = falloffFactor;
        }
        /// <summary>Generates noise value for 1D coordinate.</summary>
        public static float Noise(float x) => Noise(x, 0, 0);
        /// <summary>Generates noise value for 2D coordinates.</summary>
        public static float Noise(float x, float y) => Noise(x, y, 0);
        /// <summary>Generates noise value for 3D coordinates.</summary>
        public static float Noise(float x, float y, float z)
        {
            float total = 0;
            float frequency = 1f;
            float amplitude = 1f;
            float maxValue = 0;

            for (int i = 0; i < octaves; i++)
            {
                total += Perlin(x * frequency, y * frequency, z * frequency) * amplitude;
                maxValue += amplitude;
                amplitude *= falloff;
                frequency *= 2;
            }

            return total / maxValue;
        }
        private static float Perlin(float x, float y, float z)
        {
            int X = (int)Math.Floor(x) & 255;
            int Y = (int)Math.Floor(y) & 255;
            int Z = (int)Math.Floor(z) & 255;

            x -= (float)Math.Floor(x);
            y -= (float)Math.Floor(y);
            z -= (float)Math.Floor(z);

            float u = Fade(x);
            float v = Fade(y);
            float w = Fade(z);

            int A = p[X] + Y;
            int AA = p[A] + Z;
            int AB = p[A + 1] + Z;
            int B = p[X + 1] + Y;
            int BA = p[B] + Z;
            int BB = p[B + 1] + Z;

            float result = Lerp(w,
                Lerp(v,
                    Lerp(u, Grad(p[AA], x, y, z), Grad(p[BA], x - 1, y, z)),
                    Lerp(u, Grad(p[AB], x, y - 1, z), Grad(p[BB], x - 1, y - 1, z))),
                Lerp(v,
                    Lerp(u, Grad(p[AA + 1], x, y, z - 1), Grad(p[BA + 1], x - 1, y, z - 1)),
                    Lerp(u, Grad(p[AB + 1], x, y - 1, z - 1), Grad(p[BB + 1], x - 1, y - 1, z - 1)))
            );

            return (result + 1) / 2; // Normalize to 0 - 1
        }
        private static float Fade(float t) => t * t * t * (t * (t * 6 - 15) + 10);
        private static float Grad(int hash, float x, float y, float z)
        {
            int h = hash & 15;
            float u = h < 8 ? x : y;
            float v = h < 4 ? y : (h == 12 || h == 14 ? x : z);
            return ((h & 1) == 0 ? u : -u) +
                   ((h & 2) == 0 ? v : -v);
        }
        /// <summary>Sets the seed for noise generation.</summary>
        public static void noiseSeed(int seed) => NoiseSeed(seed);
        /// <summary>Sets the level of detail and falloff factor for noise generation.</summary>
        public static void noiseDetail(int lod, float falloffFactor) => NoiseDetail(lod, falloffFactor);
        /// <summary>Generates noise value for 1D coordinate.</summary>
        public static float noise(float x) => Noise(x);
        /// <summary>Generates noise value for 2D coordinates.</summary>
        public static float noise(float x, float y) => Noise(x, y);
        /// <summary>Generates noise value for 3D coordinates.</summary>
        public static float noise(float x, float y, float z) => Noise(x, y, z);
        #endregion
        #region Trigonometry

        /// <summary>Sets the angle mode (Degrees or Radians).</summary>
        public static void AngleMode(AngleModeType mode)
        {
            _angleMode = mode;
        }
        /// <summary>Converts an angle to radians if necessary.</summary>
        private static float ToRadians(float angle)
        {
            return _angleMode == AngleModeType.DEGREES ? (float)(Math.PI / 180) * angle : angle;
        }
        /// <summary>Converts radians to the current angle mode.</summary>
        private static float FromRadians(float radians)
        {
            return _angleMode == AngleModeType.DEGREES ? radians * (180f / (float)Math.PI) : radians;
        }
        /// <summary>Returns the cosine of an angle.</summary>
        public static float Cos(float angle) => (float)Math.Cos(ToRadians(angle));
        /// <summary>Returns the sine of an angle.</summary>
        public static float Sin(float angle) => (float)Math.Sin(ToRadians(angle));
        /// <summary>Returns the tangent of an angle.</summary>
        public static float Tan(float angle) => (float)Math.Tan(ToRadians(angle));
        /// <summary>Returns the arc cosine of a value.</summary>
        public static float Acos(float value) => FromRadians((float)Math.Acos(value));
        /// <summary>Returns the arc sine of a value.</summary>
        public static float Asin(float value) => FromRadians((float)Math.Asin(value));
        /// <summary>Returns the arc tangent of a value.</summary>
        public static float Atan(float value) => FromRadians((float)Math.Atan(value));
        /// <summary>Returns the angle whose tangent is the quotient of two specified numbers.</summary>
        public static float Atan2(float y, float x) => FromRadians((float)Math.Atan2(y, x));
        /// <summary>Converts radians to degrees.</summary>
        public static float Degrees(float radians) => radians * (180f / (float)Math.PI);
        /// <summary>Converts degrees to radians.</summary>
        public static float Radians(float degrees) => degrees * ((float)Math.PI / 180f);
        /// <summary>Sets the angle mode (Degrees or Radians).</summary>
        public static void angleMode(AngleModeType mode) => AngleMode(mode);
        /// <summary>Returns the cosine of an angle.</summary>
        public static float cos(float angle) => Cos(angle);
        /// <summary>Returns the sine of an angle.</summary>
        public static float sin(float angle) => Sin(angle);
        /// <summary>Returns the tangent of an angle.</summary>
        public static float tan(float angle) => Tan(angle);
        /// <summary>Returns the arc cosine of a value.</summary>
        public static float acos(float value) => Acos(value);
        /// <summary>Returns the arc sine of a value.</summary>
        public static float asin(float value) => Asin(value);
        /// <summary>Returns the arc tangent of a value.</summary>
        public static float atan(float value) => Atan(value);
        /// <summary>Returns the angle whose tangent is the quotient of two specified numbers.</summary>
        public static float atan2(float y, float x) => Atan2(y, x);
        /// <summary>Converts radians to degrees.</summary>
        public static float degrees(float radians) => Degrees(radians);
        /// <summary>Converts degrees to radians.</summary>
        public static float radians(float degrees) => Radians(degrees);
        #endregion
        #region Time & Date
        private static readonly Stopwatch stopwatch = Stopwatch.StartNew();
        /// <summary>Returns the current day of the month.</summary>
        public static int Day() => DateTime.Now.Day;
        /// <summary>Returns the current month.</summary>
        public static int Month() => DateTime.Now.Month;
        /// <summary>Returns the current year.</summary>
        public static int Year() => DateTime.Now.Year;
        /// <summary>Returns the current hour.</summary>
        public static int Hour() => DateTime.Now.Hour;
        /// <summary>Returns the current minute.</summary>
        public static int Minute() => DateTime.Now.Minute;
        /// <summary>Returns the current second.</summary>
        public static int Second() => DateTime.Now.Second;
        /// <summary>Returns the number of milliseconds elapsed since the application started.</summary>
        public static long Millis() => stopwatch.ElapsedMilliseconds;
        /// <summary>Returns the current day of the month.</summary>
        public static int day() => Day();
        /// <summary>Returns the current month.</summary>
        public static int month() => Month();
        /// <summary>Returns the current year.</summary>
        public static int year() => Year();
        /// <summary>Returns the current hour.</summary>
        public static int hour() => Hour();
        /// <summary>Returns the current minute.</summary>
        public static int minute() => Minute();
        /// <summary>Returns the current second.</summary>
        public static int second() => Second();
        /// <summary>Returns the number of milliseconds elapsed since the application started.</summary>
        public static long millis() => Millis();
        #endregion

    }
}
