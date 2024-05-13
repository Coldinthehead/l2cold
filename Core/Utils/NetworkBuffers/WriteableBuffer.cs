namespace Core.Utils.NetworkBuffers
{

    internal class WriteableBuffer
    {
        private List<byte> _data;

        public WriteableBuffer(int size)
        {
            _data = new List<byte>(2 + size)
            {
                0,0, // 
            };
        }

        public WriteableBuffer()
        {
            _data = new List<byte>(32)
            {
                0,0, // 
            };
        }

        private void WriteRaw(byte b)
        {
            _data.Add(b);
        }

        public WriteableBuffer WriteByte(int value)
        {
            WriteRaw((byte)value);
            return this;
        }



        public WriteableBuffer WriteInt(int value)
        {
            WriteRaw((byte)value);
            WriteRaw((byte)(value >> 8));
            WriteRaw((byte)(value >> 16));
            WriteRaw((byte)(value >> 24));
            return this;
        }

        public WriteableBuffer WriteInt(uint value)
        {
            WriteRaw((byte)value);
            WriteRaw((byte)(value >> 8));
            WriteRaw((byte)(value >> 16));
            WriteRaw((byte)(value >> 24));
            return this;
        }


        public byte[] toByteArray()
        {
            byte[] arr = new byte[_data.Count];
            var len = _data.Count;
            arr[0] = (byte)len;
            arr[1] = (byte)(len >> 8);
            for (int i = 2; i < _data.Count; i++)
            {
                arr[i] = _data[i];
            }
            return arr;
        }

        internal WriteableBuffer WriteBytes(byte[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                WriteRaw(data[i]);
            }
            return this;
        }

        internal WriteableBuffer WriteShort(int value)
        {
            WriteRaw((byte)value);
            WriteRaw((byte)(value >> 8));

            return this;
        }
    }
}
