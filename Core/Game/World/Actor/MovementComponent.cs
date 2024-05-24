using Core.Engine;
using Core.Game.Data;
using Core.Utils.Math;

namespace Core.Game.World.Actor
{
    public class MovementComponent : UpdatableComponent
    {
        public Vec2 Position => gameObject.transform.Position;
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
        public override void Update(float dt)
        {
            if (_moveDistance > 0)
            {
                _moveTimer += dt;
                var dir = _moveDirection;
                var step = dt * (_moveTimer >= ACCELERATION_TIME ? _stats.RunSpd
                                                : _stats.WalkSpd);
                dir *= step;
                gameObject.transform.Position += dir;
                _moveDistance -= step;
                if (_moveDistance <= 0)
                {
                    _moveDistance = 0;
                    gameObject.transform.Position = _moveTarget;
                    _moveTimer = 0;
                }
            }
        }
    }
}
