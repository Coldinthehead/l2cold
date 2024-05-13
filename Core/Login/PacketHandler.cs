using Core.Logs;
using Core.Security;
using Core.Utils;
using Core.Utils.NetworkBuffers;
using System.Text;

namespace Core.Login
{

    enum AuthenticationResult
    {
        InvalidLogin,
        InvalidPassword,
        Succes,
        Banned,

    }

    public class PacketHandler
    {
        private readonly Logger<PacketHandler> _logger = Logger<PacketHandler>.BuildLogger();

        public void HandlePacket(NetClient client, ReadableBuffer buffer)
        {
            int opCode = buffer.ReadByte();
            switch (opCode)
            {
                case 0x07:
                    {
                        _logger.Log($"Received [REQUEST_AUTH_GG] from <{client}>");
                        int sessionId = buffer.ReadInt();
                        _logger.Log($"in id : {sessionId} vs {client.SessionId}");

                        //gg authenticated
                        WriteableBuffer outPacket = new WriteableBuffer();
                        outPacket.WriteByte(0x0b)
                            .WriteInt(sessionId)
                            .WriteInt(0).WriteInt(0).WriteInt(0).WriteInt(0);

                        client.Send(outPacket.toByteArray());
                    }
                    break;
                case 0x00:
                    {
                        _logger.Log($"Received [REQUEST_AUTH_LOGIN] from <{client}>");
                        byte[] rawData = new byte[128];
                        buffer.readBytesInPlace(rawData);
                        var dec = client.DecryptWithRsa(rawData);

                        byte[] id = ExtractId(dec);
                        byte[] pw = ExtractPw(dec);
                        dec.ToConsole();
                        _logger.Log($"id : {ASCIIEncoding.ASCII.GetString(id)}");
                        _logger.Log($"pw : {ASCIIEncoding.ASCII.GetString(pw)}");

                        var authResult = ProcessAuthentication(id, pw);
                        switch (authResult)
                        {
                            case AuthenticationResult.Succes:
                                {
                                    client.SetSessionKeys(SessionKeys.GenerateSessionKeys());

                                    WriteableBuffer loginOkPacket = new WriteableBuffer();
                                    loginOkPacket.WriteByte(0x03)
                                         .WriteInt(client.SKeys.Login1)
                                         .WriteInt(client.SKeys.Login2)
                                         .WriteInt(0)
                                         .WriteInt(0)
                                         .WriteInt(0x000003ea)
                                         .WriteInt(0)
                                         .WriteInt(0)
                                         .WriteInt(0)
                                         .WriteBytes(new byte[16]);

                                    client.Send(loginOkPacket.toByteArray());
                                }
                                break;

                            default:
                                break;
                        }
                    }
                    break;
                case 0x05:
                    {
                        _logger.Log($"Received [REQUEST_SERVER_LIST] from <{client}>");
                        int login1 = buffer.ReadInt();
                        int login2 = buffer.ReadInt();
                        _logger.Log($"{login1} == {client.SKeys.Login1} && {login2} == {client.SKeys.Login2}");
                        var serverList = BuildServerList();
                        client.Send(serverList.toByteArray());
                    }
                    break;
                case 0x02:
                    {
                        _logger.Log($"Received [REQUEST_SERVER_LOGIN] from <{client}>");
                        int login1 = buffer.ReadInt();
                        int login2 = buffer.ReadInt();
                        _logger.Log($"{login1} == {client.SKeys.Login1} && {login2} == {client.SKeys.Login2}");
                        int serverId = buffer.ReadByte();
                        _logger.Log($"Server id : " + serverId);

                        var playOk = new WriteableBuffer();
                        playOk.WriteByte(0x07)
                            .WriteInt(client.SKeys.Play1)
                            .WriteInt(client.SKeys.Play2);

                        client.Send(playOk.toByteArray());
                    }
                    break;
                default:
                    {
                        _logger.Log($"Unknown opcode [{opCode}] from : <{client}>");
                    }
                    break;
            }
        }

        struct GameServerData
        {
            public int[] IpAddress;
            public int Port;
            public int AgeLimit;
            public bool IsPvp;
            public int CurrentOnline;
            public int MaxPlayers;
            public bool Brackets;
            public int Status;
            public int ID;
            public int ServerType;

            public GameServerData(int[] ipAddress, int port, int currentOnline, int id)
            {
                IpAddress = ipAddress;
                Port = port;
                AgeLimit = 0;
                CurrentOnline = currentOnline;
                MaxPlayers = 1000;
                Brackets = false;
                Status = 1;
                ID = id;
            }
        }

        private WriteableBuffer BuildServerList()
        {
            List<GameServerData> servers = new();
            servers.Add(new GameServerData(new int[] { 127, 0, 0, 1 }, 7777, 0, 1));
            WriteableBuffer buffer = new WriteableBuffer();
            buffer.WriteByte(0x04)
                .WriteByte(servers.Count) // Servers count;
                .WriteByte(1); // Last server id
            foreach( var server in servers ) 
            {
                buffer.WriteByte(server.ID)

                .WriteByte(server.IpAddress[0])
                .WriteByte(server.IpAddress[1])
                .WriteByte(server.IpAddress[2])
                .WriteByte(server.IpAddress[3])

                .WriteInt(server.Port)

                .WriteByte(server.AgeLimit)
                .WriteByte(server.IsPvp ? 0 : 1)
                .WriteShort(server.CurrentOnline)
                .WriteShort(server.MaxPlayers)
                .WriteByte(server.Status)
                .WriteInt(server.ServerType)
                .WriteByte(server.Brackets ? 1 : 0);

            }
            buffer.WriteShort(0); //end of servers
            buffer.WriteShort(0); // characters on servers;
            return buffer; 

        }

        private AuthenticationResult ProcessAuthentication(byte[] id, byte[] pw)
        {
            return AuthenticationResult.Succes;
        }

        private static byte[] ExtractId(byte[] dec)
        {
            var idStart = 3;
            var idEnd = 17;
            return dec.Slice(idStart, idEnd);
        }

        private static byte[] ExtractPw(byte[] dec)
        {
            var pwStart = 17;
            var pwEnd = 17 + 16;
            return dec.Slice(pwStart, pwEnd);
        }
    }
}
