using Core.Game.Network.ClientPacket;
using Core.Game.World.Components;
using Core.Utils.FSM;


namespace Core.Game.World.Actor.PlayerStates
{
    public class AttackDetails
    {
        public ICharacter Target;
        public Action Callback;

        public AttackDetails(ICharacter target, Action callback)
        {
            Target = target;
            Callback = callback;
        }
    }

    public class PostAttackDetails
    {
        public float WaitDuration;
        public Action WaitFinishCallback;

        public PostAttackDetails(float waitDuration, Action callback)
        {
            WaitDuration = waitDuration;
            WaitFinishCallback = callback;
        }
    }

    public class PostAttackState : PlayerBaseState, IPayloadedState<PostAttackDetails>
    {

        private PostAttackDetails _details;
        private float _waitTimer;
        public PostAttackState(PlayerBehaviourFSM stateMachine) : base(stateMachine)
        {
        }

        public void OnEnter(PostAttackDetails details)
        {
            _details = details;
            _waitTimer = 0;
        }
        public override void Attack(AttackDetails details)
        {
            StateMachine.CallActionFailed();
        }

        public override void Update(float dt)
        {
            Console.WriteLine("Post attack");
            _waitTimer += dt;
            if (_waitTimer >= _details.WaitDuration)
            {
                _details.WaitFinishCallback();
            }
        }
    }

    public class PlayerAttackState : PlayerBaseState, IPayloadedState<AttackDetails>
    {
        private readonly IPacketBroadcaster _packetBroadcaster;
        private readonly PlayerState _state;

        private float _attackDelay;
        private float _attackTimer;
        private AttackDetails _details;
        public PlayerAttackState(PlayerBehaviourFSM stateMachine) : base(stateMachine)
        {
            _packetBroadcaster = stateMachine.gameObject.FindComponent<IPacketBroadcaster>();
            _state = stateMachine.gameObject.GetComponent<PlayerState>();
            _attackDelay = 1f;
        }

        public void OnEnter(AttackDetails attackDetails)
        {
            _details = attackDetails;
            _attackTimer = 0;
            var startAttack = OutPacketFactory.BuildAutoAttackStart(_state);
            _packetBroadcaster.BroadcastPacket(startAttack);

            var attackResut = OutPacketFactory.BuildAttackResult(_state, attackDetails.Target, 0);
            _packetBroadcaster.BroadcastPacket(attackResut);
            StateMachine.CallAttackStarted(attackDetails);
        }

        public override void Attack(AttackDetails details)
        {
            if (_details!= null && details.Target == _details.Target && _attackTimer > 0)
            {
                StateMachine.CallActionFailed();
                return;
            }
            base.Attack(details);
        }

        public override void Update(float dt)
        {
            Console.WriteLine("Attack");
            _attackTimer += dt;
            if (_attackTimer >= _attackDelay / 2)
            {
                var postAttackDetails = new PostAttackDetails(_attackDelay / 2, FinishAttack);
                StateMachine.ChangeState<PostAttackState, PostAttackDetails>(postAttackDetails);
            }
        }

        private void FinishAttack()
        {
            StateMachine.CallAttackFinished();
            var finishAttackPacket = OutPacketFactory.BuildAutoAttackFinish(_state);
            _packetBroadcaster.BroadcastPacket(finishAttackPacket);
            StateMachine.ChangeState<PlayerIdleState>();
            _attackTimer = 0;
            if (_details.Callback!= null)
            {
                _details.Callback();
            }
            else
            {
                StateMachine.ChangeState<PlayerIdleState>();
            }
        }
    }
}
