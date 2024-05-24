using Core.Engine;
using Core.Utils.FSM;


namespace Core.Game.World.Actor.States
{
    public class PlayerFollowTarget : PlayerBaseState, IPayloadedState<GameObject>
    {
        public PlayerFollowTarget(StateMachine<PlayerBaseState> stateMachine) : base(stateMachine)
        {
        }

        public void OnEnter(GameObject state)
        {
            Console.WriteLine("Enter follow target state");
        }

        public override void Update(float dt)
        {
            
        }
    }
}
