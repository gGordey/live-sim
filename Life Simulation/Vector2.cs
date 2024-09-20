using System;
using System.Numerics;

namespace Life_Simulation
{
    class Vector2
    {
        private byte _x;
        private byte _y;

        public byte X { get { return _x; } }
        public byte Y { get { return _y; } }

        public Vector2(byte x, byte y) { _x = x; _y = y; }
        public Vector2(int x, int y) { _x = (byte)x; _y = (byte)y; }
        public Vector2() { _x = 0; _y = 0; }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(Vector2)) return false;
            Vector2 vector = (Vector2)obj;
            return X == vector._x && Y == vector._y;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Vector2 a, Vector2 b) { return a.Equals(b); }
        public static bool operator !=(Vector2 a, Vector2 b) { return !a.Equals(b); }

        public static Vector2 operator +(Vector2 a, Vector2 b) { return new Vector2 (a.X + b.X, a.Y + b.Y); }
        public static Vector2 operator -(Vector2 a, Vector2 b) { return new Vector2(a.X - b.X, a.Y - b.Y); }
    }
}
