using Core.Common.Network;
using Core.Game.Network;
using Core.Game.Network.ClientPacket;
using Core.Game.World;
using Core.Utils.Logs;
using Core.Utils.Math;
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
        public static int SYNC_THRESHOLD = 30;

        public void Run(GameClient client, ReadableBuffer message)
        {
            var current = new Vec2(message.ReadInt(), message.ReadInt());
            var currentZ = message.ReadInt(); 
            var heading = message.ReadInt();
            var boat = message.ReadInt();
            _logger.Log($"{heading}");

            var clientDistance = Vec2.Distance(current, client.Player.Target);
            var serverDistance = Vec2.Distance(client.Player.Origin, client.Player.Target);
            var pingComp = client.Ping * 0.001f * client.Player.Stats.RunSpd;
        /*    _logger.Log($"Curret Client dist : {clientDistance}");
            _logger.Log($"Current Server dist : {serverDistance}");*/
            _logger.Log($"Diff client - server :{clientDistance - serverDistance}");
            client.Player.UpdateClientPosition(current, currentZ);
        }
    }
}
