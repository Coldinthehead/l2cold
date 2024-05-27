using Core.Game.Network.ClientPacket;
using Core.Game.World.Components;
using Core.Utils.FSM;
using Core.Utils.Math;
using Org.BouncyCastle.Asn1.X509;


namespace Core.Game.World.Actor.PlayerStates
{


    public class PlayerMoveToPointState : PlayerBaseState, IPayloadedState<Vec2>
    {
        private readonly MovemventComponent _movement;
        private readonly IPacketBroadcaster _network;
        private readonly PlayerState _playerCharacter;

        public PlayerMoveToPointState(PlayerBehaviourFSM stateMachine) : base(stateMachine)
        {
            _movement = stateMachine.gameObject.GetComponent<MovemventComponent>();
            _network = stateMachine.gameObject.FindComponent<IPacketBroadcaster>();
            _playerCharacter = stateMachine.gameObject.GetComponent<PlayerState>();
        }
        
        public void OnEnter(Vec2 point)
        {
            StartMovement(point);
        }

        public override void MoveToPoint(Vec2 point)
        {
            StartMovement(point);
        }

        public override void Update(float dt)
        {
            if (_movement.DistanceToTarget > 0)
            {
                _movement.Translate(dt);
            }
            else
            {
                StateMachine.ChangeState<PlayerIdleState>();
            }
        }

        private void StartMovement(Vec2 point)
        {
            _network.BroadcastPacket(OutPacketFactory.BuildOutMoveToLocation(_playerCharacter, point
                , (int)StateMachine.gameObject.transform.ZPosition));
            _movement.SetTarget(point);
        }

        public override void OnExit()
        {
        }
    }
}
