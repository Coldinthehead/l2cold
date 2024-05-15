using Core.Game.Data;
using Core.Game.Network;
using Core.Game.Network.ClientPacket;
using Core.Logs;
using Core.Utils.NetworkBuffers;
using Core.Math;

namespace Core.Game.Contorller
{
    public class EnterWorldController : IPacketController
    {
        private static Logger<EnterWorldController> _logger = Logger<EnterWorldController>.BuildLogger();
        private readonly ActivePlayers _players;

        public EnterWorldController(ActivePlayers players)
        {
            _players = players;
        }

        public void Run(GameClient client, ReadableBuffer message)
        {
            _logger.Log($"[ENTER_WORLD] received from :", client);
            var character = GameCharacter.BuildMockCharacter();
            client.SendData(BuildMockUserInfo(client, character));
            client.SendData(BuildChangeMoveType());
            client.SendData(BuildQuestList());
            client.SendData(BuildMagicIconEffects());
            client.SendData(BuildETCStatusUpdate());
            client.SendData(BuildHennaInfo());
            client.SendData(BuildFriendList());
            client.SendData(BuildItemList());
            client.SendData(BuildShortcutInit());
            client.SendData(BuildExStorageMaxCount());
            client.SendData(BuildMacroList());
            client.SendData(BuildClientTime());
            client.SendData(BuildSkillList());
            client.SendData(BuildTargetSelected());
            client.SendData(BuildSetCompasZone());
            client.SendData(BuildActionFailed());
            _players.AddPlayer(client, new Player(character.Info.ObjectId, new Vec2((float)character.x, (float)character.y)));
        }

        private static byte[] BuildMagicIconEffects()
        {
            return new WriteableBuffer().WriteByte(OutPacket.MAGIC_EFFECT_ICONS).WriteShort(0).toByteArray();
        }

        private static byte[] BuildActionFailed()
        {
            return new WriteableBuffer().WriteByte(OutPacket.ACTION_FAILED).toByteArray();
        }

        private static byte[] BuildSetCompasZone()
        {
            return new WriteableBuffer().WriteByte(OutPacket.SET_COMPAS_ZONE).WriteShort(0x32).WriteInt(0x0f).toByteArray();
        }

        private static byte[] BuildSkillList()
        {
            return new WriteableBuffer().WriteByte(OutPacket.SKILL_LIST).WriteInt(0).toByteArray();
        }

        private static byte[] BuildShortcutInit()
        {
            return new WriteableBuffer().WriteByte(OutPacket.SHORTCUT_INIT).WriteInt(0).toByteArray();
        }

        private static byte[] BuildItemList()
        {
            return new WriteableBuffer().WriteByte(OutPacket.ITEM_LIST).WriteShort(0).WriteShort(0).toByteArray();
        }

        private static byte[] BuildFriendList()
        {
            return new WriteableBuffer().WriteByte(OutPacket.FRIEND_LIST).WriteInt(0).toByteArray();
        }

        private static byte[] BuildQuestList() => new WriteableBuffer().WriteByte(OutPacket.QUEST_LIST).WriteInt(0).toByteArray();

        private static byte[] BuildTargetSelected()
        {
            var packet = new WriteableBuffer();
            packet.WriteByte(OutPacket.TARGET_SELECTED)
                .WriteInt(1)
                .WriteInt(1)
                .WriteDouble(0).WriteDouble(0).WriteDouble(0);

            return packet.toByteArray();
        }

        private static byte[] BuildClientTime()
        {
            var packet = new WriteableBuffer();
            packet.WriteByte(OutPacket.SET_CLIENT_TIME).WriteInt(7000).WriteInt(6);

            return packet.toByteArray();
        }

        private static byte[] BuildMacroList()
        {
            var packet = new WriteableBuffer();
            packet.WriteByte(OutPacket.MACRO_LIST).WriteInt(0).WriteBytes([0,0,0]);

            return packet.toByteArray();
        }

        private static byte[] BuildExStorageMaxCount()
        {
            var packet = new WriteableBuffer();
            packet.WriteByte(0xfe).WriteShort(0x2e);
            for (int i = 0; i < 7; i++)
            {
                packet.WriteInt(0);
            }

            return packet.toByteArray();
        }

        private static byte[] BuildHennaInfo()
        {
            var packet = new WriteableBuffer();
            packet.WriteByte(OutPacket.HENNA_INFO);
            packet.WriteBytes([0, 0, 0, 0, 0, 0]);
            packet.WriteInt(0)
                .WriteInt(0);

            return packet.toByteArray();
        }

        private static byte[] BuildETCStatusUpdate()
        {
            var packet = new WriteableBuffer();
            packet.WriteByte(OutPacket.ETC_STATUS_UPDATE)
                .WriteInt(0) // dulist power
                .WriteInt(0) //weight penalty
                .WriteInt(0) // chat block
                .WriteInt(0) // danger zone
                .WriteInt(0)
                .WriteInt(0)
                .WriteInt(0); // penalty level 1-15

            return packet.toByteArray();
        }

        private static byte[] BuildChangeMoveType()
        {
            var packet = new WriteableBuffer();
            packet.WriteByte(OutPacket.CHANGE_MOVE_TYPE)
                .WriteInt(1)//obj id
                .WriteInt(1)//is running
                .WriteInt(0);

            return packet.toByteArray();
        }

        private static byte[] BuildMockUserInfo(GameClient client, GameCharacter character)
        {
            var packet = new WriteableBuffer();

            var info = character.Info;
            var stats = character.Stats;
            packet.WriteByte(OutPacket.USER_INFO)

                .WriteInt((int)character.x)
                .WriteInt((int)character.y)
                .WriteInt((int)character.z)
                .WriteInt(0) // heading
                .WriteInt(info.ObjectId)

                .WriteString(info.Name)

                .WriteInt(info.Race)
                .WriteInt(info.Female ? 1 : 0)
                .WriteInt(info.CurrentClass)
                .WriteInt(info.Level)

                .WriteLong(info.Exp)

                .WriteInt(stats.STR)
                .WriteInt(stats.DEX)
                .WriteInt(stats.CON)
                .WriteInt(stats.INT)
                .WriteInt(stats.MEN)
                .WriteInt(stats.WIT)

                .WriteInt((int)info.MaxHealth)
                .WriteInt((int)info.CurrentHealth)
                .WriteInt((int)info.MaxMana)
                .WriteInt((int)info.CurrentMana)

                .WriteInt(info.Sp)
                .WriteInt(0) // current weight;
                .WriteInt(0) // max weight
                .WriteInt(20); // is weapon equipped 20 no 40 yes

            var itemIds = info.ObjectsId;
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

            itemIds = info.ItemsId;
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

            for (int i = 0; i < 14; i++)
                packet.WriteShort(0x00);

            packet.WriteInt(0x00);

            for (int i = 0; i < 12; i++)
                packet.WriteShort(0x00);

            packet.WriteInt(0);

            for (int i = 0; i < 4; i++)
                packet.WriteShort(0x00);

            packet.WriteInt(stats.Patk)
                .WriteInt(stats.PatkSpd)
                .WriteInt(stats.Pdef)
                .WriteInt(stats.Evasion)
                .WriteInt(stats.Accuracy)
                .WriteInt(stats.Crit)
                .WriteInt(stats.Matk)
                .WriteInt(stats.MatkSpd)
                .WriteInt(stats.PatkSpd)
                .WriteInt(stats.Mdef);

            packet.WriteInt(0) // pvp flag 
                .WriteInt(info.Karma);

            packet.WriteInt(stats.RunSpd)
                .WriteInt(stats.WalkSpd)
                .WriteInt(stats.RunSpd)//swim
                .WriteInt(stats.WalkSpd) //swim
                .WriteInt(0)
                .WriteInt(0)
                .WriteInt(0) // fly speed;
                .WriteInt(0) // fly speed;

                .WriteDouble(1.0)// mov multiplier
                .WriteDouble(1.0) // attck spd multiplier
                .WriteDouble(16.0) // coll radius
                .WriteDouble(32.0); // coll height

            packet.WriteInt(info.HairStyle)
                .WriteInt(info.HairColor)
                .WriteInt(info.Face)
                .WriteInt(1) // is gm

                .WriteString(character.Title);

            packet
                .WriteInt(info.ClanId)
                .WriteInt(0) // clan crest id
                .WriteInt(0) // ally id
                .WriteInt(0) // ally crest id
                .WriteInt(0) // is clan leader

                .WriteByte(0) // mount type?
                .WriteByte(0) // private store type 
                .WriteByte(0) // can craft

                .WriteInt(0) // pvp kills
                .WriteInt(0); // pk kills

            packet.WriteShort(0); // cubics
            packet.WriteByte(0); // end cubics

            packet.WriteInt(0); // abnormal effect mask
            packet.WriteByte(0);

            packet.WriteInt(0) // clan privs

                .WriteShort(0) // rec have
                .WriteShort(0) // rec left

                .WriteInt(0) // mount id?

                .WriteShort(7777) // inventory limit

                .WriteInt(info.CurrentClass)
                .WriteInt(0)
                .WriteInt(110) // max cp
                .WriteInt(0) // current cp

                .WriteByte(0) // enchant effect
                .WriteByte(0) // event circle 

                .WriteInt(0) // clan crest large id?

                .WriteByte(0) // is noble
                .WriteByte(0) // is hero

                .WriteByte(0) // is fishing

                .WriteInt(0)
                .WriteInt(0)
                .WriteInt(0) // fishing xyz

                .WriteInt(int.MaxValue) // name color
                .WriteByte(1) // is running

                .WriteInt(0) // pledge class
                .WriteInt(0) // pledge type
                .WriteInt(int.MaxValue / 2) //title color
                .WriteInt(0); // curesed weapon


            var res = packet.toByteArray();
            return res;
        }
    }
}
