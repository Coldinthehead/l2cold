
using Core.Utils.Math;

namespace Core.Engine
{
    public class Transform
    {
        public Vec2 Position;
        public float ZPosition;
        public int Heading;
        public readonly GameObject gameObject;

        public Transform(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }
    }
}
