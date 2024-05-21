using System.Net.Sockets;

namespace Core.Common.Network
{
    public interface IClientFactory<T> where T : IClient
    {
        public T BuildClient(TcpClient socket);
    }
}
