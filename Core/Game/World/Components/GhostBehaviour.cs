using Core.Engine;
using Core.Game.World.Actor;
using Core.Game.World.Actor.PlayerStates;
using Core.Utils.Math;

namespace Core.Game.World.Components
{
    public class GhostBehaviour : UpdatableComponent
    {
        private readonly PlayerBehaviourFSM _stateMachine;
        private MovemventComponent _movement;
        private GhostNetwork _network;
        private PlayerState _state;

        private List<Vec2> _movePoints;
        private int _current;

        public GhostBehaviour(PlayerBehaviourFSM stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void SetMovePoint(List<Vec2> points)
        {
            _movePoints = points;
            _current = 0;
        }

        public override void OnStart()
        {
            _movement = gameObject.GetComponent<MovemventComponent>();
            _network = gameObject.GetComponent<GhostNetwork>();
            _state = gameObject.GetComponent<PlayerState>();
        }

        public override void Update(float dt)
        {
            Think(dt);
            _stateMachine.Update(dt);
        }

        private void Think(float dt)
        {
            if (_movement.DistanceToTarget <= 0)
            {
                _current++;
                _current %= _movePoints.Count;
                Move(_movePoints[_current]);
            }
        }

        public void Move(Vec2 point)
        {
            _stateMachine.ChangeState<PlayerMoveToPointState, Vec2>(point);
        }


    }
}
