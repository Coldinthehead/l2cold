using Core.Security.Crypt;
using Core.Utils;

namespace Core.Game.Network
{
    public class GameCrypt : IDataCrypter
    {
        public byte[] GetKey() => _blowfihsKey;

        private readonly byte[] _blowfihsKey;

        private readonly byte[] _inKey = new byte[16];
        private readonly byte[] _outKey = new byte[16];
        public GameCrypt()
        {
            _blowfihsKey = GenerateBlowfishKey();
            Array.Copy(_blowfihsKey, _inKey, 16);
            Array.Copy(_blowfihsKey, _outKey, 16);
        }

        private static byte[] STATIC_HALF = { 0xc8, 0x27, 0x93, 0x01, 0xa1, 0x6c, 0x31, 0x97 };
        private static byte[] GenerateBlowfishKey()
        {
            var half = ServerRandom.NextBytes(8);
            var res = new byte[16];
            for (int i = 0; i < 8; i++)
            {
                res[i] = half[i];
            }
            for (int i = 8; i < 16; i++)
            {
                res[i] = STATIC_HALF[i - 8];
            }
            return res;
        }

        public void DecryptInPlace(byte[] data, int offset, int size)
        {
            int a = 0;
            for (int i = 0; i < size; i++)
            {
                int b = data[offset + i] & 0xff;
                data[offset + i] = (byte)(b ^ _inKey[i & 15] ^ a);
                a = b;
            }

            // Shift key.
            int old = _inKey[8] & 0xff;
            old |= (_inKey[9] << 8);
            old |= (_inKey[10] << 16);
            old |= (_inKey[11] << 24);
            old += size;
            _inKey[8] = (byte)(old);
            _inKey[9] = (byte)((old >> 8));
            _inKey[10] = (byte)((old >> 16));
            _inKey[11] = (byte)((old >> 24));
        }

        public void CryptInPlace(byte[] data, int offset, int size)
        {
            int a = 0;
            for (int i = 0; i < size; i++)
            {
                int b = data[offset + i] & 0xff;
                a = b ^ _outKey[i & 15] ^ a;
                data[offset + i] = (byte)a;
            }

            // Shift key.
            int old = _outKey[8] & 0xff;
            old |= (_outKey[9] << 8) & 0xff00;
            old |= (_outKey[10] << 16) & 0xff0000;
            old |= (_outKey[11] << 24) & 0xff0000;
            old += size;
            _outKey[8] = (byte)(old & 0xff);
            _outKey[9] = (byte)((old >> 8) & 0xff);
            _outKey[10] = (byte)((old >> 16) & 0xff);
            _outKey[11] = (byte)((old >> 24) & 0xff);
        }


    }

    public class NoCrypter : IDataCrypter
    {
        public void CryptInPlace(byte[] data, int offset, int size)
        {
            
        }

        public void DecryptInPlace(byte[] data, int offset, int size)
        {
            
        }
    }
}
