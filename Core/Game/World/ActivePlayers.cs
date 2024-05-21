using Core.Game.Network;
using Core.Game.Network.ClientPacket;
using Core.Game.World.Actor;
using Core.Utils.Math;

namespace Core.Game.World
{
    public class ActivePlayers
    {
        private Dictionary<GameClient, Player> _onlinePlayers = new();
        private List<ICharacter> _activePlayers = new();


        public void Tick(float dt)
        {
            foreach (var p in _activePlayers)
            {
                p.Update(dt);
            }
        }

        public void AddPlayer(GameClient client, Player player)
        {
            _onlinePlayers[client] = player;
            _activePlayers.Add(player);
        }

        public Player GetPlayer(GameClient client)
        {
            return _onlinePlayers[client];
        }

        public void BroadcastPacket(byte[] packet)
        {
            foreach (var client in _onlinePlayers.Keys)
            {
                client.SendData(packet);
            }
        }

        public void BroadcastMoveToLocation(Player player, Vec2 target, float zTarget)
        {
            foreach (var client in _onlinePlayers.Keys)
            {
                var packet = OutPacketFactory.BuildOutMoveToLocation(player, target, (int)zTarget);
                client.SendData(packet);
            }
        }

        public IEnumerable<ICharacter> GetOnlinePlayers()
        {
            return _activePlayers;
        }


        public void AddGhost(GhostPlayer ghostPlayer)
        {
            _activePlayers.Add(ghostPlayer);
            ghostPlayer.OnMovement += OnGhostMovement;
        }

        private void OnGhostMovement(IMovable character)
        {
            var packet = OutPacketFactory.BuildOutMoveToLocation(character, character.Target, (int)character.TargetZ);
            BroadcastPacket(packet);
        }

        public ICharacter FindById(int objectId)
        {
            foreach (var character in _activePlayers)
            {
                if (character.ObjectId == objectId)
                {
                    return character;
                }
            }
            return null;
        }
    }
}
