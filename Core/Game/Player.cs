using Core.Game.Data;
using Core.Math;
using System.Diagnostics;
using System.Runtime.ExceptionServices;

namespace Core.Game
{
    public class Player
    { 
        public readonly int ObjectId;
        public Vec2 ClientPosition;
        public float ZPosition;
        public Vec2 CurrentTarget;
        public float ZTarget;

        public Vec2 ServerPosition;
        private Vec2 _moveDirection;
        private float _moveDistance;

        public GameCharacter CharacterDetails;


        public Player(int objectId, Vec2 position, float zPosition, GameCharacter details)
        {
            ObjectId = objectId;
            ClientPosition = position;
            ZPosition = zPosition;
            CharacterDetails = details;
            ServerPosition = new Vec2(position.x, position.y);
        }

        public void UpdateClientPosition(Vec2 newPos,  float zPosition) 
        {
            ClientPosition = newPos;
            ZPosition = zPosition;
        }

        public void Move(Vec2 target, float targetZ)
        {
            _moveDistance = Vec2.Distance(ServerPosition, target);
            _moveDirection = Vec2.Direction(ServerPosition, target);
            CurrentTarget = target;
            ZTarget = targetZ;
        }
        public Vec2 CalculatePositionOnPing(int ping)
        {
            return ServerPosition + _moveDirection * (ping  * 0.001f);
        }
        public void Update(float dt)
        {
            if (_moveDistance > 0)
            {
                var dir = _moveDirection;
                var step = dt * CharacterDetails.Stats.RunSpd;
                dir.x *= step;
                dir.y *= step;
                ServerPosition.x += dir.x;
                ServerPosition.y += dir.y;
                _moveDistance -= step;
                if (_moveDistance - 16 <= 0)
                    _moveDistance = 0;
            }
        }
    }
}
