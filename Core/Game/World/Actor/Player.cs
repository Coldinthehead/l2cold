using Core.Game.Data;
using Core.Math;

namespace Core.Game.World.Actor
{
    public class Player
    {
        public readonly int ObjectId;
        public Vec2 ClientPosition;
        public float ZPosition;
        public Vec2 CurrentTarget;
        public float ZTarget;

        public int ServerHeading;

        public Vec2 ServerPosition;
        private Vec2 _moveDirection;
        private float _moveDistance;
        private float _moveTimer;

        private const float ACCELERATION_TIME = 0.5f;

        public GameCharacter CharacterDetails;
        public Player(int objectId, Vec2 position, float zPosition, GameCharacter details)
        {
            ObjectId = objectId;
            ClientPosition = position;
            ZPosition = zPosition;
            CharacterDetails = details;
            ServerPosition = new Vec2(position.x, position.y);
            _moveTimer = 0;
        }

        public void UpdateClientPosition(Vec2 newPos, float zPosition)
        {
            ClientPosition = newPos;
            ZPosition = zPosition;
        }

        public void Move(Vec2 target, float targetZ)
        {
            _moveDistance = Vec2.Distance(ClientPosition, target);
            _moveDirection = Vec2.Direction(ServerPosition, target);
            CurrentTarget = target;
            ZTarget = targetZ;
            ServerHeading = CalculateHeadingFrom(ServerPosition, target);
        }

        private int CalculateHeadingFrom(Vec2 origin, Vec2 target)
        {
            var angle = MathF.Atan2(target.y - origin.y, target.x - origin.x);
            return (int)(angle * 10430.37999999999D + 32768.0D);
        }
        public Vec2 CalculatePositionOnPing(int ping)
        {
            return ServerPosition + _moveDirection * (ping * 0.001f);
        }
        public void Update(float dt)
        {
            if (_moveDistance > 0)
            {
                _moveTimer += dt;
                var dir = _moveDirection;
                var step = dt * (_moveTimer >= ACCELERATION_TIME ? CharacterDetails.Stats.RunSpd
                                                : CharacterDetails.Stats.WalkSpd);
                dir *= step;
                ServerPosition += dir;
                _moveDistance -= step;
                if (_moveDistance <= 0)
                {
                    _moveDistance = 0;
                    ServerPosition = CurrentTarget;
                    _moveTimer = 0;
                }
            }
        }
    }
}
