using Core.Game.Data;
using Core.Game.Data.Static.Items;
using Core.Game.World.Components;

namespace Core.Game.World.Items
{
    public class ItemInstance
    {
        public readonly int ObjectId;
        private readonly WeaponTemplate _template;

        private bool Equeppied;
        private int EnchantLevel;

        public virtual int ItemId => _template.ID;

        public ItemInstance(int objectId, WeaponTemplate template)
        {
            ObjectId = objectId;
            _template = template;
        }

        public virtual NetworkItem GetSerizlized()
        {
            var item = new NetworkItem();
            item.ObjectId = ObjectId;
            item.Type1 = (int)Constants.ItemType1.WeaponJewel;
            item.ItemId = _template.ID;
            item.Type2 = (int)Constants.ItemType2.Weapon;
            item.CustomType1 = 0;
            item.IsEquipped = Equeppied;
            item.EnchantLevel = EnchantLevel;
            item.AugmentationId = 0;
            item.Mana = -1;
            item.Bodypart = (int)Constants.Bodypart.RightHand;
            return item;
        }

        public virtual void OnUse(PlayerInventory inventory)
        {
            Console.WriteLine("on use item" + ObjectId);
            if (Equeppied)
            {
                inventory.DeequipWeapon(this);
            }
            else
            {
                inventory.EquipWeapon(this);
            }
        }

        public virtual void OnEquip(PlayerInventory playerInventory)
        {
            Equeppied = true;
        }

        public virtual void OnDeequip(PlayerInventory playerInventory)
        {
            Equeppied = false;
        }
    }
}
