using Core.Game.Data;
using Core.Math;

namespace Core.Game.World.Actor
{

    public class MovementComponent
    {
        public Vec2 Position;
        public Vec2 Target => _moveTarget;

        public bool IsMoving => _moveDistance > 0;
        private CharacterStats _stats;
        private Vec2 _moveDirection;
        private Vec2 _moveTarget;
        private float _moveDistance;
        private float _moveTimer;

        public const float ACCELERATION_TIME = 0.5f;

        public MovementComponent(CharacterStats stats)
        {
            _stats = stats;
            _moveTarget = new Vec2(0, 0);
        }

        public void Move(Vec2 target, float zTarget)
        {
            _moveTarget = target;
            _moveDirection = Vec2.Direction(Position, target);
            _moveDistance = Vec2.Distance(Position, target);
        }

        public void Stop()
        {
            _moveTimer = 0;
        }
        public void Tick(float dt)
        {
            if (_moveDistance > 0)
            {
                _moveTimer += dt;
                var dir = _moveDirection;
                var step = dt * (_moveTimer >= ACCELERATION_TIME ? _stats.RunSpd
                                                : _stats.WalkSpd);
                dir *= step;
                Position += dir;
                _moveDistance -= step;
                if (_moveDistance <= 0)
                {
                    _moveDistance = 0;
                    Position = _moveTarget;
                    _moveTimer = 0;
                }
            }
        }
    }

    public class FollowTargetComponent
    {
        public event Action<ICharacter> OnTargetReached;
        private readonly MovementComponent _movement;

        public ICharacter FollowTarget;
        private int _followDistance;

        private float _currentDistance;

        public FollowTargetComponent(MovementComponent movement)
        {
            _movement = movement;
        }

        public void Update(float dt)
        {
            var dist = Vec2.Distance(_movement.Position, FollowTarget.Origin);
            if (dist - _followDistance >= 0)
            {
                _movement.Move(FollowTarget.Origin, FollowTarget.OriginZ);
                _movement.Tick(dt);
            }
            else
            {
                var dir = Vec2.Direction(_movement.Position, FollowTarget.Origin);
                var compStep = dir * (_followDistance - 10);
                var finalPos = FollowTarget.Origin - compStep;
                _movement.Position = finalPos;
                _movement.Stop();
                OnTargetReached?.Invoke(FollowTarget);
            }
        }

        public void BeakState()
        {
            OnTargetReached?.Invoke(FollowTarget);
        }

        public void SetTarget(ICharacter target, int distance)
        {
            FollowTarget = target;
            _followDistance = distance;
            _currentDistance = Vec2.Distance(_movement.Position, target.Origin);
        }
    }

    public class Player : IMovable, ICharacter
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
            GearItemId = details.GeartItemId;
            _movement = new MovementComponent(Stats)
            {
                Position = position,

            };
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
                        _movement.Tick(dt);

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
        }
    }
}
