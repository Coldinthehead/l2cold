using Core.Network;
using Org.BouncyCastle.Crypto.Engines;

namespace Core
{
    public class ClientCrypt
    {
        private readonly Random rand = new Random();

        public int SessionId => _sessionId;
        public byte[] blowfishKey;
        public BlowfishCipher cipher;

        public ScrambledKeyPair RsaKeyPair;
        private int _sessionId;
        public ClientCrypt(ScrambledKeyPair pair)
        {
            blowfishKey = Program.GenerateBytes(16);
            cipher = new BlowfishCipher(blowfishKey);
            _sessionId = 0;
            RsaKeyPair = pair;
        }

        public void DecryptDataBlowfish(byte[] data)
        {
            cipher.decrypt(data, 0, data.Length - 2);
        }

        public void GenerateSessionId()
        {
            _sessionId = rand.Next(50, 250) * rand.Next(25, 255) * rand.Next(1, 13) + rand.Next(100000, 10000000);
        }

        public byte[] DecryptBlockRSA(byte[] data)
        {
            RsaEngine rsaEngine = new RsaEngine();
            rsaEngine.Init(false, RsaKeyPair._privateKey);
            var res = rsaEngine.ProcessBlock(data, 0, data.Length);
            return res;

        }
    }
}
