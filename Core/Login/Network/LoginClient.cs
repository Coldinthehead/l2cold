using Core.Common.Network;
using Core.Common.Security;
using Core.Login.Security;
using System.Net;
using System.Net.Sockets;

namespace Core.Login.Network
{
    public class LoginClient : IClient
    {

        public EndPoint RemoteEndPoint => _connection.Client.RemoteEndPoint;

        public SessionKeys SKeys => _sessionKeys;
        private readonly ClientCrypt _crypt;
        private readonly TcpClient _connection;
        private SessionKeys _sessionKeys;

        public LoginClient(TcpClient connection, ClientCrypt crypt)
        {
            _connection = connection;
            _crypt = crypt;

            _connection.SendTimeout = 0;
            _connection.NoDelay = true;
            _sessionKeys = SessionKeys.GetEmptyKeys();
        }

        public void SendInit()
        {
            _crypt.GenerateSessionId();
            WriteableBuffer buffer = new WriteableBuffer();
            buffer.WriteByte(0x00)
                .WriteInt(_crypt.SessionId)
                .WriteInt(0x0000c621)
                .WriteBytes(_crypt.RsaKeyPair._scrambledModulus)
                .WriteInt(0x29DD954E)
                .WriteInt(0x77C39CFC)
                .WriteInt(0x97ADB620)
                .WriteInt(0x07BDE0F7)
                .WriteBytes(_crypt.blowfishKey)
                .WriteByte(0);
            _connection.GetStream().Write(buffer.toByteArray());
            _connection.GetStream().Flush();
        }

        public bool HasData() => _connection.Available > 0;
        public bool Connected() => _connection.Connected;

        public TcpClient Connection => _connection;

        public int SessionId => _crypt.SessionId;

        public ReadableBuffer ReceiveData()
        {
            byte[] data = new byte[_connection.Available];
            _connection.GetStream().Read(data);
            var cryptedData = new byte[data.Length - 2];
            Array.Copy(data, 2, cryptedData, 0, data.Length - 2);
            _crypt.DecryptDataBlowfish(cryptedData);
            var readBuffer = new ReadableBuffer(cryptedData);
            return readBuffer;
        }

        public void ForceDisconnect()
        {
            _connection.Close();
        }

        public void Send(byte[] bytes)
        {
            _connection.GetStream().Write(bytes);
            _connection.GetStream().Flush();
        }

        public byte[] DecryptWithRsa(byte[] bytes)
        {
            return _crypt.DecryptBlockRSA(bytes);
        }

        public void SetSessionKeys(SessionKeys sessionKeys)
        {
            _sessionKeys = sessionKeys;
        }
    }
}
