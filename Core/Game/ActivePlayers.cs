
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
        public void UpdatePlayers()
        {
            foreach (var player in _activePlayers)
            {
                player.Update();
            }
        }

        public Player GetPlayer(GameClient client)
        {
            return _onlinePlayers[client];
        }
    }
}
