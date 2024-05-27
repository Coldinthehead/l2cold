using Core.Utils.FSM;
using Core.Utils.Math;


namespace Core.Game.World.Actor.PlayerStates
{
    public abstract class LockedState : PlayerBaseState, IExitableState
    {
        protected LockedState(PlayerBehaviourFSM stateMachine) : base(stateMachine)
        {
        }

        public override void MoveToPoint(Vec2 point)
        {
            StateMachine.CallActionFailed();
        }

        public override void FollowTarget(FollowTargetDetails details)
        {
            StateMachine.CallActionFailed();
        }

        public override void Attack(AttackDetails details)
        {
            StateMachine.CallActionFailed();
        }
    }


    public abstract class PlayerBaseState : IExitableState
    {
        protected readonly PlayerBehaviourFSM StateMachine;

        protected PlayerBaseState(PlayerBehaviourFSM stateMachine)
        {
            StateMachine = stateMachine;     
        }

        public virtual void MoveToPoint(Vec2 point)
        {
            StateMachine.ChangeState<PlayerMoveToPointState, Vec2>(point);
        }

        public virtual void FollowTarget(FollowTargetDetails details)
        {
            StateMachine.ChangeState<PlayerFollowTarget, FollowTargetDetails>(details);
        }

        public virtual void Attack(AttackDetails details)
        {
            StateMachine.ChangeState<PlayerAttackState, AttackDetails>(details);
        }

        public virtual void Update(float dt)
        {

        }

        public virtual void OnExit()
        {

        }
    }
}
