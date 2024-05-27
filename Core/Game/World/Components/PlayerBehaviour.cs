using Core.Engine;
using Core.Game.Network.ClientPacket;
using Core.Game.World.Actor;
using Core.Game.World.Actor.PlayerStates;
using Core.Utils.Math;

namespace Core.Game.World.Components
{

    public class PlayerBehaviour : UpdatableComponent
    {
        public ICharacter CurrentTarget { get; private set; }

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

            _stateMachine.OnActionFailed += 
                () => _network.SendPersonalPacket(OutPacketFactory.BuildActionFailed()) ;
        }

        public override void Update(float dt)
        {
            _stateMachine.Update(dt);
        }

        public void Move(Vec2 target, int targetZ)
        {
            gameObject.transform.ZPosition = targetZ;
            _stateMachine.MoveToPoint(target);
        }

        public void Attack(ICharacter target)
        {
            SetCurrentTarget(target);
            var details = new AttackDetails(target, AutoAttackLoopCallback);
            var attackDistance = 50;
            if (Vec2.Distance(gameObject.transform.Position, target.Origin) <= attackDistance)
            {
                _stateMachine.Attack(details);
            }
            else
            {
                _stateMachine.FollowTarget(new FollowTargetDetails(
                    target.gameObject
                    , attackDistance
                    , AttackOnTargetReachedCallback));
            }

            void AutoAttackLoopCallback()
            {
                Attack(target);
            }

            void AttackOnTargetReachedCallback()
            {
                _stateMachine.Attack(details);
            }
        }

        public void Follow(ICharacter target)
        {
            _stateMachine.FollowTarget(new FollowTargetDetails(target.gameObject, 50, null));
        }
        public void SelectTarget(ICharacter character)
        {
            if (!SetCurrentTarget(character))
            {
                Follow(character);
            }
        }

        private bool SetCurrentTarget(ICharacter target)
        {
            if (CurrentTarget != target)
            {
                CurrentTarget = target;
                _network.SendPersonalPacket(OutPacketFactory.BuildMyTargetSelected(CurrentTarget));
                return true;
            }
            return false;
        }
    }
}
