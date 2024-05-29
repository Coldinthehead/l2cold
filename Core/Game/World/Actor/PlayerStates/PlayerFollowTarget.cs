using Core.Engine;
using Core.Game.Network.ClientPacket;
using Core.Game.World.Components;
using Core.Utils.FSM;


namespace Core.Game.World.Actor.PlayerStates
{
    public class FollowTargetDetails
    {
        public GameObject Target;
        public float Distance;
        public Action Callback;

        public FollowTargetDetails(GameObject target, float distance, Action callback)
        {
            Target = target;
            Distance = distance;
            Callback = callback;
        }
    }

    public class PlayerFollowTarget : PlayerBaseState, IPayloadedState<FollowTargetDetails>
    {
        private readonly MovemventComponent _movement;
        private readonly IPacketBroadcaster _packetBroadcaster;
        private readonly PlayerState _state;
        
        private FollowTargetDetails _details;
        public PlayerFollowTarget(PlayerBehaviourFSM stateMachine) : base(stateMachine)
        {
            _movement = stateMachine.gameObject.GetComponent<MovemventComponent>();
            _state = stateMachine.gameObject.GetComponent<PlayerState>();
            _packetBroadcaster = stateMachine.gameObject.FindComponent<IPacketBroadcaster>();
        }

        public void OnEnter(FollowTargetDetails details)
        {
            _details = details;
            var target = _details.Target.GetComponent<PlayerState>();
            StateMachine.CallStartFollowTarget(details);
            _packetBroadcaster.BroadcastPacket(OutPacketFactory
                .BuildMoveToPawn(_state, target, (int)_details.Distance));
        }

        public override void Update(float dt)
        {
            _movement.SetTarget(_details.Target.transform.Position);
            if (_movement.DistanceToTarget >= _details.Distance)
            {
                _movement.Translate(dt);
            }
            else
            {
                _movement.ForceStop();
                if (_details.Callback != null)
                {
                    _details.Callback();
                    StateMachine.CallTargetReached(_details);
                }
            }
        }

        public override void OnExit()
        {
            StateMachine.CallFollowStopped();
            _packetBroadcaster.BroadcastPacket(OutPacketFactory.BuildStopMove(_state));

        }
    }
}
