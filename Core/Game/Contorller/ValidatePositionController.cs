using Core.Game.Network;
using Core.Game.Network.ClientPacket;
using Core.Logs;
using Core.Math;
using Core.Utils.NetworkBuffers;
using System.Numerics;

namespace Core.Game.Contorller
{
    public class ValidatePositionController : IPacketController
    {
        private static Logger<ValidatePositionController> _logger = Logger<ValidatePositionController>.BuildLogger();
        private readonly ActivePlayers _activePlayers;

        public ValidatePositionController(ActivePlayers activePlayers)
        {
            _activePlayers = activePlayers;
        }

        public void Run(GameClient client, ReadableBuffer message)
        {
            var current = new Vec2(message.ReadInt(), message.ReadInt());
            var currentZ = message.ReadInt(); 
            var heading = message.ReadInt();
            var boat = message.ReadInt();

            client.Player.UpdatePosition(current, currentZ);
            var clientDistance = Vec2.Distance(current, client.Player.CurrentTarget);
            var serverDistance = Vec2.Distance(client.Player.ServerPosition, client.Player.CurrentTarget);
            _logger.Log($"client :{current}");
            _logger.Log($"server : {client.Player.ServerPosition}");
            _logger.Log($"Curret Client dist : {clientDistance}");
            _logger.Log($"Current Server dist : {serverDistance}");
            _logger.Log($"Diff client - server :{clientDistance - serverDistance}");

           /* var player = client.Player;
            _activePlayers.BroadcastMoveToLocation(player, player.CurrentTarget, player.ZTarget);*/
           
        }
    }
}
