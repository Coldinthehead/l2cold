using Core.Common.Network;
using Core.Game.Network;
using Core.Game.Network.ClientPacket;

namespace Core.Game.Network.Controller
{
    public class SkillCdController : IPacketController
    {
        public void Run(GameClient client, ReadableBuffer message)
        {
            client.SendData(OutPacketFactory.BuildSkillCd());
        }
    }
}
