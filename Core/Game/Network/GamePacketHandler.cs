using Core.Common.Network;
using Core.Game.Network.Controller;
using Core.Utils.Logs;
using Core.Game.Network.Contorller;


namespace Core.Game.Network
{
    public class GamePacketHandler : IPacketHadnler<GameClient>
    {
        private static Logger<GamePacketHandler> _logger = Logger<GamePacketHandler>.BuildLogger();

        private Dictionary<int, IPacketController> _controllers;
        private readonly IPacketController _unknownPacketController;

        public GamePacketHandler(Dictionary<int, IPacketController> controllers)
        {
            _controllers = controllers;
            _unknownPacketController = new UnknownPacketController();
        }

        public void HandlePacket(GameClient client, ReadableBuffer message)
        {
            _logger.Log($"Recieveing data from [{client}]");
            int opCode = message.ReadByte();
            var controller = _controllers.GetValueOrDefault(opCode, _unknownPacketController);
            controller.Run(client, message);
        }
    }
}
