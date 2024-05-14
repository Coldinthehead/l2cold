using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils
{
    public class ReadQue
    {
        public bool TryRead() => Read();
        public byte[] GetPacket() => _packets.Dequeue();

        private byte[] _buffer;
        private int _position;
        private readonly Queue<byte[]> _packets = new();
        private readonly NetworkStream _stream;
        private readonly Socket _socket;
        public ReadQue(TcpClient client)
        {
            _socket = client.Client;
            _stream = client.GetStream();
            _buffer = null;
            _position = 0;
        }

        public bool Read()
        {
            if (_socket.Available > 0)
            {
                if (_buffer == null)
                {
                    CreateBuffer();
                }
                var availableSize = TrimSize();
                ReadData(availableSize);
                if (_position >= _buffer.Length)
                {
                    _packets.Enqueue(_buffer);
                    DeleteBuffer();
                }
                var hasCompetedPackets = _packets.Count > 0;
                return hasCompetedPackets;
            }

            return false;
        }

        private void ReadData(int size)
        {
            _stream.Read(_buffer, _position, size);
            _position += size;
        }

        private void CreateBuffer()
        {
            var len = _stream.ReadByte() | (_stream.ReadByte() << 8);
            _buffer = new byte[len];
            _position = 2;
        }

        private void DeleteBuffer()
        {
            _buffer = null;
            _position = 0;
        }

        private int TrimSize()
        {
           return  _socket.Available < _buffer.Length - _position ? _socket.Available : _buffer.Length -_position;
        }
   

    }
}
