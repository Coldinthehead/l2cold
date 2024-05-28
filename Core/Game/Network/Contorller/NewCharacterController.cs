using Core.Common.Network;
using Core.Game.Data.User;
using Core.Game.Network.ClientPacket;
using Core.Game.Network.Controller;
using Core.Game.Services;

namespace Core.Game.Network.Contorller
{
    public class NewCharacterController : IPacketController
    {
        private readonly CharacterService _characterService;
        public NewCharacterController(CharacterService characterService)
        {
            _characterService = characterService;
        }

        public void Run(GameClient client, ReadableBuffer message)
        {
            string name = message.ReadString();
            int race = message.ReadInt();
            bool female = message.ReadInt() == 1;
            int templateId = message.ReadInt();
            for (int i = 0; i < 6; i++) message.ReadInt(); // skip stats
            int hairStyle = message.ReadInt();
            int hairColor = message.ReadInt();
            int face = message.ReadInt();

            _characterService.CreateNewCharacter(client, templateId, 
                new PlayerAppearance(name, race,face,hairStyle, hairColor, female));
            client.SendData(OutPacketFactory.BuildCharacterCreateOk());
            client.SendData(OutPacketFactory.BuildCharSelectList(client ,
                _characterService.LoadCharacterList(client.AccountName)));

            /* _characterService.CreateCharacter(client.AccountName, name,
                 new PlayerAppearance(race, face, hairStyle, hairColor, female, templateId));

             Console.WriteLine("request create character with name" + name);

             client.SendData(OutPacketFactory.BuildCharacterCreateOk());
             client.SendData(OutPacketFactory.BuildCharSelectList(client
            , _characterService.LoadCharacterList(client.AccountName)));*/
        }
    }
}
