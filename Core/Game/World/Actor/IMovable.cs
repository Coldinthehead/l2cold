using Core.Game.Data;
using Core.Utils.Math;

namespace Core.Game.World.Actor
{
    public interface IMovable
    {
        public int ObjectId { get; }
        public Vec2 Target { get; }
        public float TargetZ { get; }
        public Vec2 Origin { get; }
        public float OriginZ { get; }
        public int Heading { get; }
    }

    public interface ICharacter : IMovable
    {
        public CharacterInfo Info { get; }
        public string Title { get; }
        public CharacterGear GearItemId { get; }
 
        public CharacterStats Stats { get; }

        public bool IsMoving { get; }

        public void Update(float dt);
    }
}
