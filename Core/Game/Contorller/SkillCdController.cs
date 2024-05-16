using Core.Game.Network;
using Core.Game.Network.ClientPacket;
using Core.Utils.NetworkBuffers;

namespace Core.Game.Contorller
{
    public class SkillCdController : IPacketController
    {
        public void Run(GameClient client, ReadableBuffer message)
        {
            client.SendData(OutPacketFactory.BuildSkillCd());
        }

       
    }
}
