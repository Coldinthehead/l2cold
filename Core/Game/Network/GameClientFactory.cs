using Core.Common.Network;
using Core.Game.Network.Security;
using System.Net.Sockets;


namespace Core.Game.Network
{
    public class GameClientFactory : IClientFactory<GameClient>
    {
        public GameClient BuildClient(TcpClient socket)
        {
            var gameClient = new GameClient(socket, new NoCrypter());
            return gameClient;
        }
    }
}
