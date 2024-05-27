using Core.Engine;
using Core.Game.World.Actor.PlayerStates;
using Core.Utils.FSM;
using Core.Utils.Math;

namespace Core.Game.World.Actor
{
    public class PlayerBehaviourFSM : StateMachine<PlayerBaseState>
    {
        public event Action OnActionFailed;
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
    }
}
