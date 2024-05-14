
using System.Text;

namespace Core.Utils.NetworkBuffers
{
    public class ReadableBuffer
    {
        private readonly byte[] _buffer;
        private int _position;

        public ReadableBuffer(byte[] buffer)
        {
            _buffer = buffer;
            _position = 0;
        }

        public int ReadByte()
        {
            return _buffer[_position++];
        }

        public int ReadInt()
        {
            return _buffer[_position++]
                | _buffer[_position++] << 8
                | _buffer[_position++] << 16
                | _buffer[_position++] << 24;
        }

        public void readBytesInPlace(byte[] rawData)
        {
            for (int i = 0; i < rawData.Length; i++)
            {
                rawData[i] = _buffer[_position++];
            }
        }

        public int ReadShort()
        {
            return  _buffer[_position++]
                | (_buffer[_position++] << 8) ;
        }

        internal string ReadString()
        {
            var sb = new StringBuilder();
            int charId;
            while ( (charId = ReadShort()) != 0)
            {
                sb.Append((char)charId);
            }
            return sb.ToString();
        }
    }
}
