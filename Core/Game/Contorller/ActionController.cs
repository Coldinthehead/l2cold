using Core.Game.Network;
using Core.Game.Network.ClientPacket;
using Core.Math;
using Core.Utils.NetworkBuffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Game.Contorller
{
    public class ActionController : IPacketController
    {
        private readonly ActivePlayers _worldCharacters;

        public ActionController(ActivePlayers worldCharacters)
        {
            _worldCharacters = worldCharacters;
        }

        public void Run(GameClient client, ReadableBuffer message)
        {
            int objectId = message.ReadInt();
            var origin = new Vec2(message.ReadInt(), message.ReadInt());
            var originZ = message.ReadInt();
            int actionId = message.ReadByte();
            var character = _worldCharacters.FindById(objectId); 

            if (character != null)
            {
                client.Player.CharacterTarget = character;
                var packet = OutPacketFactory.BuildMyTargetSelected(character);
                client.SendData(packet);
            }
            else
                client.SendData(OutPacketFactory.BuildActionFailed());
        }
    }
}
