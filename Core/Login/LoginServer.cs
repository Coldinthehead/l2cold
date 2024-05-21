using Core.Common.Network;
using Core.Login.Network;
using Core.Utils.Logs;
using System.Net.Sockets;


namespace Core.Login
{
    public class LoginServer : Server<LoginClient>
    {
        private static Logger<LoginServer> _logger = Logger<LoginServer>.BuildLogger();

        public LoginServer(TcpListener connectionListener, IClientFactory<LoginClient> clientFactory, IPacketHadnler<LoginClient> packetHandler) 
            : base(connectionListener, clientFactory, packetHandler)
        {
        }

        public override void ClientConnected(LoginClient client)
        {
            _logger.Log($"New connection from {client.RemoteEndPoint}");
            client.SendInit();
        }
    }
}
