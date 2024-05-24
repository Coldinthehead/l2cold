using Core.Engine;
using Core.Game.Data;
using Core.Game.Network;
using Core.Game.World.Actor;
using Core.Game.World.Actor.States;
using Core.Game.World.Components;

namespace Core.Game.World.Factory
{
    public class PlayerFactory
    {
        private readonly ActivePlayers _activePlayers;

        public PlayerFactory(ActivePlayers activePlayers)
        {
            _activePlayers = activePlayers;
        }

        public GameObject BuildPlayer(GameClient cliemt, GameCharacterModel details)
        {
            var gameObject = new GameObject(details.Info.ObjectId);
            gameObject.transform.Position = new Utils.Math.Vec2(details.x, details.y);
            gameObject.transform.ZPosition = (float)details.z;

            var state = new PlayerState(details);
            gameObject.AddComponent(state);


            var networkComponent = new PlayerNetwork(cliemt, _activePlayers);
            gameObject.AddComponent(networkComponent);
            var behaviour = BuildPlayerBehaviour(gameObject);


            gameObject.AddComponent(behaviour);
            return gameObject;
        }

        private PlayerBehaviour BuildPlayerBehaviour(GameObject gameObject)
        {
            var fsm = new PlayerBehaviourFSM(gameObject);
            fsm.AddState(new PlayerIdleState(fsm));
            fsm.AddState(new PlayerMoveToPointState(fsm));
            var beh = new PlayerBehaviour(fsm);
            return beh;
        }
    }
}
