using Core.Utils.FSM;


namespace Core.Game.World.Actor.PlayerStates
{

    public class PlayerIdleState : PlayerBaseState, IState
    {
        public PlayerIdleState(PlayerBehaviourFSM stateMachine) : base(stateMachine)
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
