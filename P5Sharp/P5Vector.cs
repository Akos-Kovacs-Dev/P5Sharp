using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P5Sharp
{
    public class P5Vector
    {
        public float x;
        public float y;
        public float z;

        public P5Vector(float x = 0, float y = 0, float z = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        // ----- Static Creation -----
        public static P5Vector CreateVector(float x, float y, float z = 0) => new P5Vector(x, y, z);

        public static P5Vector FromAngle(float angle)
        {
            return new P5Vector((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        public static P5Vector FromAngles(float theta, float phi)
        {
            float sinPhi = (float)Math.Sin(phi);
            float x = (float)(Math.Cos(theta) * sinPhi);
            float y = (float)(Math.Sin(theta) * sinPhi);
            float z = (float)Math.Cos(phi);
            return new P5Vector(x, y, z);
        }

        public static P5Vector Random2D()
        {
            double angle = new Random().NextDouble() * 2 * Math.PI;
            return FromAngle((float)angle);
        }

        public static P5Vector Random3D()
        {
            var rand = new Random();
            float theta = (float)(rand.NextDouble() * 2 * Math.PI);
            float phi = (float)(Math.Acos(2 * rand.NextDouble() - 1));
            return FromAngles(theta, phi);
        }

        // ----- Vector Operations -----
        public void Add(P5Vector v)
        {
            x += v.x;
            y += v.y;
            z += v.z;
        }

        public void Sub(P5Vector v)
        {
            x -= v.x;
            y -= v.y;
            z -= v.z;
        }

        public void Mult(float n)
        {
            x *= n;
            y *= n;
            z *= n;
        }

        public void Div(float n)
        {
            if (n != 0)
            {
                x /= n;
                y /= n;
                z /= n;
            }
        }

        public void Rem(float n)
        {
            x %= n;
            y %= n;
            z %= n;
        }

        public float Mag() => (float)Math.Sqrt(x * x + y * y + z * z);

        public float MagSq() => x * x + y * y + z * z;

        public void Normalize()
        {
            float m = Mag();
            if (m > 0) Div(m);
        }

        public void SetMag(float len)
        {
            Normalize();
            Mult(len);
        }

        public void Limit(float max)
        {
            if (MagSq() > max * max)
            {
                Normalize();
                Mult(max);
            }
        }

        public float Heading()
        {
            return (float)Math.Atan2(y, x); // 2D heading
        }

        public void SetHeading(float angle)
        {
            float m = Mag();
            x = (float)Math.Cos(angle) * m;
            y = (float)Math.Sin(angle) * m;
        }

        public void Rotate(float angle)
        {
            float newX = x * (float)Math.Cos(angle) - y * (float)Math.Sin(angle);
            float newY = x * (float)Math.Sin(angle) + y * (float)Math.Cos(angle);
            x = newX;
            y = newY;
        }

        public static float Dot(P5Vector a, P5Vector b) => a.x * b.x + a.y * b.y + a.z * b.z;

        public static P5Vector Cross(P5Vector a, P5Vector b)
        {
            return new P5Vector(
                a.y * b.z - a.z * b.y,
                a.z * b.x - a.x * b.z,
                a.x * b.y - a.y * b.x
            );
        }

        public static float Dist(P5Vector a, P5Vector b)
        {
            return (float)Math.Sqrt(
                (a.x - b.x) * (a.x - b.x) +
                (a.y - b.y) * (a.y - b.y) +
                (a.z - b.z) * (a.z - b.z)
            );
        }

        public static float AngleBetween(P5Vector v1, P5Vector v2)
        {
            float dot = Dot(v1, v2);
            float mags = v1.Mag() * v2.Mag();
            return (float)Math.Acos(dot / mags);
        }

        public void Reflect(P5Vector normal)
        {
            P5Vector n = normal.Copy();
            n.Normalize();
            float d = 2 * Dot(this, n);
            n.Mult(d);
            this.Sub(n);
        }

        public P5Vector Copy() => new P5Vector(x, y, z);

        public bool Equals(P5Vector v) => x == v.x && y == v.y && z == v.z;

        public float[] Array() => new float[] { x, y, z };

        public static P5Vector Lerp(P5Vector v1, P5Vector v2, float amt)
        {
            return new P5Vector(
                v1.x + (v2.x - v1.x) * amt,
                v1.y + (v2.y - v1.y) * amt,
                v1.z + (v2.z - v1.z) * amt
            );
        }

        public void ClampToZero(float epsilon = 1e-6f)
        {
            if (Math.Abs(x) < epsilon) x = 0;
            if (Math.Abs(y) < epsilon) y = 0;
            if (Math.Abs(z) < epsilon) z = 0;
        }

        public void Set(float newX, float newY, float newZ = 0)
        {
            x = newX;
            y = newY;
            z = newZ;
        }

        public override string ToString() => $"P5Vector({x}, {y}, {z})";
    }
}
