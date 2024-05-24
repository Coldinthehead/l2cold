using Core.Utils.FSM;


namespace Core.Game.World.Actor.States
{

    public class PlayerIdleState : PlayerBaseState, IState
    {
        public PlayerIdleState(StateMachine<PlayerBaseState> stateMachine) : base(stateMachine)
        {
        }

        public void OnEnter()
        {
            Console.WriteLine("Entering Idle State");
        }

        public override void OnExit()
        {
            Console.WriteLine("Exit Idle State");
        }
    }
}
