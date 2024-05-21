using Core.Common.Network;
using Core.Game.Network;

namespace Core.Game.Contorller
{
    public interface IPacketController
    {
        public void Run(GameClient client, ReadableBuffer message);
    }
}
