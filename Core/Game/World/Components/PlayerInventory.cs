using Core.Engine;
using Core.Game.Data;
using Core.Game.Network.ClientPacket;
using Core.Game.World.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Core.Game.World.Components
{
    public class PlayerInventory : Component
    {
        private PlayerNetwork _network;
        private List<ItemInstance> _allItems;

        public PlayerInventory()
        {
            _allItems = new List<ItemInstance>();
        }

        public override void OnStart()
        {
            _network = gameObject.GetComponent<PlayerNetwork>(); 
        }

        public void AddAll(List<ItemInstance> items)
        {
            List<ItemChangedDetails> itemDetails = new List<ItemChangedDetails>();
            foreach (var item in items)
            {
                _allItems.Add(item);
                var change = (int)Constants.ItemChangeType.Added;
                var detail = new ItemChangedDetails(change, item.GetSerizlized());
                itemDetails.Add(detail);
            }
            _network.SendPersonalPacket(OutPacketFactory.BuildInventoryUpdate(itemDetails));
        }

        public List<NetworkItem> GetSerializable()
        {
            var res = new List<NetworkItem>();
            foreach (var item in  _allItems)
            {
                res.Add(item.GetSerizlized());
            }
            return res;
        }
    }
}
