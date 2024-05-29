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
            StateMachine.CallOnIdle();
        }

        public override void OnExit()
        {
            Console.WriteLine("Exit Idle State");
        }
    }
}
