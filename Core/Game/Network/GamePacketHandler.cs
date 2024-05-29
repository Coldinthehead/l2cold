using Core.Common.Network;
using Core.Common.Services;
using Core.Game.Network.Controller;
using Core.Game.Network.ClientPacket;
using Core.Game.Repository;
using Core.Game.Services;
using Core.Game.World;
using Core.Game.World.Factory;
using Core.Utils;
using Core.Utils.Logs;
using Core.Game.Network.Contorller;


namespace Core.Game.Network
{
    public class GamePacketHandler : IPacketHadnler<GameClient>
    {
        private static Logger<GamePacketHandler> _logger = Logger<GamePacketHandler>.BuildLogger();

        private readonly LoginServerService _loginServer;
        private readonly ActivePlayers _worldPlayers;
        private readonly PlayerFactory _playerFactory;
        private readonly PlayerTempaltesRepository _templates;
        private readonly CharacterService _characterService;
        private Dictionary<int, IPacketController> _controllers;
        private readonly IPacketController _unknownPacketController;

        public GamePacketHandler(LoginServerService loginServer
            , ActivePlayers worldPlayers
            , PlayerFactory playerFactory
            , PlayerTempaltesRepository templates
            , CharacterService characterService
            , Dictionary<int, IPacketController> controllers)
        {
            _loginServer = loginServer;
            _worldPlayers = worldPlayers;
            _playerFactory = playerFactory;
            _templates = templates;
            _characterService = characterService;
            _controllers = controllers;
            _unknownPacketController = new UnknownPacketController();
        }

        public void HandlePacket(GameClient client, ReadableBuffer message)
        {
            _logger.Log($"Recieveing data from [{client}]");
            int opCode = message.ReadByte();
            var controller = _controllers.GetValueOrDefault(opCode, _unknownPacketController);
            controller.Run(client, message);
          /*  switch (opCode)
            {
                case InPacket.PROTOCOL_VERISION:
                    new ProtocolVersionController().Run(client, message);
                    break;
                case InPacket.REQUEST_AUTHENTICATION:
                    new RequestAuthController(_loginServer, _characterService).Run(client, message);
                    break;
                case InPacket.CHARACTER_SELECTED:
                    new CharacterSelectedController(_playerFactory, _characterService).Run(client, message);
                    break;
                case InPacket.EX_PACKET:
                    _logger.Log($"[EX_PACKET] received from :", client);
                    break;
                case InPacket.ENTER_WORLD:
                    new EnterWorldController(_worldPlayers).Run(client, message);
                    break;
                case InPacket.CHARACTER_MOVE_TO_LOCATION:
                    new CharMoveController().Run(client, message);
                    break;
                case InPacket.VALIDATE_POSITION:
                    new ValidatePositionController(_worldPlayers).Run(client, message);
                    break;
                case InPacket.REQUEST_SKILL_CD:
                    new SkillCdController().Run(client, message);
                    break;
                case InPacket.ACTION:
                    new ActionController(_worldPlayers).Run(client, message);
                    break;
                case 0xA8:
                    new NetPingController().Run(client, message);
                    break;
                case 0x0A:
                    new AttackRequestController(_worldPlayers).Run(client, message);
                    break;
                case 0x38:
                    new SayController().Run(client, message);
                    break;
                case 0x0e:
                    new CharacterCreateController(_templates).Run(client, message);
                    break;
                case 0x0b:
                    new NewCharacterController(_characterService).Run(client, message);
                    break;
                default:
                    _logger.Log($"Unknown opcode [{opCode.ToHex()}] from [{client}]");
                    break;*/
          
        }
    }
}
