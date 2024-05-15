using Core.Common.Services;
using Core.Game.Contorller;
using Core.Game.Network;
using Core.Game.Network.ClientPacket;
using Core.Logs;
using Core.Utils;
using System.Net.Sockets;


namespace Core.Game
{
    public partial class GameServer
    {
        private static Logger<GameServer> _logger = Logger<GameServer>.BuildLogger();
        private readonly TcpListener _connectionListener;
        private readonly LoginServerService _loginServer;
        private List<GameClient> _activeClients = new();
        private List<GameClient> _toRemove = new();

        private readonly ActivePlayers _worldPlayers = new();

        public GameServer(TcpListener tcpListener, LoginServerService loginServer)
        {
            _connectionListener = tcpListener;
            _loginServer = loginServer;
        }

        public bool Runing => true;

        public void Start()
        {
            _connectionListener.Start();
            _logger.Log($"Listening on : {_connectionListener.LocalEndpoint}");
        }

        public void Stop() => _connectionListener.Stop();

        public void Tick()
        {
            ConnectPendingClients();
            ReadActiveClients();
            RemoveInactiveClients();
            _worldPlayers.UpdatePlayers();
        }

        private void ConnectPendingClients()
        {
            if (_connectionListener.Pending())
            {
                var gameClient = new GameClient(_connectionListener.AcceptTcpClient(), new NoCrypter());
                _activeClients.Add(gameClient);
            }
        }

        private void ReadActiveClients()
        {
            foreach (var gameClient in _activeClients)
            {
                try
                {
                    if (gameClient.HasData())
                    {
                        ReadSingleClient(gameClient);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Log($"Error from [{gameClient}] : {ex}");
                    _toRemove.Add(gameClient);
                    gameClient.ForceDisonnect();
                }
            }
        }

        private void RemoveInactiveClients()
        {
            foreach (var c in _toRemove)
            {
                _activeClients.Remove(c);
                _logger.Log($"Client removed from active clients : {c}");
            }
            _toRemove.Clear();
        }

        private void ReadSingleClient(GameClient client)
        {
            _logger.Log($"Recieveing data from [{client}]");
            var buffer = client.ReceiveRawData();
            int opCode = buffer.ReadByte();
            _logger.Log($"Received [{opCode.ToHex()}]");
            switch (opCode)
            {
                case InPacket.PROTOCOL_VERISION:
                        new ProtocolVersionController().Run(client, buffer);
                    break;
                case InPacket.REQUEST_AUTHENTICATION:
                        new RequestAuthController(_loginServer).Run(client, buffer);
                    break;

                case InPacket.CHARACTER_SELECTED:
                        new CharacterSelectedController().Run(client, buffer);
                    break;
                case InPacket.EX_PACKET:
                        _logger.Log($"[EX_PACKET] received from :", client);
                    break;
                case InPacket.ENTER_WORLD:
                        new EnterWorldController(_worldPlayers).Run(client, buffer);
                    break;
                case InPacket.CHARACTER_MOVE_TO_LOCATION:
                        new CharMoveController(_worldPlayers).Run(client, buffer);
                    break;
                default:
                        _logger.Log($"Unknown opcode [{opCode.ToHex()}] from [{client}]");
                    break;
            }
        }
    }
}
