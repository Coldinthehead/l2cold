using Core.Common.Network;
using Core.Game.Network;
using Core.Game.Network.ClientPacket;
using Core.Game.World;
using Core.Game.World.Components;
using Core.Utils.Logs;
using Core.Utils.Math;

namespace Core.Game.Contorller
{
    public class ActionController : IPacketController
    {
        private static readonly Logger<ActionController> _logger = Logger<ActionController>.BuildLogger();
        private readonly ActivePlayers _worldCharacters;

        public ActionController(ActivePlayers worldCharacters)
        {
            _worldCharacters = worldCharacters;
        }

        public void Run(GameClient client, ReadableBuffer message)
        {
            int objectId = message.ReadInt();
            var origin = new Vec2(message.ReadInt(), message.ReadInt());
            var originZ = message.ReadInt();
            int actionId = message.ReadByte();
            var character = _worldCharacters.FindById(objectId);
            _logger.Log($"Action id {actionId}");
            if (character != null)
            {
               client.Player.GetComponent<PlayerBehaviour>().SelectTarget(character);
            }
            else
                client.SendData(OutPacketFactory.BuildActionFailed());
        }
    }
}
