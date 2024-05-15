using Core.Game.Data;
using Core.Game.Network;
using Core.Logs;
using Core.Utils.NetworkBuffers;

namespace Core.Game.Contorller
{
    public class CharacterSelectedController : IPacketController
    {
        private static Logger<CharacterSelectedController> _logger = Logger<CharacterSelectedController>.BuildLogger();

        public void Run(GameClient client, ReadableBuffer message)
        {
            var charId = message.ReadInt();
            _logger.Log($"[CHARACTER_SELECTED] id:[{charId}] from:", client);
            client.SendData(BuildSelectedCharacter(client.Skeys.Play2));
        }

        private static byte[] BuildSelectedCharacter(int pkey2)
        {
            var character = GameCharacter.BuildMockCharacter();
            var info = character.Info;
            var packet = new WriteableBuffer();
            packet.WriteByte(0x15)
                .WriteString(info.Name)
                .WriteInt(info.ObjectId)
                .WriteString(character.Title)
                .WriteInt(pkey2)
                .WriteInt(info.ClanId)
                .WriteInt(0)
                .WriteInt(info.Female ? 1 : 0)
                .WriteInt(info.Race)
                .WriteInt(info.CurrentClass)
                .WriteInt(1) // is active ?
                .WriteInt((int)character.x).WriteInt((int)character.y).WriteInt((int)character.z)
                .WriteDouble(info.CurrentHealth)
                .WriteDouble(info.CurrentMana)
                .WriteInt(info.Sp)
                .WriteLong(info.Exp)
                .WriteInt(info.Level)
                .WriteInt(info.Karma)
                .WriteInt(0);

            for (int i = 0; i < 38; i++)
                packet.WriteInt(0);

            packet.WriteInt(1234); // game time on server

            for (int i = 0; i < 14; i++)
                packet.WriteInt(0);


            return packet.toByteArray();
        }
    }
}
