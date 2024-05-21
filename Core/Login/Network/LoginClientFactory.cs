using Core.Common.Network;
using Core.Common.Security.Crypt;
using Core.Login.Security;
using System.Net.Sockets;

namespace Core.Login.Network
{
    public class LoginClientFactory : IClientFactory<LoginClient>
    {
        public LoginClient BuildClient(TcpClient socket)
        {
            var client = new LoginClient(socket, new ClientCrypt(ScrambledKeyPair.GetRandomPair()));
            return client;
        }
    }
}
