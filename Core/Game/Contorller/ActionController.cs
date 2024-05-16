using Core.Game.Network;
using Core.Game.Network.ClientPacket;
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
        public void Run(GameClient client, ReadableBuffer message)
        {
            client.SendData(OutPacketFactory.BuildActionFailed());
        }
    }
}
