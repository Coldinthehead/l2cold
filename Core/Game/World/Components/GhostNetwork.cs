using Core.Engine;

namespace Core.Game.World.Components
{
    public class GhostNetwork : Component, IPacketBroadcaster
    {
        private readonly ActivePlayers _activePlayers;

        public GhostNetwork(ActivePlayers activePlayers)
        {
            _activePlayers = activePlayers;
        }

        public void BroadcastPacket(byte[] packet)
        {
            _activePlayers.BroadcastPacket(packet);
        }
    }
}
