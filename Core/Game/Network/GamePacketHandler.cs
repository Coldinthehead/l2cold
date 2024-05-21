using Core.Common.Network;
using Core.Common.Services;
using Core.Game.Contorller;
using Core.Game.Network.ClientPacket;
using Core.Game.Repository;
using Core.Game.Services;
using Core.Game.World;
using Core.Utils;
using Core.Utils.Logs;


namespace Core.Game.Network
{
    public class GamePacketHandler : IPacketHadnler<GameClient>
    {
        private static Logger<GamePacketHandler> _logger = Logger<GamePacketHandler>.BuildLogger();

        private readonly LoginServerService _loginServer;
        private readonly ActivePlayers _worldPlayers;
        private readonly ObjectIdFactory _idFactory = new();
        private readonly PlayerRepository _characterRepository;

        public GamePacketHandler(LoginServerService loginServer
            , ActivePlayers worldPlayers
            , ObjectIdFactory idFactory
            , PlayerRepository playerRepository)
        {
            _loginServer = loginServer;
            _worldPlayers = worldPlayers;
            _idFactory = idFactory;
            _characterRepository = playerRepository;
        }

        public void HandlePacket(GameClient client, ReadableBuffer message)
        {
            _logger.Log($"Recieveing data from [{client}]");
            int opCode = message.ReadByte();
            switch (opCode)
            {
                case InPacket.PROTOCOL_VERISION:
                    new ProtocolVersionController().Run(client, message);
                    break;
                case InPacket.REQUEST_AUTHENTICATION:
                    new RequestAuthController(_loginServer, _characterRepository).Run(client, message);
                    break;
                case InPacket.CHARACTER_SELECTED:
                    new CharacterSelectedController(_characterRepository).Run(client, message);
                    break;
                case InPacket.EX_PACKET:
                    _logger.Log($"[EX_PACKET] received from :", client);
                    break;
                case InPacket.ENTER_WORLD:
                    new EnterWorldController(_worldPlayers).Run(client, message);
                    break;
                case InPacket.CHARACTER_MOVE_TO_LOCATION:
                    new CharMoveController(_worldPlayers, _idFactory).Run(client, message);
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
                default:
                    _logger.Log($"Unknown opcode [{opCode.ToHex()}] from [{client}]");
                    break;
            }
        }
    }
}
