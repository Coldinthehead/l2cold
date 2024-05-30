using Core.Common.Network;
using Core.Game.Network.Controller;
using Core.Game.World.Components;
using Core.Utils.Logs;


namespace Core.Game.Network.Contorller
{
    public class UseItemController : IPacketController
    {
        public static readonly Logger<UseItemController> _logger = Logger<UseItemController>.BuildLogger();

        public void Run(GameClient client, ReadableBuffer message)
        {
            var itemId = message.ReadInt();
            _logger.Log($"Use item {itemId}");
            var inventory = client.Player.GetComponent<PlayerInventory>();
            var item = inventory.GetItemById(itemId);
            if (item != null)
            {
                item.OnUse(inventory);
            }
        }
    }
}
