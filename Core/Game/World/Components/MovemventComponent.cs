using Core.Engine;
using Core.Utils.Math;

namespace Core.Game.World.Components
{

    public class MovemventComponent : Component
    {
        public Vec2 Origin => _transform.Position;
        public Vec2 Target { get; private set; }
        public float DistanceToTarget => _moveDistance;

        public float ZTarget { get; private set; }
        private Transform _transform;
        private Vec2 _targetPoint;
        private float _moveDistance;
        private Vec2 _direction;
        private float _moveTimer;
        private PlayerState _state;

        private float _walkDistance;

        public override void OnStart()
        {
            _transform = gameObject.transform;
            _state = gameObject.GetComponent<PlayerState>();
        }

        public void SetTarget(Vec2 target)
        {
            _targetPoint = target;
            _moveDistance = Vec2.Distance(_transform.Position, target);
            _direction = Vec2.Direction(_transform.Position, target);
            ZTarget = _transform.ZPosition;
            Target = target;

            if (_moveTimer == 0)
            {
                _walkDistance = _state.Stats.WalkSpd / 2.4f;
            }
        }

        public void Translate(float dt)
        {
            if (_walkDistance > 0)
            {
                var speed = _state.Stats.WalkSpd * dt;
                var step = _direction * speed;
                _transform.Position += step;
                _walkDistance -= speed;
                _moveDistance -= speed;
            }
            else if (_moveDistance > 0) 
            {
                var speed = _state.Stats.RunSpd * dt;
                var step = _direction * speed;
                _transform.Position += step;
                _moveDistance -= speed;
            }
             _moveTimer += dt;

            if (_moveDistance <= 0)
            {
                _transform.Position = _targetPoint;
                _moveTimer = 0;
                _moveDistance = 0;
            }
        }

        internal void ForceStop()
        {
            _moveTimer = 0;
        }
    }
}
