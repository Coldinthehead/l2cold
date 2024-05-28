using Core.Engine;
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
        public GameObject gameObject { get; }
        public GameCharacterModel Info { get; }
        public CharacterGear GearItemId { get; }
        public CharacterStats Stats { get; }
        public bool IsMoving { get; }

    }
}
