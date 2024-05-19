using System;

namespace Core.Math
{
    public struct Vec2
    {
        public float x;
        public float y;

        public Vec2(float x, float y)
            => (this.x, this.y) = (x, y);

        public Vec2(double x1, double y1) : this()
        {
            x = (float)x1;
            y = (float)y1;
        }

        public override string ToString()
        {
            return $"Vec2({x}, {y})";
        }

        public static Vec2 operator *(Vec2 lhs, float delta)
        {
            return new Vec2(lhs.x * delta, lhs.y * delta);
        }

        public static Vec2 operator +(Vec2 lhs, Vec2 rhs)
        {
            return new Vec2(lhs.x + rhs.x, lhs.y + rhs.y);
        }

        public static Vec2 operator -(Vec2 lhs, Vec2 rhs)
        {
            return new Vec2(lhs.x - rhs.x, lhs.y - rhs.y);
        }
    
        public static float Distance(Vec2 a , Vec2 b)
        {
            var dir = new Vec2(a.x - b.x, a.y - b.y);
            var dirLen = dir.x * dir.x + dir.y * dir.y;
            return MathF.Sqrt(dirLen);
        }

        public static Vec2 Direction(Vec2 from, Vec2 to)
        {
            var dir = new Vec2(to.x - from.x, to.y - from.y);
            var sqLen = dir.x * dir.x + dir.y * dir.y;
            var len = MathF.Sqrt(sqLen);
            dir.x /= len;
            dir.y /= len;
            return dir;
        }
    }

    public static class MathC
    {
    }
}
