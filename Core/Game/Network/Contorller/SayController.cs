using Core.Common.Network;
using Core.Game.Network.Controller;

namespace Core.Game.Network.Contorller
{
    internal class SayController : IPacketController
    {
        public void Run(GameClient client, ReadableBuffer message)
        {
            var text = message.ReadString();
            var type = message.ReadInt();
            Console.WriteLine($"[{type}]<{text}>");
        }
    }
}
