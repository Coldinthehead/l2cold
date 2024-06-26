﻿using Core.Common.Network;
using Core.Game.Network.Controller;
using Core.Utils.Logs;

namespace Core.Game.Network.Contorller
{
    internal class UnknownPacketController : IPacketController
    {
        private Logger<UnknownPacketController> _logger = new Logger<UnknownPacketController>();
        public void Run(GameClient client, ReadableBuffer message)
        {
            _logger.Log("Unknown packet from client" + client);
        }
    }
}
