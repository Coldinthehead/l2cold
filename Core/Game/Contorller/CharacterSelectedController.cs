using Core.Game.Data;
using Core.Game.Network;
using Core.Game.Network.ClientPacket;
using Core.Logs;
using Core.Utils.NetworkBuffers;

namespace Core.Game.Contorller
{
    public class CharacterSelectedController : IPacketController
    {
        private static Logger<CharacterSelectedController> _logger = Logger<CharacterSelectedController>.BuildLogger();
        private readonly PlayerRepository _playerRepository;

        public CharacterSelectedController(PlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public void Run(GameClient client, ReadableBuffer message)
        {
            var charId = message.ReadInt();
            _logger.Log($"[CHARACTER_SELECTED] id:[{charId}] from:", client);
            var player = _playerRepository.LoadCharacter(charId);
            client.SendData(OutPacketFactory.BuildSelectedCharacter(client.Skeys.Play2, player));
            client.Player = player;
        }
    }
}
