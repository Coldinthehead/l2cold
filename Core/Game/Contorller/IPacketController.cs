using Core.Game.Network;
using Core.Utils.NetworkBuffers;

namespace Core.Game.Contorller
{
    public interface IPacketController
    {
        public void Run(GameClient client, ReadableBuffer message);
    }
}
