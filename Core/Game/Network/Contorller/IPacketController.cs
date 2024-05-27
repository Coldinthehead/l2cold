using Core.Common.Network;
using Core.Game.Network;

namespace Core.Game.Network.Controller
{
    public interface IPacketController
    {
        public void Run(GameClient client, ReadableBuffer message);
    }
}
