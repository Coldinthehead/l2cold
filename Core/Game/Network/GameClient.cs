using Core.Game.World.Actor;
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

        public Player Player { get; set; }
        public int Ping { get; set; }

        private SessionKeys _sKeys;
        private ReadQue _inQue;

        public GameClient(TcpClient client, IDataCrypter cryptInterface)
        {
            _connection = client;
            _connection.NoDelay = true;
            _cryptInterface = cryptInterface;
            _sKeys = SessionKeys.GetEmptyKeys();
            _inQue = new ReadQue(client);
            Ping = 0;
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
            Console.WriteLine($"in << {data[2].ToHex()} for:" + this);
            return new ReadableBuffer(data.Slice(2));
        }

        public void SendData(byte[] data)
        {
            Console.WriteLine($"out >> {data[2].ToHex()} for : " + this);
            var outData = new byte[data.Length];
            Array.Copy(data, outData, data.Length);
            _cryptInterface.CryptInPlace(outData, 2, outData.Length - 2);
            _connection.GetStream().Write(outData);
        }

        public override string ToString()
        {
            return Player == null ?
             _connection.Client.RemoteEndPoint.ToString()
             : Player.CharacterDetails.Info.Name;
        }
    }
}
