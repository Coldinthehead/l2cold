using Core.Common.Network;
using Core.Common.Services;
using Core.Game.Network;
using Core.Utils.Logs;
using System.Net.Sockets;


namespace Core.Game
{
    public partial class GameServer : Server<GameClient>
    {
        private static Logger<GameServer> _logger = Logger<GameServer>.BuildLogger();

        public GameServer(TcpListener tcpListener
            , LoginServerService loginServer
            , IClientFactory<GameClient> clientFactory
            , IPacketHadnler<GameClient> packetHandler)
            : base(tcpListener, clientFactory, packetHandler)
        {
        }

        public override void ClientConnected(GameClient client)
        {
            _logger.Log("Client conneted : ", client);
        }
    }
}
