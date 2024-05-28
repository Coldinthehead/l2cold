using Core.Common.Network;
using Core.Common.Security;
using Core.Common.Services;
using Core.Game.Network.ClientPacket;
using Core.Game.Services;
using Core.Utils.Logs;


namespace Core.Game.Network.Controller
{
    internal class RequestAuthController : IPacketController
    {
        private static Logger<RequestAuthController> _logger = Logger<RequestAuthController>.BuildLogger();

        private readonly LoginServerService _loginServer;
        private readonly CharacterService _characterService;
        

        public RequestAuthController(LoginServerService logginServer, CharacterService chracterService)
        {
            _loginServer = logginServer;
            _characterService = chracterService;
        }

        public void Run(GameClient client, ReadableBuffer message)
        {
            _logger.Log($"Received [REQUEST_AUTH] from [{client}]");
            var accId = message.ReadString();
            var sessionKeys = ReadSessinKeys(message);
            var accDetails = new LSAccountDetails(accId, sessionKeys);
            if (_loginServer.IsAccountLoggedIn(accDetails))
            {
                client.AccountName = accId;
                client.SetSessionKeys(sessionKeys);
                var savedCharacter = _characterService.LoadCharacterList(client.AccountName);
                client.SendData(OutPacketFactory.BuildCharSelectList(client, savedCharacter));
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
