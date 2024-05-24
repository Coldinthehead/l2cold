using Core.Engine;
using Core.Game.World.Actor.States;
using Core.Utils.FSM;


namespace Core.Game.World.Actor
{
    public class PlayerBehaviourFSM : StateMachine<PlayerBaseState>
    {
        public readonly GameObject gameObject;

        public PlayerBehaviourFSM(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public void Update(float dt)
        {
            CurrentState.Update(dt);
        }
    }
}
