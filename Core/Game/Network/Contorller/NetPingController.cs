using Core.Common.Network;
using Core.Game.Network;
using Core.Utils.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Game.Network.Controller
{
    public class NetPingController : IPacketController
    {
        private static Logger<NetPingController> _logger = Logger<NetPingController>.BuildLogger();
        public void Run(GameClient client, ReadableBuffer message)
        {
            var requestCode = message.ReadInt();
            var ping = message.ReadInt();
            client.Ping = ping;
            _logger.Log($"Client ping : {ping}");
        }
    }
}
