using Core.Common.Network;
using Core.Game.Network;
using Core.Game.Services;
using Core.Game.World;
using Core.Utils.Math;
using Core.Utils.Logs;
using Core.Game.World.Components;
using Core.Game.Network.ClientPacket;

namespace Core.Game.Network.Controller
{
    public class CharMoveController : IPacketController
    {
        private static Logger<CharMoveController> _logger = Logger<CharMoveController>.BuildLogger();

        public void Run(GameClient client, ReadableBuffer message)
        {
            var target = new Vec2(message.ReadInt(),  message.ReadInt());
            var targetZ = message.ReadInt();
            var origin = new Vec2(message.ReadInt(), message.ReadInt());
            var originZ = message.ReadInt();
            int moveType = message.ReadInt();
            _logger.Log($"Move from {origin} , to {target}");

            var behaviour = client.Player.GetComponent<PlayerBehaviour>();
            behaviour.Move(target, targetZ);
        }


 

        
    }
}