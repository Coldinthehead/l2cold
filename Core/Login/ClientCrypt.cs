using Core.Security.Crypt;
using Core.Utils;
using Org.BouncyCastle.Crypto.Engines;

namespace Core.Login
{
    public class ClientCrypt
    {
        public int SessionId => _sessionId;
        public byte[] blowfishKey;
        public BlowfishCipher cipher;

        public ScrambledKeyPair RsaKeyPair;
        private int _sessionId;
        public ClientCrypt(ScrambledKeyPair pair)
        {
            blowfishKey = ServerRandom.NextBytes(16);
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
            _sessionId = ServerRandom.Next(50, 250) * ServerRandom.Next(25, 255) * ServerRandom.Next(1, 13) + ServerRandom.Next(100000, 10000000);
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
