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

      
        public void Tick(float dt)
        {
            foreach (var p in _onlinePlayers.Values)
            {
                p.Update(dt);
            }
        }

        public void AddPlayer(GameClient client, GameObject player)
        {
            _onlinePlayers[client] = player;
            _activePlayers.Add(player.GetComponent<PlayerState>());
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

        public IEnumerable<ICharacter> GetOnlinePlayers()
        {
            return _activePlayers;
        }

  /*      public void AddGhost(GhostPlayer ghostPlayer)
        {
            _activePlayers.Add(ghostPlayer);
            ghostPlayer.OnMovement += OnGhostMovement;
        }*/

        private void OnGhostMovement(IMovable character)
        {
            var packet = OutPacketFactory.BuildOutMoveToLocation(character, character.Target, (int)character.TargetZ);
            BroadcastPacket(packet);
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
    }
}
