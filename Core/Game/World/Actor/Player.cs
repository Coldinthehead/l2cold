using Core.Game.Data;
using Core.Utils.Math;
/*
namespace Core.Game.World.Actor
{*/

  /*  public class Player : IMovable, ICharacter
    {
        public event Action<Player> OnForceStopMove;
        enum BehaviourState
        {
            idle,
            MoveToPoint,
            FollowTarget,
        }

        public int ObjectId { get; private set; }
        public Vec2 ClientPosition;
        public float OriginZ { get; private set; }
        public Vec2 Target => _movement.Target;
        public float TargetZ { get; private set; }
        public int Heading { get; private set; }
        public Vec2 Origin => _movement.Position;

        public string Title { get; private set; }
        public CharacterInfo Info { get; private set; }
        public CharacterStats Stats { get; private set; }
        public CharacterGear GearObjectId { get; private set; }
        public CharacterGear GearItemId { get; private set; }

        public bool IsMoving => _movement.IsMoving;

        public ICharacter CharacterTarget { get; set; }

        private MovementComponent _movement;
        private FollowTargetComponent _followTarget;

        private BehaviourState _currentBehaviour;
        public Player(int objectId, Vec2 position, float zPosition, GameCharacter details)
        {
            ObjectId = objectId;
            ClientPosition = position;
            OriginZ = zPosition;
            Title = details.Title;
            Info = details.Info;
            Stats = details.Stats;
            GearObjectId = details.GearObjectId;
            GearItemId = details.GearItemId;
            _movement = new MovementComponent(Stats);
            _followTarget = new FollowTargetComponent(_movement);
            _currentBehaviour = BehaviourState.idle;
            _followTarget.OnTargetReached += OnFolowTargetReached;
        }

   
        public void UpdateClientPosition(Vec2 newPos, float zPosition)
        {
            ClientPosition = newPos;
            OriginZ = zPosition;
        }

        public void Move(Vec2 target, float targetZ)
        {
            switch (_currentBehaviour)
            {
                case BehaviourState.FollowTarget:
                {
                        _followTarget.BeakState();
                        _movement.Move(target, targetZ);
                        TargetZ = targetZ;
                        Heading = CalculateHeadingFrom(Origin, target);
                        _currentBehaviour = BehaviourState.MoveToPoint;
                    }
                break;
                default:
                    {
                        _movement.Move(target, targetZ);
                        TargetZ = targetZ;
                        Heading = CalculateHeadingFrom(Origin, target);
                        _currentBehaviour = BehaviourState.MoveToPoint;
                    }

                    break;
            }
           
        }

        private int CalculateHeadingFrom(Vec2 origin, Vec2 target)
        {
            var angle = MathF.Atan2(target.y - origin.y, target.x - origin.x);
            return (int)(angle * 10430.37999999999D + 32768.0D);
        }
        public void Update(float dt)
        {
            switch (_currentBehaviour)
            {
                case BehaviourState.MoveToPoint:
                    {
                        _movement.Update(dt);

                    }
                    break;
                case BehaviourState.FollowTarget:
                    {
                        _followTarget.Update(dt);
                    }
                    break;
                default:
                    break;
            }
        }

        public void StartFollowTarget(ICharacter target, int distance)
        {
            _currentBehaviour = BehaviourState.FollowTarget;
            _followTarget.SetTarget(target, distance);
        }

        private void OnFolowTargetReached(ICharacter character)
        {
            _currentBehaviour = BehaviourState.idle;
            OnForceStopMove?.Invoke(this);
        }*/
/*    }
}*/
