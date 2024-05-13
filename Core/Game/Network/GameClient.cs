using Core.Security.Crypt;
using Core.Utils;
using System.Net.Sockets;

namespace Core.Game.Network
{

    public class GameClient
    {
        private readonly TcpClient _connection;
        private IDataCrypter _cryptInterface;

        public GameClient(TcpClient client, IDataCrypter cryptInterface)
        {
            _connection = client;
            _cryptInterface = cryptInterface;
        }

        public void SetCryptInterface(IDataCrypter cryptInterface)
        {
            _cryptInterface = cryptInterface;
        }

        internal void ForceDisonnect()
        {
            _connection.Close();
        }

        internal bool HasData()
        {
            return _connection.Available > 0;
        }

        public byte[] ReceiveRawData()
        {
            byte[] data = new byte[_connection.Available];
            _connection.GetStream().Read(data, 0, data.Length);
            var packet = data.Slice(2); ;
            _cryptInterface.DecryptInPlace(packet, 0, packet.Length);
            return packet;
        }

        public void SendData(byte[] data)
        {
            _cryptInterface.CryptInPlace(data, 2, data.Length - 2);
            _connection.GetStream().Write(data);
            _connection.GetStream().Flush();
        }
    }
}
