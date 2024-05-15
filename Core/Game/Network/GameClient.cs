using Core.Security;
using Core.Security.Crypt;
using Core.Utils;
using Core.Utils.NetworkBuffers;
using System.Net.Sockets;

namespace Core.Game.Network
{

    public class GameClient
    {
        private readonly TcpClient _connection;
        private IDataCrypter _cryptInterface;
        public SessionKeys Skeys => _sKeys;

        private SessionKeys _sKeys;
        private ReadQue _inQue;

        public GameClient(TcpClient client, IDataCrypter cryptInterface)
        {
            _connection = client;
            _cryptInterface = cryptInterface;
            _sKeys = SessionKeys.GetEmptyKeys();
            _inQue = new ReadQue(client);
        }

        public void SetCryptInterface(IDataCrypter cryptInterface)
        {
            _cryptInterface = cryptInterface;
        }

        public void SetSessionKeys(SessionKeys keys)
        {
            _sKeys = keys;
        }

        internal void ForceDisonnect()
        {
            _connection.Close();
        }

        internal bool HasData()
        {
            return _inQue.TryRead();
        }

        public ReadableBuffer ReceiveRawData()
        {
            var data = _inQue.GetPacket();
            _cryptInterface.DecryptInPlace(data, 2, data.Length-2);
            return new ReadableBuffer(data.Slice(2));
        }

        public void SendData(byte[] data)
        {
            _cryptInterface.CryptInPlace(data, 2, data.Length - 2);
            _connection.GetStream().Write(data);
            _connection.GetStream().Flush();
        }
    }
}
