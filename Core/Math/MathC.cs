namespace Core.Math
{
    public struct Vec2
    {
        public float x;
        public float y;

        public Vec2(float x, float y) 
            => (this.x, this.y) = (x,y);

        public Vec2(double x1, double y1) : this()
        {
           x = (float)x1;
           y = (float)y1;
        }

        public override string ToString()
        {
            return $"Vec2({x}, {y})";
        }
    }

    public static class MathC
    {
    }
}
