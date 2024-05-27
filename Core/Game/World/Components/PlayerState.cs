using Core.Engine;
using Core.Game.Data;
using Core.Game.World.Actor;
using Core.Utils.Math;

namespace Core.Game.World.Components
{
    public class PlayerState : Component, ICharacter
    {
        public CharacterInfo Info {get; private set;}

        public string Title  {get; private set;}

        public CharacterGear GearItemId  {get; private set;}
        public CharacterGear GearObjectId { get; private set;}

        public CharacterStats Stats {get; private set;}

        public bool IsMoving => _movement.DistanceToTarget > 0;

        public Vec2 Target => _movement.Target;

        public float TargetZ => _movement.ZTarget;

        public Vec2 Origin => gameObject.transform.Position;

        public float OriginZ => gameObject.transform.ZPosition;

        public int Heading => gameObject.transform.Heading;

        private MovemventComponent _movement;

        public PlayerState(GameCharacterModel details)
        {
            Title = details.Title;
            Info = details.Info;
            Stats = details.Stats;
            GearObjectId = details.GearObjectId;
            GearItemId = details.GearItemId;
        }

        public override void OnStart()
        {
            _movement = gameObject.GetComponent<MovemventComponent>();
        }

    }
}
