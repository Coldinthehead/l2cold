using Core.Engine;
using Core.Game.World.Actor.PlayerStates;
using Core.Utils.FSM;
using Core.Utils.Math;

namespace Core.Game.World.Actor
{
    public class PlayerBehaviourFSM : StateMachine<PlayerBaseState>
    {
        public event Action OnActionFailed;
        public event Action<Vec2> OnMoveToPoint;
        public event Action OnIdle;
        public event Action<FollowTargetDetails> OnFollowStarted;
        public event Action OnFollowStop;
        public event Action<FollowTargetDetails> OnTargetReached;
        public event Action<AttackDetails> OnAttackStarted;
        public event Action OnAttackFinished;
        public readonly GameObject gameObject;

        public PlayerBehaviourFSM(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public void Update(float dt)
        {
            CurrentState.Update(dt);
        }
        public void MoveToPoint(Vec2 point)
        {
            CurrentState.MoveToPoint(point);
        }

        public void FollowTarget(FollowTargetDetails details)
        {
            CurrentState.FollowTarget(details);
        }

        public void Attack(AttackDetails details) 
        {
            CurrentState.Attack(details);
        }

        public void CallActionFailed()
        {
            Console.WriteLine("FSM action failed");
            OnActionFailed?.Invoke();
        }

        public void CallMoveToPoint(Vec2 target)
        {
            OnMoveToPoint?.Invoke(target);
        }

        public void CallOnIdle()
        {
            OnIdle?.Invoke();
        }

        public void CallStartFollowTarget(FollowTargetDetails details)
        {
            OnFollowStarted?.Invoke(details);
        }

        public void CallFollowStopped()
        {
            OnFollowStop?.Invoke();
        }

        public void CallTargetReached(FollowTargetDetails details)
        {
            OnTargetReached?.Invoke(details);
        }

        internal void CallAttackStarted(AttackDetails attackDetails)
        {
            OnAttackStarted?.Invoke(attackDetails);
        }

        internal void CallAttackFinished()
        {
            OnAttackFinished?.Invoke();
        }
    }
}
