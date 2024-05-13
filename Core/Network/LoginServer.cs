using Core.Logs;
using System.Net.Sockets;


namespace Core.Network
{
    public class LoginServer
    {
        public bool Running => true;

        private static Logger<LoginServer> _logger = Logger<LoginServer>.BuildLogger();

        private readonly TcpListener _connectionListener;
        private readonly PacketHandler _packetHandler = new();

        private readonly List<NetClient> _activeClients = new();
        private readonly List<NetClient> _toRemove = new();

        public LoginServer(TcpListener connectionListener)
        {
            _connectionListener = connectionListener;
        }

        public void Start()
        {
            _connectionListener.Start();
            _logger.Log($"Server listening at : {_connectionListener.LocalEndpoint}");
        }

        public void Stop()
        {
            _connectionListener.Stop();
        }

        public void Tick()
        {
            if (_connectionListener.Pending())
            {
                ConnectClient(_connectionListener.AcceptTcpClient());
            }
            ReadCurrentClients();
            RemoveIdleClients();
            Thread.Sleep(100);
        }

        private void ConnectClient(TcpClient client)
        {
            _logger.Log($"New connection from {client.Client.RemoteEndPoint}");
            var netClient = new NetClient(client, new ClientCrypt(ScrambledKeyPair.GetRandomPair()));
            _activeClients.Add(netClient);
            netClient.SendInit();
        }

        private void ReadCurrentClients()
        {
            foreach (var netClient in _activeClients)
            {
                try
                {
                    if (netClient.HasData())
                    {
                        _packetHandler.HandlePacket(netClient, netClient.ReceiveData());
                    }
                } 
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    _toRemove.Add(netClient);
                    netClient.ForceDisconnect();
                }
            }
        }

        private void RemoveIdleClients()
        {
            foreach (var c in _toRemove)
            {
                _activeClients.Remove(c);
            }
            _toRemove.Clear();
        }
    }
}
