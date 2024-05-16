using Core.Game.Data;
using Core.Math;

namespace Core.Game
{
    public class Player
    { 
        public readonly int ObjectId;
        public Vec2 Position;
        public float ZPosition;

        public GameCharacter CharacterDetails;

        public Player(int objectId, Vec2 position, float zPosition, GameCharacter details)
        {
            ObjectId = objectId;
            Position = position;
            ZPosition = zPosition;
            CharacterDetails = details;
        }

        public void Update()
        {
        }
    }
}
