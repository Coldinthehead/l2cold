using Core.Common.Network;
using Core.Game.Network.ClientPacket;
using Core.Game.Network.Controller;
using Core.Game.World.Components;


namespace Core.Game.Network.Contorller
{
    internal class RequestItemListController : IPacketController
    {
        public void Run(GameClient client, ReadableBuffer message)
        {
            var inventory = client.Player.GetComponent<PlayerInventory>();
            var items = inventory.GetSerializable();
            client.SendData(OutPacketFactory.BuildInventoryList(items, true));
        }
    }
}
