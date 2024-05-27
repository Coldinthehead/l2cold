using Core.Common.Network;
using Core.Game.Network;
using Core.Game.Network.ClientPacket;
using Core.Game.World;
using Core.Utils.Math;
using Core.Utils.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Game.World.Actor;
using Core.Game.World.Components;

namespace Core.Game.Network.Controller
{
    public class AttackRequestController : IPacketController
    {
        private static Logger<AttackRequestController> _logger = Logger<AttackRequestController>.BuildLogger();

        private readonly ActivePlayers _worldPlayers;

        public AttackRequestController(ActivePlayers worldPlayers)
        {
            _worldPlayers = worldPlayers;
        }

        public void Run(GameClient client, ReadableBuffer message)
        {
            _logger.Log("Handle attack");
            int objId = message.ReadInt();
            var targetPos = new Vec2(message.ReadInt(), message.ReadInt());
            var targetZ = message.ReadInt();
            int attackId = message.ReadByte();

            /*var target = client.Player.CharacterTarget;*/
            ICharacter target = _worldPlayers.FindById(objId);
            if (target == null)
            {
                client.SendData(OutPacketFactory.BuildActionFailed());
                return;
            }
            Console.WriteLine("Process attack");
            var player = client.Player.GetComponent<PlayerBehaviour>();
            player.Attack(target);
/*
            var distance = 50;
            client.Player.StartFollowTarget(target, distance);
            var packet = OutPacketFactory.BuildMoveToPawn(client.Player, target, distance);
            _worldPlayers.BroadcastPacket(packet);*/
            
        }
    }
}
