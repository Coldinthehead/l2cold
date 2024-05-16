using Core.Game.Network;
using Core.Game.Network.ClientPacket;
using Core.Logs;
using Core.Utils.NetworkBuffers;

namespace Core.Game.Contorller
{
    internal class ProtocolVersionController : IPacketController
    {
        private static Logger<ProtocolVersionController> _logger = Logger<ProtocolVersionController>.BuildLogger();

        public void Run(GameClient client, ReadableBuffer message)
        {
            var version = message.ReadInt();
            _logger.Log($"Received [PROTOCOL_VERISON]({version}) from [{client}]");
            var clientCrypt = new GameCrypt();
            var key = clientCrypt.GetKey();
            client.SendData(OutPacketFactory.BuildCryptInit(key));
            client.SetCryptInterface(clientCrypt);
        }

       
    }
}
