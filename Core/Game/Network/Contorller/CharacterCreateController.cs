using Core.Common.Network;
using Core.Game.Network.ClientPacket;
using Core.Game.Network.Controller;
using Core.Game.Repository;

namespace Core.Game.Network.Contorller
{
    public class CharacterCreateController : IPacketController
    {
        private readonly PlayerTempaltesRepository _templates;

        public CharacterCreateController(PlayerTempaltesRepository templates)
        {
            _templates = templates;
        }

        public void Run(GameClient client, ReadableBuffer message)
        {
            Console.WriteLine("Request create character from " + client);

            var startingTempaltes = _templates.GetStartingTempaltes();
            client.SendData(OutPacketFactory.BuildNewCharacterList(startingTempaltes));
        }
    }
}
