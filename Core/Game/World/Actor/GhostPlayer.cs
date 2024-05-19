
using Core.Game.Data;
using Core.Game.Network.ClientPacket;
using Core.Math;

namespace Core.Game.World.Actor
{
    public class GhostPlayer : IMovable , ICharacter
    {
        public event Action<IMovable> OnMovement;
        private MovementComponent _movement;
        public int ObjectId { get; private set; }
        public Vec2 Origin => _movement.Position;
        public float OriginZ { get;private set; }
        public Vec2 Target => _movement.Target;
        public float TargetZ { get; private set; } 
        public string Title { get; private set; }
        public CharacterInfo Info { get; private set; }
        public CharacterStats Stats { get; private set; }
        public CharacterGear GearObjectId { get; private set; }
        public CharacterGear GearItemId { get; private set; }

        public int Heading { get; private set; }
        public bool IsMoving => _movement.IsMoving;

        private int _pathIndex = 0;
        private List<Vec2> _path = new List<Vec2>();

        public GhostPlayer(int objectId, Vec2 position, float zPosition, GameCharacter details)
        {
            ObjectId = objectId;
            Title = details.Title;
            Info = details.Info;
            Stats = details.Stats;
            GearObjectId = details.GearObjectId;
            GearItemId = details.GeartItemId;
            _movement = new MovementComponent(Stats)
            {
                Position = position,

            };
            OriginZ = zPosition;
        }

        public void AddMoveNode(Vec2 node)
        {
            _path.Add(node);
        }

        public void Move(Vec2 target, float targetZ)
        {
            _movement.Move(target, targetZ);
            TargetZ = targetZ;
        }

        public void Update(float dt)
        {
            if (_movement.IsMoving)
                _movement.Tick(dt);
            else
            {
                _pathIndex++;
                _pathIndex %= _path.Count;
                Move(_path[_pathIndex], OriginZ);
                OnMovement?.Invoke(this);
            }
        }
    }
}
