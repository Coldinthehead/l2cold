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
            client.SendData(BuildCryptInit(key));
            client.SetCryptInterface(clientCrypt);
        }

        private static byte[] BuildCryptInit(byte[] key)
        {
            var cryptInit = new WriteableBuffer();
            cryptInit.WriteByte(OutPacket.CRYPT_INIT)
                .WriteByte(1); // 0 protocol missmatch 1 good
            for (int i = 0; i < 8; i++)
                cryptInit.WriteByte(key[i]);
            cryptInit.WriteInt(1) // use encryption
                .WriteInt(1) // server id??/
                .WriteByte(1); // unknown

            return cryptInit.toByteArray();
        }
    }
}
