using Core.Common.Network;
using Core.Game.Network.Controller;

namespace Core.Game.Network.Contorller
{
    public class CharacterCreateController : IPacketController
    {
        public void Run(GameClient client, ReadableBuffer message)
        {
            Console.WriteLine("Request create character from " + client);
        }
    }
}
