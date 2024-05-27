
using Core.Game.World.Actor;
using Core.Game.World.Components;

namespace Core.Game.Network.ClientPacket
{
    public static class OutPacket
    {
        public const int CRYPT_INIT = 0x00;
        public const int MOVED_TO_LOCATION = 0x01;
        public const int CHAR_INFO = 0x03;
        public const int USER_INFO = 0x04;
        public const int CHARACTER_LIST_INFO = 0x13;
        public const int QUEST_LIST = 0x80;
        public const int MAGIC_EFFECT_ICONS = 0x7F;
        public const int FRIEND_LIST = 0xFA;
        public const int ITEM_LIST = 0x1B;
        public const int SHORTCUT_INIT = 0x45;
        public const int SKILL_LIST = 0x58;
        public const int SET_COMPAS_ZONE = 0xFE;
        public const int ACTION_FAILED = 0x25;
        public const int TARGET_SELECTED = 0x29;
        public const int SET_CLIENT_TIME = 0xEC;
        public const int MACRO_LIST = 0xE7;
        public const int HENNA_INFO = 0xE4;
        public const int ETC_STATUS_UPDATE = 0xF3;
        public const int CHANGE_MOVE_TYPE = 0x2E;
        public const int CHARACTER_SELECTED = 0x15;
        public const int SKILL_CD = 0xC1;
        public const int RELATION_CHANGED = 0xCE;
        public const int NET_PING = 0xD3;
        public const int MY_TARGET_SELECTED = 0xA6;

      
    }
}
