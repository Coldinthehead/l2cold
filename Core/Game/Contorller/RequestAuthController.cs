using Core.Common.Services;
using Core.Game.Data;
using Core.Game.Network;
using Core.Login;
using Core.Logs;
using Core.Security;
using Core.Utils.NetworkBuffers;


namespace Core.Game.Contorller
{
    internal class RequestAuthController : IPacketController
    {
        private static Logger<RequestAuthController> _logger = Logger<RequestAuthController>.BuildLogger();

        private readonly LoginServerService _loginServer;

        public RequestAuthController(LoginServerService logginServer)
        {
            _loginServer = logginServer;
        }

        public void Run(GameClient client, ReadableBuffer message)
        {
            _logger.Log($"Received [REQUEST_AUTH] from [{client}]");
            var accId = message.ReadString();
            var sessionKeys = ReadSessinKeys(message);
            var accDetails = new LSAccountDetails(accId, sessionKeys);
            if (_loginServer.IsAccountLoggedIn(accDetails))
            {
                client.SendData(BuildCharInfo(accDetails));
                client.SetSessionKeys(sessionKeys);
            }
        }

        private static SessionKeys ReadSessinKeys(ReadableBuffer message)
        {
            int playKey2 = message.ReadInt();
            int playKey1 = message.ReadInt();
            int login1 = message.ReadInt();
            int login2 = message.ReadInt();
            return SessionKeys.FromValues(playKey1, playKey2, login1, login2);
        }

        private static byte[] BuildCharInfo(LSAccountDetails accDetails)
        {
            var characterSlosts = new List<CharacterSlotInfo>();
            characterSlosts.Add(CharacterSlotInfo.BuildMockCharacterSlot());
            characterSlosts.Add(CharacterSlotInfo.BuildMockCharacterSlot());
            characterSlosts.Add(CharacterSlotInfo.BuildMockCharacterSlot());
            characterSlosts.Add(CharacterSlotInfo.BuildMockCharacterSlot());

            var packet = new WriteableBuffer();
            packet.WriteByte(0x13)
                .WriteInt(characterSlosts.Count); // characters count;

            foreach (var character in characterSlosts)
            {
                packet.WriteString(character.Name)
                    .WriteInt(character.ObjectId)
                    .WriteString(accDetails.Id)
                    .WriteInt(accDetails.Skeys.Play1)
                    .WriteInt(character.ClanId)
                    .WriteInt(0) // ?
                    .WriteInt(character.Female ? 1 : 0)
                    .WriteInt(character.Race)
                    .WriteInt(character.BaseClass)
                    .WriteInt(1)  // ?
                    .WriteInt(0).WriteInt(0).WriteInt(0) // xyz
                    .WriteDouble(character.CurrentHealth)
                    .WriteDouble(character.CurrentMana)
                    .WriteInt(character.Sp)
                    .WriteLong(character.Exp)
                    .WriteInt(character.Level)
                    .WriteInt(character.Karma);
                for (int i = 0; i < 9; i++)
                    packet.WriteInt(0);

                var itemIds = character.ObjectsId;
                packet.WriteInt(itemIds.D_HAIR)
                    .WriteInt(itemIds.R_EAR)
                    .WriteInt(itemIds.L_EAR)
                    .WriteInt(itemIds.NECK)
                    .WriteInt(itemIds.R_FINGER)
                    .WriteInt(itemIds.L_FINGER)
                    .WriteInt(itemIds.HEAD)
                    .WriteInt(itemIds.R_HAND)
                    .WriteInt(itemIds.L_HAND)
                    .WriteInt(itemIds.GLOVES)
                    .WriteInt(itemIds.CHEST)
                    .WriteInt(itemIds.LEGS)
                    .WriteInt(itemIds.FEET)
                    .WriteInt(itemIds.BACK)
                    .WriteInt(itemIds.LR_HAND)
                    .WriteInt(itemIds.HAIR)
                    .WriteInt(itemIds.FACE);
                itemIds = character.ItemsId;
                packet.WriteInt(itemIds.D_HAIR)
                  .WriteInt(itemIds.R_EAR)
                  .WriteInt(itemIds.L_EAR)
                  .WriteInt(itemIds.NECK)
                  .WriteInt(itemIds.R_FINGER)
                  .WriteInt(itemIds.L_FINGER)
                  .WriteInt(itemIds.HEAD)
                  .WriteInt(itemIds.R_HAND)
                  .WriteInt(itemIds.L_HAND)
                  .WriteInt(itemIds.GLOVES)
                  .WriteInt(itemIds.CHEST)
                  .WriteInt(itemIds.LEGS)
                  .WriteInt(itemIds.FEET)
                  .WriteInt(itemIds.BACK)
                  .WriteInt(itemIds.LR_HAND)
                  .WriteInt(itemIds.HAIR)
                  .WriteInt(itemIds.FACE);

                packet.WriteInt(character.HairStyle)
                    .WriteInt(character.HairColor)
                    .WriteInt(character.Face)
                    .WriteDouble(character.MaxHealth)
                    .WriteDouble(character.MaxMana)
                    .WriteInt(0) // deleting time
                    .WriteInt(character.CurrentClass)
                    .WriteInt(0) // is active
                    .WriteByte(0) // enchant effect
                    .WriteInt(0); // augmentation id
            }


            return packet.toByteArray();
        }

     
    }
}
