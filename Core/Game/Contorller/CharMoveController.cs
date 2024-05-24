﻿using Core.Common.Network;
using Core.Game.Network;
using Core.Game.Network.ClientPacket;
using Core.Game.Services;
using Core.Game.World;
using Core.Utils.Math;
using Core.Utils.Logs;
using Core.Game.World.Components;

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

            var behaviour = client.Player.GetComponent<PlayerBehaviour>();
            behaviour.Move(target, targetZ);
         /*   var player = _players.GetPlayer(client);
            player.UpdateClientPosition(origin, originZ);
            player.Move(target, targetZ);
            var packet = OutPacketFactory.BuildOutMoveToLocation(player, target, targetZ);
            _players.BroadcastPacket(packet);*/
           
        }


 

        
    }
}