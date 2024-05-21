namespace Core.Common.Network
{
    public interface IPacketHadnler<T> where T : IClient
    {
        public void HandlePacket(T client, ReadableBuffer message);
    }
}
