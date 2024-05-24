using Core.Common.Network;
using Core.Engine;
using Core.Game.Network;
using Core.Game.Network.ClientPacket;
using Core.Game.Repository;
using Core.Game.World.Factory;
using Core.Utils.Logs;

namespace Core.Game.Contorller
{
    public class CharacterSelectedController : IPacketController
    {
        private static Logger<CharacterSelectedController> _logger = Logger<CharacterSelectedController>.BuildLogger();
        private readonly PlayerRepository _playerRepository;
        private readonly PlayerFactory _playerFactory;

        public CharacterSelectedController(PlayerRepository playerRepository, PlayerFactory playerFactory)
        {
            _playerRepository = playerRepository;
            _playerFactory = playerFactory;
        }

        public void Run(GameClient client, ReadableBuffer message)
        {
            var charId = message.ReadInt();
            _logger.Log($"[CHARACTER_SELECTED] id:[{charId}] from:", client);
            var playerModel = _playerRepository.LoadCharacter(charId);
            client.SendData(OutPacketFactory.BuildSelectedCharacter(client.Skeys.Play2, playerModel));

            GameObject playerGameObject = _playerFactory.BuildPlayer(client, playerModel);

            client.Player = playerGameObject;
        }
    }
}
