using Core.Game.Network;
using Core.Logs;
using Core.Utils.NetworkBuffers;
using Core.Game.Network.ClientPacket;
using Core.Game.World.Actor;

namespace Core.Game.Contorller
{
    public class EnterWorldController : IPacketController
    {
        private static Logger<EnterWorldController> _logger = Logger<EnterWorldController>.BuildLogger();
        private readonly ActivePlayers _players;

        public EnterWorldController(ActivePlayers players)
        {
            _players = players;
        }

        public void Run(GameClient client, ReadableBuffer message)
        {
            _logger.Log($"[ENTER_WORLD] received from :", client);
            var player = client.Player;
            var character = player;
            client.SendData(OutPacketFactory.BuildMockUserInfo(client, player));
            client.SendData(OutPacketFactory.BuildChangeMoveType(character.Info.ObjectId));
            client.SendData(OutPacketFactory.BuildQuestList());
            client.SendData(OutPacketFactory.BuildMagicIconEffects());
            client.SendData(OutPacketFactory.BuildETCStatusUpdate());
            client.SendData(OutPacketFactory.BuildHennaInfo());
            client.SendData(OutPacketFactory.BuildFriendList());
            client.SendData(OutPacketFactory.BuildItemList());
            client.SendData(OutPacketFactory.BuildShortcutInit());
            client.SendData(OutPacketFactory.BuildExStorageMaxCount());
            client.SendData(OutPacketFactory.BuildMacroList());
            client.SendData(OutPacketFactory.BuildClientTime());
            client.SendData(OutPacketFactory.BuildSkillList());
            client.SendData(OutPacketFactory.BuildTargetSelected(player));
            client.SendData(OutPacketFactory.BuildSetCompasZone());
            client.SendData(OutPacketFactory.BuildActionFailed());

            SendNetPingPacket(client);
            InformClientsWithPlayer(player);
            InformClientWithPlayers(client);
            _players.AddPlayer(client, player);
        }

        private void SendNetPingPacket(GameClient client)
        {
            var pingPacket = OutPacketFactory.BuildNetPing(client);
            client.SendData(pingPacket);
        }

        private void InformClientsWithPlayer(Player player)
        {
            _players.BroadcastPacket(OutPacketFactory.BuildCharInfo(player));
            _players.BroadcastPacket(OutPacketFactory.BuildRelationChanged(player));
        }

        private void InformClientWithPlayers(GameClient client)
        {
            foreach (var player in _players.GetOnlinePlayers())
            {
                client.SendData(OutPacketFactory.BuildCharInfo(player));
                client.SendData(OutPacketFactory.BuildRelationChanged(player));
                if (player.IsMoving)
                {
                    client.SendData(OutPacketFactory.BuildOutMoveToLocation(
                        player, player.Target, (int)player.TargetZ));
                }
            }
        }
    }
}
