using Core.Engine;
using Core.Game.Network;

namespace Core.Game.World.Components
{
    public class PlayerNetwork : Component
    {
        private readonly GameClient _client;
        private readonly ActivePlayers _allPlayers;
        public PlayerNetwork(GameClient client, ActivePlayers players)
        {
            _client = client;
            _allPlayers = players;
        }

        public void BroadcastPacket(byte[] packet)
        {
            _allPlayers.BroadcastPacket(packet);
        }

        public void SendPersonalPacket(byte[] packet)
        {
            _client.SendData(packet);
        }
    }
}
