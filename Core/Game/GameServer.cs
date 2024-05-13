using Core.Game.Network;
using Core.Logs;
using Core.Utils;
using Core.Utils.NetworkBuffers;
using System;
using System.Net.Sockets;


namespace Core.Game
{
    public class GameServer
    {
        private static Logger<GameServer> _logger = Logger<GameServer>.BuildLogger();
        private readonly TcpListener _connectionListener;
        private List<GameClient> _activeClients = new();
        private List<GameClient> _toRemove = new();

        public GameServer(TcpListener tcpListener)
        {
            _connectionListener = tcpListener;
        }

        public bool Runing => true;

        public void Start()
        {
            _connectionListener.Start();
            _logger.Log($"Listening on : {_connectionListener.LocalEndpoint}");
        }

        public void Stop()
        {
            _connectionListener.Stop();
        }

        public void Tick()
        {
            if(_connectionListener.Pending())
            {
                ConnectClient(_connectionListener.AcceptTcpClient());
            }
            ReadActiveClients();
            RemoveInactiveClients();
        }

        private void ConnectClient(TcpClient client)
        {
            var gameClient = new GameClient(client, new NoCrypter());
            _activeClients.Add(gameClient);
            _logger.Log($"Get connection from : [{client}]");
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
            var rawData = client.ReceiveRawData();
            ReadableBuffer buffer = new ReadableBuffer(rawData);
            int opCode = buffer.ReadByte();
            switch (opCode)
            {
                case 0:
                    {
                        int version = buffer.ReadInt(); 
                        _logger.Log($"Received [PROTOCOL_VERISON]({version}) from [{client}]");
                        var clientCrypt = new GameCrypt();
                        var key = clientCrypt.GetKey();

                        var cryptInit = new WriteableBuffer();
                        cryptInit.WriteByte(0x00)
                            .WriteByte(1); // 0 protocol missmatch 1 good
                        for (int i = 0; i < 8; i++)
                            cryptInit.WriteByte(key[i]);
                        cryptInit.WriteInt(1) // use encryption
                            .WriteInt(1) // server id??/
                            .WriteByte(1); // unknown

                        client.SendData(cryptInit.toByteArray());
                        client.SetCryptInterface(clientCrypt);
                    }
                    break;
                case 0x8:
                    {
                        _logger.Log($"Received [REQUEST_AUTH] from [{client}]");
                        //swapped on purpose. indian byte positions
                        string accId = buffer.ReadString();
                        int playKey2 = buffer.ReadInt();
                        int playKey1 = buffer.ReadInt();
                        int login1 = buffer.ReadInt();
                        int login2 = buffer.ReadInt();

                        _logger.Log($"accid:[{accId}] {playKey1} == {0}|{playKey2} == {0}|{login1} == {0}|{login2} == {0}");

                        
                    }
                    break;
                default:
                    {
                        _logger.Log($"Unknown opcode [{opCode}] from [{client}]");
                    }
                    break;
            }
        }
    }
}
