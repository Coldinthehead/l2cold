using Core.Game.Network;
using Core.Logs;
using Core.Math;
using Core.Utils.NetworkBuffers;
using System.Numerics;

namespace Core.Game.Contorller
{
    public class CharMoveController : IPacketController
    {
        private static Logger<CharMoveController> _logger = Logger<CharMoveController>.BuildLogger();
        private readonly ActivePlayers _players;

        public CharMoveController(ActivePlayers players)
        {
            _players = players;
        }

        public void Run(GameClient client, ReadableBuffer message)
        {
            var target = new Vec2(message.ReadInt(),  message.ReadInt());
            var targetZ = message.ReadInt();
            var origin = new Vec2(message.ReadInt(), message.ReadInt());
            var originZ = message.ReadInt();
            int moveType = message.ReadInt();
            _logger.Log($"Move from {origin} , to {target}");

            client.SendData(BuildOutMoveToLocation(client, _players.GetPlayer(client), origin, originZ, target, targetZ)) ;
        }

        private byte[] BuildOutMoveToLocation(GameClient client,Player player, Vec2 from, int zFrom, Vec2 target, int targetZ)
        {
            var movePacket = new WriteableBuffer();
            movePacket.WriteByte(0x01)
                .WriteInt(player.ObjectId)
                .WriteInt((int)target.x)
                .WriteInt((int)target.y)
                .WriteInt(targetZ)
                .WriteInt((int)from.x)
                .WriteInt((int)from.y)
                .WriteInt(zFrom);


            return movePacket.toByteArray();
        }

        
    }
}