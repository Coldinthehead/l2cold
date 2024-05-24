using Core.Utils.FSM;


namespace Core.Game.World.Actor.States
{
    public abstract class PlayerBaseState : IExitableState
    {
        protected readonly StateMachine<PlayerBaseState> StateMachine;

        protected PlayerBaseState(StateMachine<PlayerBaseState> stateMachine)
        {
            StateMachine = stateMachine;
        }

        public virtual void Update(float dt)
        {

        }

        public virtual void OnExit()
        {

        }
    }
}
