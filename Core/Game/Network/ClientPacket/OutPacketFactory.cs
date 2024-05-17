using Core.Common.Services;
using Core.Math;
using Core.Utils.NetworkBuffers;

namespace Core.Game.Network.ClientPacket
{
    public class OutPacketFactory
    {
        public static byte[] BuildSelectedCharacter(int pkey2, Player player)
        {
            var character = player.CharacterDetails;
            var info = player.CharacterDetails.Info;
            var packet = new WriteableBuffer();
            packet.WriteByte(OutPacket.CHARACTER_SELECTED)
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

        public static byte[] BuildSkillCd()
        {
            return new WriteableBuffer().WriteByte(OutPacket.SKILL_CD).WriteInt(0).toByteArray();
        }

        public static byte[] BuildCryptInit(byte[] key)
        {
            var cryptInit = new WriteableBuffer();
            cryptInit.WriteByte(OutPacket.CRYPT_INIT)
                .WriteByte(1); // 0 protocol missmatch 1 good
            for (int i = 0; i < 8; i++)
                cryptInit.WriteByte(key[i]);
            cryptInit.WriteInt(1) // use encryption
                .WriteInt(1) // server id??/
                .WriteByte(1); // unknown

            return cryptInit.toByteArray();
        }

        public static byte[] BuildOutMoveToLocation(GameClient client, Player player,Vec2 target, int targetZ)
        {
            var origin = player.CalculatePositionOnPing(client.Ping);
            var movePacket = new WriteableBuffer();
            movePacket.WriteByte(OutPacket.MOVED_TO_LOCATION)
                .WriteInt(player.ObjectId)
                .WriteInt((int)target.x)
                .WriteInt((int)target.y)
                .WriteInt(targetZ)
                .WriteInt((int)origin.x)
                .WriteInt((int)origin.y)
                .WriteInt((int)player.ZPosition);

            Console.WriteLine("Building packet for player with id" + player.ObjectId);

            return movePacket.toByteArray();
        }

        public static byte[] BuildCharInfo(LSAccountDetails accDetails, List<Player> characterList)
        {
            var packet = new WriteableBuffer();
            packet.WriteByte(OutPacket.CHARACTER_LIST_INFO)
                .WriteInt(characterList.Count); // characters count;

            foreach (var player in characterList)
            {
                var character = player.CharacterDetails.Info;
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

                var itemIds = player.CharacterDetails.GeartItemId;
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
                itemIds = player.CharacterDetails.GearObjectId;
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
        public static byte[] BuildRelationChanged(Player player)
        {
            var packet = new WriteableBuffer();
            packet.WriteByte(OutPacket.RELATION_CHANGED)
                .WriteInt(player.ObjectId)
                .WriteInt(0) // relation
                .WriteInt(0) // attackable?
                .WriteInt(0) // karma
                .WriteInt(0); // pvpflag;

            return packet.toByteArray();
        }

        public static byte[] BuildCharInfo(Player player)
        {
            var mockCharacter = player.CharacterDetails;
            var packet = new WriteableBuffer();
            packet.WriteByte(OutPacket.CHAR_INFO)
                .WriteInt((int)player.Position.x)
                .WriteInt((int)player.Position.y)
                .WriteInt((int)player.ZPosition)
                .WriteInt(0) // on boat
                .WriteInt(player.ObjectId)
                .WriteString(mockCharacter.Info.Name) //playerName
                .WriteInt(mockCharacter.Info.Race)
                .WriteInt(mockCharacter.Info.Female ? 1 : 0)
                .WriteInt(mockCharacter.Info.CurrentClass);
            var itemIds = player.CharacterDetails.GeartItemId;
            packet.WriteInt(itemIds.D_HAIR)
              .WriteInt(itemIds.HEAD)
              .WriteInt(itemIds.R_HAND)
              .WriteInt(itemIds.L_HAND)
              .WriteInt(itemIds.GLOVES)
              .WriteInt(itemIds.CHEST)
              .WriteInt(itemIds.LEGS)
              .WriteInt(itemIds.FEET)
              .WriteInt(itemIds.BACK)
              .WriteInt(itemIds.LR_HAND)
              .WriteInt(0) // hair
              .WriteInt(0); // face;

            packet.WriteShort(0x00);
            packet.WriteShort(0x00);
            packet.WriteShort(0x00);
            packet.WriteShort(0x00);

            packet.WriteInt(0x00);

            packet
                .WriteShort(0x00)
                .WriteShort(0x00)
                .WriteShort(0x00)
                .WriteShort(0x00)

                .WriteShort(0x00)
                .WriteShort(0x00)
                .WriteShort(0x00)
                .WriteShort(0x00)

                .WriteShort(0x00)
                .WriteShort(0x00)
                .WriteShort(0x00)
                .WriteShort(0x00)

                .WriteInt(0)

                .WriteShort(0)
                .WriteShort(0)
                .WriteShort(0)
                .WriteShort(0);

            packet.WriteInt(0) // pvp flag
                .WriteInt(0) //arma
                .WriteInt(mockCharacter.Stats.MatkSpd)
                .WriteInt(mockCharacter.Stats.PatkSpd)
                .WriteInt(0) // pvp flag
                .WriteInt(0) //karma
                .WriteInt(mockCharacter.Stats.RunSpd)
                .WriteInt(mockCharacter.Stats.WalkSpd)
                .WriteInt(mockCharacter.Stats.RunSpd) // swim
                .WriteInt(mockCharacter.Stats.WalkSpd) // swim
                .WriteInt(mockCharacter.Stats.RunSpd) // fly
                .WriteInt(mockCharacter.Stats.WalkSpd) // fly
                .WriteInt(mockCharacter.Stats.RunSpd) // fly
                .WriteInt(mockCharacter.Stats.WalkSpd) // fly
                .WriteDouble(1.0) // move speed multiplier
                .WriteDouble(1.0) // attack speed multiplier
                .WriteDouble(16.0) // coll radius
                .WriteDouble(32.0) // coll height
                .WriteInt(mockCharacter.Info.HairStyle)
                .WriteInt(mockCharacter.Info.HairColor)
                .WriteInt(mockCharacter.Info.Face)
                .WriteString(mockCharacter.Title)
                .WriteInt(0) // clanid
                .WriteInt(0) // clan crest id
                .WriteInt(0) // ally id
                .WriteInt(0) // ally crest id
                .WriteInt(0) // ??
                .WriteByte(1) // 1 - stand 0 - sit
                .WriteByte(1) // is runing
                .WriteByte(0) // in combat
                .WriteByte(0) // is fake death?
                .WriteByte(0) // invisible
                .WriteByte(0) // mounted?
                .WriteByte(0) // shop type
                .WriteShort(0) // cubics
                .WriteByte(0) // party room?
                .WriteInt(0) // status effect mask
                .WriteByte(0) // rec left
                .WriteShort(0) // rec have
                .WriteInt(mockCharacter.Info.CurrentClass)
                .WriteInt(24555) // cp cur
                .WriteInt(2555) // cp max
                .WriteByte(0) // enchant effect
                .WriteByte(0) // tvt circle
                .WriteInt(0) // large crest id
                .WriteByte(0) // noble
                .WriteByte(0) // hero
                .WriteByte(0) // fishing
                .WriteInt(0).WriteInt(0).WriteInt(0) // xyz fishing
                .WriteInt(int.MaxValue) // name color
                .WriteInt(0) // heading??
                .WriteInt(0) // pledge class
                .WriteInt(0) // pledge type
                .WriteInt(int.MaxValue / 2) // title color
                .WriteInt(0); //cursedWeapon

            var data = packet.toByteArray();
            return data;
        }

        public static byte[] BuildMagicIconEffects()
        {
            return new WriteableBuffer().WriteByte(OutPacket.MAGIC_EFFECT_ICONS).WriteShort(0).toByteArray();
        }

        public static byte[] BuildActionFailed()
        {
            return new WriteableBuffer().WriteByte(OutPacket.ACTION_FAILED).toByteArray();
        }

        public static byte[] BuildSetCompasZone()
        {
            return new WriteableBuffer().WriteByte(OutPacket.SET_COMPAS_ZONE).WriteShort(0x32).WriteInt(0x0f).toByteArray();
        }

        public static byte[] BuildSkillList()
        {
            return new WriteableBuffer().WriteByte(OutPacket.SKILL_LIST).WriteInt(0).toByteArray();
        }

        public static byte[] BuildShortcutInit()
        {
            return new WriteableBuffer().WriteByte(OutPacket.SHORTCUT_INIT).WriteInt(0).toByteArray();
        }

        public static byte[] BuildItemList()
        {
            return new WriteableBuffer().WriteByte(OutPacket.ITEM_LIST).WriteShort(0).WriteShort(0).toByteArray();
        }

        public static byte[] BuildFriendList()
        {
            return new WriteableBuffer().WriteByte(OutPacket.FRIEND_LIST).WriteInt(0).toByteArray();
        }

        public static byte[] BuildQuestList() => new WriteableBuffer().WriteByte(OutPacket.QUEST_LIST).WriteInt(0).toByteArray();

        public static byte[] BuildTargetSelected(Player player)
        {
            var packet = new WriteableBuffer();
            packet.WriteByte(OutPacket.TARGET_SELECTED)
                .WriteInt(player.CharacterDetails.Info.ObjectId)
                .WriteInt(1)
                .WriteDouble(player.Position.x).WriteDouble(player.Position.y).WriteDouble(player.ZPosition);

            return packet.toByteArray();
        }

        public static byte[] BuildClientTime()
        {
            var packet = new WriteableBuffer();
            packet.WriteByte(OutPacket.SET_CLIENT_TIME).WriteInt(7000).WriteInt(6);

            return packet.toByteArray();
        }

        public static byte[] BuildMacroList()
        {
            var packet = new WriteableBuffer();
            packet.WriteByte(OutPacket.MACRO_LIST).WriteInt(0).WriteBytes([0, 0, 0]);

            return packet.toByteArray();
        }

        public static byte[] BuildExStorageMaxCount()
        {
            var packet = new WriteableBuffer();
            packet.WriteByte(0xfe).WriteShort(0x2e);
            for (int i = 0; i < 7; i++)
            {
                packet.WriteInt(0);
            }

            return packet.toByteArray();
        }

        public static byte[] BuildHennaInfo()
        {
            var packet = new WriteableBuffer();
            packet.WriteByte(OutPacket.HENNA_INFO);
            packet.WriteBytes([0, 0, 0, 0, 0, 0]);
            packet.WriteInt(0)
                .WriteInt(0);

            return packet.toByteArray();
        }

        public static byte[] BuildETCStatusUpdate()
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

        public static byte[] BuildChangeMoveType(int objId)
        {
            var packet = new WriteableBuffer();
            packet.WriteByte(OutPacket.CHANGE_MOVE_TYPE)
                .WriteInt(objId)//obj id
                .WriteInt(1)//is running
                .WriteInt(0);

            return packet.toByteArray();
        }

        public static byte[] BuildMockUserInfo(GameClient client, Player player)
        {
            var packet = new WriteableBuffer();
            var character = player.CharacterDetails;
            var info = character.Info;
            var stats = character.Stats;
            packet.WriteByte(OutPacket.USER_INFO)

                .WriteInt((int)player.Position.x)
                .WriteInt((int)player.Position.y)
                .WriteInt((int)player.ZPosition)
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

            var itemIds = player.CharacterDetails.GearObjectId;
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

            itemIds = player.CharacterDetails.GeartItemId;
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
