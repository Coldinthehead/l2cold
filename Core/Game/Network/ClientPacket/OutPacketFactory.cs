using Core.Common.Network;
using Core.Game.Data;
using Core.Game.Data.Static;
using Core.Game.World.Actor;
using Core.Game.World.Components;
using Core.Game.World.Items;
using Core.Utils.Math;
using System;

namespace Core.Game.Network.ClientPacket
{
    public class OutPacketFactory
    {
        public static byte[] BuildSelectedCharacter(int pkey2, GameCharacterModel model)
        {
            var packet = new WriteableBuffer();
            packet.WriteByte(OutPacket.CHARACTER_SELECTED)
                .WriteString(model.Name)
                .WriteInt(model.ObjectId)
                .WriteString(model.Title)
                .WriteInt(pkey2)
                .WriteInt(model.ClanId)
                .WriteInt(0)
                .WriteInt(model.Female ? 1 : 0)
                .WriteInt(model.Race)
                .WriteInt(model.CurrentClass)
                .WriteInt(1) // is active ?
                .WriteInt((int)model.x)
                .WriteInt((int)model.y)
                .WriteInt((int)model.z)
                .WriteDouble(model.CurrentHealth)
                .WriteDouble(model.CurrentMana)
                .WriteInt(model.Sp)
                .WriteLong(model.Exp)
                .WriteInt(model.Level)
                .WriteInt(model.Karma)
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

        public static byte[] BuildOutMoveToLocation(IMovable player,Vec2 target, int targetZ)
        {
            var origin = player.Origin;
            var movePacket = new WriteableBuffer();
            movePacket.WriteByte(OutPacket.MOVED_TO_LOCATION)
                .WriteInt(player.ObjectId)
                .WriteInt((int)target.x)
                .WriteInt((int)target.y)
                .WriteInt(targetZ)
                .WriteInt((int)origin.x)
                .WriteInt((int)origin.y)
                .WriteInt((int)player.OriginZ);
            return movePacket.toByteArray();
        }

        public static byte[] BuildCharSelectList(GameClient client
            , List<GameCharacterModel> characterList)
        {
            var packet = new WriteableBuffer();
            packet.WriteByte(OutPacket.CHARACTER_LIST_INFO)
                .WriteInt(characterList.Count); // characters count;

            foreach (var player in characterList)
            {
                var characterInfo = player;
                packet.WriteString(characterInfo.Name)
                    .WriteInt(characterInfo.ObjectId)
                    .WriteString(client.AccountName)
                    .WriteInt(client.Skeys.Play1)
                    .WriteInt(characterInfo.ClanId)
                    .WriteInt(0) // ?
                    .WriteInt(characterInfo.Female ? 1 : 0)
                    .WriteInt(characterInfo.Race)
                    .WriteInt(characterInfo.BaseClass)
                    .WriteInt(1)  // ?
                    .WriteInt(0).WriteInt(0).WriteInt(0) // xyz
                    .WriteDouble(characterInfo.CurrentHealth)
                    .WriteDouble(characterInfo.CurrentMana)
                    .WriteInt(characterInfo.Sp)
                    .WriteLong(characterInfo.Exp)
                    .WriteInt(characterInfo.Level)
                    .WriteInt(characterInfo.Karma);
                for (int i = 0; i < 9; i++)
                    packet.WriteInt(0);

                var itemIds = player.GearItemId;
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
                itemIds = player.GearObjectId;
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

                packet.WriteInt(characterInfo.HairStyle)
                    .WriteInt(characterInfo.HairColor)
                    .WriteInt(characterInfo.Face)
                    .WriteDouble(characterInfo.MaxHealth)
                    .WriteDouble(characterInfo.MaxMana)
                    .WriteInt(0) // deleting time
                    .WriteInt(characterInfo.CurrentClass)
                    .WriteInt(0) // is active
                    .WriteByte(0) // enchant effect
                    .WriteInt(0); // augmentation id
            }
            return packet.toByteArray();
        }
        public static byte[] BuildRelationChanged(ICharacter player)
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

        public static byte[] BuildCharInfo(ICharacter player)
        {
            var packet = new WriteableBuffer();
            var info = player.Info;
            packet.WriteByte(OutPacket.CHAR_INFO)
                .WriteInt((int)player.Origin.x)
                .WriteInt((int)player.Origin.y)
                .WriteInt((int)player.OriginZ)
                .WriteInt(0) // on boat
                .WriteInt(player.ObjectId)
                .WriteString(info.Name) //playerName
                .WriteInt(info.Race)
                .WriteInt(info.Female ? 1 : 0)
                .WriteInt(info.CurrentClass);
            var itemIds = player.GearItemId;
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

            var stats = player.Stats;
            packet.WriteInt(0) // pvp flag
                .WriteInt(0) //arma
                .WriteInt(stats.MAtkSpd)
                .WriteInt(stats.PAtkSpd)
                .WriteInt(0) // pvp flag
                .WriteInt(0) //karma
                .WriteInt(stats.RunSpd)
                .WriteInt(stats.WalkSpd)
                .WriteInt(stats.RunSpd) // swim
                .WriteInt(stats.WalkSpd) // swim
                .WriteInt(stats.RunSpd) // fly
                .WriteInt(stats.WalkSpd) // fly
                .WriteInt(stats.RunSpd) // fly
                .WriteInt(stats.WalkSpd) // fly
                .WriteDouble(1.0) // move speed multiplier
                .WriteDouble(1.0) // attack speed multiplier
                .WriteDouble(info.CollisionRadius) // coll radius
                .WriteDouble(info.CollisionHeight) // coll height
                .WriteInt(info.HairStyle)
                .WriteInt(info.HairColor)
                .WriteInt(info.Face)
                .WriteString(info.Title)
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
                .WriteInt(info.CurrentClass)
                .WriteInt(0) // cp cur
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

        public static byte[] BuildMyTargetSelected(ICharacter target)
        {
            var packet = new WriteableBuffer();
            packet.WriteByte(OutPacket.MY_TARGET_SELECTED).WriteInt(target.ObjectId).WriteShort(0);
            return packet.toByteArray();
        }

        public static byte[] BuildTargetSelected(ICharacter player)
        {
            var packet = new WriteableBuffer();
            packet.WriteByte(OutPacket.TARGET_SELECTED)
                .WriteInt(player.ObjectId)
                .WriteInt(1)
                .WriteDouble(player.Origin.x)
                .WriteDouble(player.Origin.y)
                .WriteDouble(player.OriginZ);

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

        public static byte[] BuildMockUserInfo(GameClient client, PlayerState player)
        {
            var packet = new WriteableBuffer();
            var character = player;
            var info = character.Info;
            var stats = character.Stats;
            packet.WriteByte(OutPacket.USER_INFO)

                .WriteInt((int)player.Origin.x)
                .WriteInt((int)player.Origin.y)
                .WriteInt((int)player.OriginZ)
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

            var itemIds = player. GearObjectId;
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

            itemIds = player.GearItemId;
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

            packet.WriteInt(stats.PAtk)
                .WriteInt(stats.PAtkSpd)
                .WriteInt(stats.PDef)
                .WriteInt(stats.Evasion)
                .WriteInt(stats.Accuracy)
                .WriteInt(stats.Crit)
                .WriteInt(stats.MAtk)
                .WriteInt(stats.MAtkSpd)
                .WriteInt(stats.PAtkSpd)
                .WriteInt(stats.MDef);

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
                .WriteDouble(info.CollisionRadius) // coll radius
                .WriteDouble(info.CollisionHeight); // coll height

            packet.WriteInt(info.HairStyle)
                .WriteInt(info.HairColor)
                .WriteInt(info.Face)
                .WriteInt(1) // is gm

                .WriteString(info.Title);

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

        public static byte[] BuildNetPing(GameClient client)
        {
            return new WriteableBuffer().WriteByte(OutPacket.NET_PING).WriteInt(145).toByteArray();
        }

        internal static byte[] BuildMoveToPawn(ICharacter player, ICharacter target, int distance)
        {
            var packet = new WriteableBuffer();
            packet.WriteByte(0x60)
                .WriteInt(player.ObjectId)
                .WriteInt(target.ObjectId)
                .WriteInt(distance)
                .WriteInt((int)player.Origin.x)
                .WriteInt((int)player.Origin.y)
                .WriteInt((int)player.OriginZ);

            return packet.toByteArray();
        }

        public static byte[] BuildStopMove(IMovable player)
        {
            var packet = new WriteableBuffer();
            packet.WriteByte(0x47)
                .WriteInt(player.ObjectId)
                .WriteInt((int)player.Origin.x)
                .WriteInt((int)player.Origin.y)
                .WriteInt((int)player.OriginZ)
                .WriteInt(player.Heading);// heading;

            return packet.toByteArray();
        }

        public static byte[] BuildAutoAttackStart(ICharacter state)
        {
            var packet = new WriteableBuffer();
            packet.WriteByte(0x2b).WriteInt(state.ObjectId);


            return packet.toByteArray();
        }

        public static byte[] BuildAttackResult(ICharacter attacker, ICharacter target, int damageAmount)
        {
            var packet = new WriteableBuffer();
            packet.WriteByte(0x05)
                .WriteInt(attacker.ObjectId)
                .WriteInt(target.ObjectId)
                .WriteInt(damageAmount)
                .WriteByte(0) // hit flags
                .WriteInt((int)attacker.Origin.x)
                .WriteInt((int)attacker.Origin.y)
                .WriteInt((int)attacker.OriginZ);
            packet.WriteShort(0);

            return packet.toByteArray();
        }

        public static byte[] BuildAutoAttackFinish(ICharacter character)
        {
            var packet = new WriteableBuffer();
            packet.WriteByte(0x2c).WriteInt(character.ObjectId);

            return packet.toByteArray();
        }

        public static byte[] BuildNewCharacterList(List<CharacterTemplate> startingTempaltes)
        {
            var packet = new WriteableBuffer();
            packet.WriteByte(0x17)
                .WriteInt(startingTempaltes.Count);
            foreach (var template  in startingTempaltes) 
            {
                packet.WriteInt(template.GetRace())
                    .WriteInt(template.ID)
                    .WriteInt(0x46)
                    .WriteInt(template.STR)
                    .WriteInt(0x0a)
                    .WriteInt(0x46)
                    .WriteInt(template.DEX)
                    .WriteInt(0x0a)
                    .WriteInt(0x46)
                    .WriteInt(template.CON)
                    .WriteInt(0x0a)
                    .WriteInt(0x46)
                    .WriteInt(template.INT)
                    .WriteInt(0x0a)
                    .WriteInt(0x46)
                    .WriteInt(template.WIT)
                    .WriteInt(0x0a)
                    .WriteInt(0x46)
                    .WriteInt(template.MEN)
                    .WriteInt(0x0a);

            }

            return packet.toByteArray();
        }

        public static byte[] BuildCharacterCreateOk()
        {
            var packet = new WriteableBuffer();
            packet.WriteByte(0x19).WriteInt(1);

            return packet.toByteArray();
        }

        public static byte[] BuildInventoryUpdate(List<ItemChangedDetails> itemList)
        {
            var packet = new WriteableBuffer();
            packet.WriteByte(0x27)
                .WriteShort(itemList.Count);

            foreach(var itemDetails in itemList)
            {
                var item = itemDetails.Item;
                packet.WriteShort(itemDetails.ChangeId)
                    .WriteShort(item.Type1)
                    .WriteInt(item.ObjectId)
                    .WriteInt(item.ItemId)
                    .WriteInt(item.Count)
                    .WriteShort(item.Type2)
                    .WriteShort(item.CustomType1)
                    .WriteShort(item.IsEquipped ? 1 : 0)
                    .WriteInt(item.Bodypart)
                    .WriteShort(item.EnchantLevel)
                    .WriteShort(item.CustomType2)
                    .WriteInt(item.AugmentationId)
                    .WriteInt(item.Mana);
            }

            return packet.toByteArray();
        }

        public static byte[] BuildInventoryList(List<NetworkItem> items, bool showWindow)
        {
            var packet = new WriteableBuffer();
            packet.WriteByte(0x1b)
                .WriteShort(showWindow ? 1 : 0)
                .WriteShort(items.Count);

            foreach (var item in items)
            {
                packet.WriteShort(item.Type1)
                      .WriteInt(item.ObjectId)
                      .WriteInt(item.ItemId)
                      .WriteInt(item.Count)
                      .WriteShort(item.Type2)
                      .WriteShort(item.CustomType1)
                      .WriteShort(item.IsEquipped ? 1 : 0)
                      .WriteInt(item.Bodypart)
                      .WriteShort(item.EnchantLevel)
                      .WriteShort(item.CustomType2)
                      .WriteInt(item.AugmentationId)
                      .WriteInt(item.Mana);
            }

            return packet.toByteArray();
        }
    }
}
