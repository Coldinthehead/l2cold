using Core.Common.Network;
using Core.Engine;
using Core.Game.Network.ClientPacket;
using Core.Game.Repository;
using Core.Game.Services;
using Core.Game.World.Factory;
using Core.Utils.Logs;

namespace Core.Game.Network.Controller
{
    public class CharacterSelectedController : IPacketController
    {
        private static Logger<CharacterSelectedController> _logger = Logger<CharacterSelectedController>.BuildLogger();
        private readonly PlayerFactory _playerFactory;
        private readonly CharacterService _characterService;

        public CharacterSelectedController(PlayerFactory playerFactory, CharacterService characterService)
        {
            _playerFactory = playerFactory;
            _characterService = characterService;
        }

        public void Run(GameClient client, ReadableBuffer message)
        {
            var charId = message.ReadInt();
            _logger.Log($"[CHARACTER_SELECTED] id:[{charId}] from:", client);
            var playerModel = _characterService.LoadSingleCharacter(client.AccountName, charId);

            client.SendData(OutPacketFactory.BuildSelectedCharacter(client.Skeys.Play2, playerModel));

            GameObject playerGameObject = _playerFactory.BuildPlayer(client, playerModel);

            client.Player = playerGameObject;
        }
    }
}
