using System.Net;
using System.Net.Sockets;

namespace Core.Common.Network
{
    public abstract class Server<T> where T : IClient
    {
        public event Action OnStart;
        public event Action OnStop;

        public bool IsRunning => true;
        public EndPoint LocalEndPoint => _clientListener.LocalEndpoint;

        private readonly TcpListener _clientListener;
        private readonly IClientFactory<T> _clientFactory;
        private readonly IPacketHadnler<T> _packetHandler;

        private List<T> _activeClients;
        private List<T> _clientsToRemove;

        public Server(TcpListener clientListener, IClientFactory<T> clientFactory, IPacketHadnler<T> packetHandler)
        {
            _activeClients = new();
            _clientsToRemove = new();
            _clientListener = clientListener;
            _clientFactory = clientFactory;
            _packetHandler = packetHandler;
        }

        public abstract void ClientConnected(T client);

        public void Start()
        {
            _clientListener.Start();
            OnStart?.Invoke();
        }

        public void Stop()
        {
            _clientListener.Stop();
            OnStop?.Invoke();
        }

        public void Tick()
        {
            if (_clientListener.Pending())
            {
                var socket = _clientListener.AcceptTcpClient();
                var client = _clientFactory.BuildClient(socket);
                _activeClients.Add(client);
                ClientConnected(client);
            }

            foreach (var activeClient in _activeClients)
            {
                try
                {
                    if (activeClient.HasData())
                    {
                        _packetHandler.HandlePacket(activeClient, activeClient.ReceiveData());
                    }
                }
                catch (Exception ex)
                {
                    _clientsToRemove.Add(activeClient);
                    activeClient.ForceDisconnect();
                    Console.WriteLine(ex);
                }
            }

            foreach (var c in _clientsToRemove)
            {
                _activeClients.Remove(c);
            }
            _clientsToRemove.Clear();
        }
    }
}
