
namespace Core.Game.Network.ClientPacket
{
    public static class InPacket
    {
        public const int PROTOCOL_VERISION = 0x00;
        public const int REQUEST_AUTHENTICATION = 0x08;
        public const int CHARACTER_SELECTED = 0x0D;
        public const int EX_PACKET = 0xD0;
        public const int ENTER_WORLD = 0x03;   
    }
}
