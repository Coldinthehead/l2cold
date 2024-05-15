using Core.Math;

namespace Core.Game
{
    public class Player
    { 
        public readonly int ObjectId;
        public Vec2 Position;

        public Player(int objectId, Vec2 position)
        {
            ObjectId = objectId;
            Position = position;
        }

        public void Update()
        {
        }
    }
}
