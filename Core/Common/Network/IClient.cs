namespace Core.Common.Network
{
    public interface IClient
    {
        public bool HasData();
        public ReadableBuffer ReceiveData();
        public void ForceDisconnect();
    }
}
