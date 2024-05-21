namespace Core.Common.Security.Crypt
{
    public interface IDataCrypter
    {
        public void DecryptInPlace(byte[] data, int offset, int size);

        public void CryptInPlace(byte[] data, int offset, int size);
    }
}
