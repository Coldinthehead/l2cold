using Core.Engine;
using Core.Game.Data;
using Core.Game.Data.Static.Items;
using Core.Game.Network.ClientPacket;
using Core.Game.World.Items;
using System.Formats.Asn1;


namespace Core.Game.World.Components
{
    public class NullItem : ItemInstance
    {
        public override int ItemId => 0;
        public NullItem(int objectId, WeaponTemplate template) : base(objectId, template)
        {
        }
    }

    public class PlayerInventory : Component
    {
        private PlayerNetwork _network;
        private PlayerState _playerState;
        private List<ItemInstance> _allItems;

        public ItemInstance RightHand { get; private set; }

        private readonly static NullItem _nullItem = new NullItem(0, null);

        public PlayerInventory()
        {
            _allItems = new List<ItemInstance>();
            RightHand = _nullItem;
        }

        public override void OnStart()
        {
            _network = gameObject.GetComponent<PlayerNetwork>(); 
            _playerState = gameObject.GetComponent<PlayerState>();
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

        public ItemInstance GetItemById(int itemId)
        {
            foreach (var item in _allItems)
            {
                if (item.ObjectId == itemId)
                    return item;
            }
            return null;
        }

        public void EquipWeapon(ItemInstance itemInstance)
        {
            var changeId = (int)Constants.ItemChangeType.Modify;
            var updateList = new List<ItemChangedDetails>();
            if (RightHand.ItemId != 0)
            {
                RightHand.OnDeequip(this);
                updateList.Add(new ItemChangedDetails(changeId, RightHand.GetSerizlized()));
            }
            
            if (RightHand.ItemId != itemInstance.ItemId) 
            {
                RightHand = itemInstance;
                itemInstance.OnEquip(this);

                updateList.Add(new ItemChangedDetails(changeId, itemInstance.GetSerizlized()));
            }

           
            _network.SendPersonalPacket(OutPacketFactory.BuildUserInfo(_playerState));
            _network.SendPersonalPacket(OutPacketFactory.BuildInventoryUpdate(updateList));
        }

        public void DeequipWeapon(ItemInstance itemInstace)
        {
            var changeId = (int)Constants.ItemChangeType.Modify;
            var updateList = new List<ItemChangedDetails>();

            itemInstace.OnDeequip(this);
            updateList.Add(new ItemChangedDetails(changeId, itemInstace.GetSerizlized()));
            RightHand = _nullItem;

            _network.SendPersonalPacket(OutPacketFactory.BuildUserInfo(_playerState));
            _network.BroadcastPacket(OutPacketFactory.BuildCharInfo(_playerState));
            _network.SendPersonalPacket(OutPacketFactory.BuildInventoryUpdate(updateList));
        }
    }
}
