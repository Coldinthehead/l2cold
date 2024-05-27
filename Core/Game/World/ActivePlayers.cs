using Core.Engine;
using Core.Game.Network;
using Core.Game.Network.ClientPacket;
using Core.Game.World.Actor;
using Core.Game.World.Components;

namespace Core.Game.World
{
    public class ActivePlayers
    {
        private Dictionary<GameClient, GameObject> _onlinePlayers = new();
        private List<ICharacter> _activePlayers = new();

        private readonly List<GameObject> _simulatedObjets = new();

      
        public void Tick(float dt)
        {
            foreach (var p in _simulatedObjets)
            {
                p.Update(dt);
            }
        }

        public void AddPlayer(GameClient client, GameObject player)
        {
            _onlinePlayers[client] = player;
            _activePlayers.Add(player.GetComponent<PlayerState>());
            _simulatedObjets.Add(player);
        }

        public GameObject GetPlayer(GameClient client)
        {
            return _onlinePlayers[client];
        }

        public void BroadcastPacket(byte[] packet)
        {
            foreach (var client in _onlinePlayers.Keys)
            {
                client.SendData(packet);
            }
        }

        public IEnumerable<ICharacter> GetAllCharacters()
        {
            return _activePlayers;
        }

        public ICharacter FindById(int objectId)
        {
            foreach (var character in _activePlayers)
            {
                if (character.ObjectId == objectId)
                {
                    return character;
                }
            }
            return null;
        }

        internal void AddGhost(GameObject ghost)
        {
            _simulatedObjets.Add(ghost);
            _activePlayers.Add(ghost.GetComponent<PlayerState>());
        }
    }
}
