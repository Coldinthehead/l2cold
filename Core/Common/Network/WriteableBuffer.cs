namespace Core.Common.Network
{

    internal class WriteableBuffer
    {
        private List<byte> _data = new();


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
            byte[] arr = new byte[_data.Count + 2];
            var len = _data.Count + 2;
            arr[0] = (byte)len;
            arr[1] = (byte)(len >> 8);
            for (int i = 0; i < _data.Count; i++)
            {
                arr[2 + i] = _data[i];
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

        public WriteableBuffer WriteString(string str)
        {
            var data = System.Text.Encoding.Unicode.GetBytes(str);
            WriteBytes(data);
            WriteShort(0);
            return this;
        }

        public WriteableBuffer WriteLong(long value)
        {
            WriteRaw((byte)value);
            WriteRaw((byte)(value >> 8));
            WriteRaw((byte)(value >> 16));
            WriteRaw((byte)(value >> 24));
            WriteRaw((byte)(value >> 32));
            WriteRaw((byte)(value >> 40));
            WriteRaw((byte)(value >> 48));
            WriteRaw((byte)(value >> 56));
            return this;
        }

        public WriteableBuffer WriteDouble(double value)
        {
            var data = BitConverter.GetBytes(value);
            var d1 = BitConverter.ToInt64(data);
            WriteLong(d1);
            return this;
        }

        internal object WriteByte(object mOVED_TO_LOCATION)
        {
            throw new NotImplementedException();
        }

        internal object WriteInt(object pAtkSpd)
        {
            throw new NotImplementedException();
        }
    }
}
