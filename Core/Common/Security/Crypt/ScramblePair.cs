using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Core.Common.Security.Crypt
{
    public class ScrambledKeyPair
    {
        public AsymmetricCipherKeyPair Pair => _pair;
        private AsymmetricCipherKeyPair _pair;

        private AsymmetricKeyParameter _publicKey;

        public byte[] _scrambledModulus;

        public AsymmetricKeyParameter _privateKey;

        public ScrambledKeyPair(AsymmetricCipherKeyPair pPair)
        {
            _pair = pPair;
            _publicKey = pPair.Public;
            _scrambledModulus = scrambleModulus((_publicKey as RsaKeyParameters).Modulus);
            _privateKey = pPair.Private;
        }

        public static ScrambledKeyPair GetRandomPair()
        {
            return new ScrambledKeyPair(genKeyPair());
        }

        public static AsymmetricCipherKeyPair genKeyPair()
        {
            SecureRandom random = new SecureRandom();
            RsaKeyGenerationParameters param = new RsaKeyGenerationParameters(
               new BigInteger("65537"), random, 1024, 10);
            RsaKeyPairGenerator rSAKeyPairGenerator = new RsaKeyPairGenerator();
            rSAKeyPairGenerator.Init(param);
            return rSAKeyPairGenerator.GenerateKeyPair();
        }

        public byte[] scrambleModulus(BigInteger modulus)
        {
            byte[] array = modulus.ToByteArray();
            if (array.Length == 129 && array[0] == 0)
            {
                byte[] array2 = new byte[128];
                Array.Copy(array, 1, array2, 0, 128);
                array = array2;
            }

            for (int i = 0; i < 4; i++)
            {
                byte b = array[i];
                array[i] = array[77 + i];
                array[77 + i] = b;
            }

            for (int i = 0; i < 64; i++)
            {
                array[i] ^= array[64 + i];
            }

            for (int i = 0; i < 4; i++)
            {
                array[13 + i] = (byte)(array[13 + i] ^ array[52 + i]);
            }

            for (int i = 0; i < 64; i++)
            {
                array[64 + i] = (byte)(array[64 + i] ^ array[i]);
            }

            return array;
        }
    }
}
