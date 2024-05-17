using Core.Game.Network;
using Core.Game.Network.ClientPacket;
using Core.Logs;
using Core.Math;
using Core.Utils.NetworkBuffers;

namespace Core.Game.Contorller
{
    public class CharMoveController : IPacketController
    {
        private static Logger<CharMoveController> _logger = Logger<CharMoveController>.BuildLogger();
        private readonly ActivePlayers _players;
        private readonly ObjectIdFactory _idFactory;

        public CharMoveController(ActivePlayers players, ObjectIdFactory idFactory)
        {
            _players = players;
            _idFactory = idFactory;
        }

        public void Run(GameClient client, ReadableBuffer message)
        {
            var target = new Vec2(message.ReadInt(),  message.ReadInt());
            var targetZ = message.ReadInt();
            var origin = new Vec2(message.ReadInt(), message.ReadInt());
            var originZ = message.ReadInt();
            int moveType = message.ReadInt();
            _logger.Log($"Move from {origin} , to {target}");

            var player = _players.GetPlayer(client);
            player.UpdatePosition(origin, originZ);
            player.Move(target, targetZ);
            var packet = OutPacketFactory.BuildOutMoveToLocation(client, player, target, targetZ);
            /*client.SendData(packet);*/
            _players.BroadcastPacket(packet);

            var clientDistance = Vec2.Distance(origin, client.Player.CurrentTarget);
            var moveSpeed = player.CharacterDetails.Stats.RunSpd * 50 * 0.001f;
            var pingTime = client.Ping * 0.001f;
            var serverDistance = Vec2.Distance(client.Player.ServerPosition, client.Player.CurrentTarget);
           
        }


 

        
    }
}