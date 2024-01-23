using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace CruZ
{
    public partial struct Vector3
    {
        public Vector3(float x = 0, float y = 0, float z = 0)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public float SqrMagnitude()
        {
            return X * X + Y * Y + Z * Z;
        }

        public static implicit operator Vector3(System.Numerics.Vector2 v)
        {
            return new(v.X, v.Y, 0);
        }

        public static implicit operator Vector3(Microsoft.Xna.Framework.Vector2 v)
        {
            return new(v.X, v.Y, 0);
        }

        public static explicit operator Microsoft.Xna.Framework.Vector2(Vector3 v)
        {
            return new(v.X, v.Y);
        }

        public static implicit operator System.Numerics.Vector3(Vector3 v)
        {
            return new(v.X, v.Y, v.Z);
        }

        public static implicit operator Microsoft.Xna.Framework.Vector3(Vector3 v)
        {
            return new(v.X, v.Y, v.Z);
        }

        public static implicit operator Vector3(Microsoft.Xna.Framework.Vector3 v)
        {
            return new(v.X, v.Y, v.Z);
        }


        public static Vector3 operator +(Vector3 left, Vector3 right)
        {
            return new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }

        public static Vector3 operator -(Vector3 left, Vector3 right)
        {
            return new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }

        public static Vector3 operator *(Vector3 v, float s)
        {
            return new(v.X * s, v.Y * s, v.Z * s);
        }


        public float X, Y, Z;


        public static Vector3 Zero => new(0, 0, 0);

        public static Vector3 One => new(1, 1, 1);
    }
}
