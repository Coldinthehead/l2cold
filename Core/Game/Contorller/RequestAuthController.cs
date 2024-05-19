using Core.Common.Services;
using Core.Game.Network;
using Core.Game.Network.ClientPacket;
using Core.Logs;
using Core.Security;
using Core.Utils.NetworkBuffers;


namespace Core.Game.Contorller
{
    internal class RequestAuthController : IPacketController
    {
        private static Logger<RequestAuthController> _logger = Logger<RequestAuthController>.BuildLogger();

        private readonly LoginServerService _loginServer;
        private readonly PlayerRepository _characterRepository;

        public RequestAuthController(LoginServerService logginServer, PlayerRepository characterRepository)
        {
            _loginServer = logginServer;
            _characterRepository = characterRepository;
        }

        public void Run(GameClient client, ReadableBuffer message)
        {
            _logger.Log($"Received [REQUEST_AUTH] from [{client}]");
            var accId = message.ReadString();
            var sessionKeys = ReadSessinKeys(message);
            var accDetails = new LSAccountDetails(accId, sessionKeys);
            if (_loginServer.IsAccountLoggedIn(accDetails))
            {
                var savedCharacter = _characterRepository.LoadCharacterList();
                client.SendData(OutPacketFactory.BuildCharSelectInfo(accDetails, savedCharacter));
                client.SetSessionKeys(sessionKeys);
            }
        }

        private static SessionKeys ReadSessinKeys(ReadableBuffer message)
        {
            int playKey2 = message.ReadInt();
            int playKey1 = message.ReadInt();
            int login1 = message.ReadInt();
            int login2 = message.ReadInt();
            return SessionKeys.FromValues(playKey1, playKey2, login1, login2);
        }
    }
}
