
using Core.Game.Network;

namespace Core.Game
{
    public class ActivePlayers
    {
        private Dictionary<GameClient, Player> _onlinePlayers = new();
        private List<Player> _activePlayers = new();

        public void AddPlayer(GameClient client, Player player)
        {
            _onlinePlayers[client] = player;
            _activePlayers.Add(player);
        }

        public Player GetPlayer(GameClient client)
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

        public IEnumerable<Player> GetOnlinePlayers()
        {
            return _onlinePlayers.Values;
        }

    }
}
