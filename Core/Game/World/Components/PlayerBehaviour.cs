using Core.Engine;
using Core.Game.Network.ClientPacket;
using Core.Game.World.Actor;
using Core.Game.World.Actor.States;
using Core.Utils.Math;


namespace Core.Game.World.Components
{

    public class PlayerBehaviour : UpdatableComponent
    {
        private readonly PlayerBehaviourFSM _stateMachine;
        private PlayerNetwork _network;
        private PlayerState _state;

        public PlayerBehaviour(PlayerBehaviourFSM stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public override void Awake()
        {
            _stateMachine.ChangeState<PlayerIdleState>();
            _network = gameObject.GetComponent<PlayerNetwork>();
            _state = gameObject.GetComponent<PlayerState>();
        }

        public override void Update(float dt)
        {
            _stateMachine.Update(dt);
        }

        public void Move(Vec2 target, int targetZ)
        {
            gameObject.transform.ZPosition = targetZ;
            _stateMachine.ChangeState<PlayerMoveToPointState, Vec2>(target);
            _network.BroadcastPacket(OutPacketFactory.BuildOutMoveToLocation(_state, target, targetZ));
        }

        public void Follow(ICharacter target)
        {

        }
    }
}
