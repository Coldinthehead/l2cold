using Core.Engine;
using Core.Game.Data;
using Core.Game.World.Components;
using Core.Utils.FSM;
using Core.Utils.Math;


namespace Core.Game.World.Actor.States
{

    public class PlayerMoveToPointState : PlayerBaseState, IPayloadedState<Vec2>
    {
        public const float ACCELERATION_TIME = 0.5f;
        private readonly Transform _transform;
        private readonly CharacterStats _stats;
        private Vec2 _targetPoint;
        private float _moveDistance;
        private Vec2 _direction;
        private float _moveTimer; 
        public PlayerMoveToPointState(PlayerBehaviourFSM stateMachine) : base(stateMachine)
        {
            _transform = stateMachine.gameObject.transform;
            _stats = stateMachine.gameObject.GetComponent<PlayerState>().Stats;
            _moveTimer = 0;
        }

        public void OnEnter(Vec2 point)
        {
            _targetPoint = point;
            _moveDistance = Vec2.Distance(_transform.Position, point);
            _direction = Vec2.Direction(_transform.Position, point);
        }

        public override void Update(float dt)
        {
            if (_moveDistance > 0)
            {
                Translate(dt);
                _moveTimer += dt;
            }
            else
            {
                StateMachine.ChangeState<PlayerIdleState>();
            }
        }

        private void Translate(float dt)
        {
            var speed = _moveTimer >= ACCELERATION_TIME ? _stats.RunSpd : _stats.WalkSpd;
            var finalSpeed = speed * dt;
            var step = _direction * finalSpeed;
            _transform.Position += step;
            _moveDistance -= finalSpeed;
            if (_moveDistance <= 0)
            {
                _transform.Position = _targetPoint;
                _moveTimer = 0;
            }
        }

        public override void OnExit()
        {
            Console.WriteLine("Exit Move to point state");
        }
    }
}
