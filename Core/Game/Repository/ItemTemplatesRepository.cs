
using Core.Game.Data.Static.Items;

namespace Core.Game.Repository
{
    public class ItemTemplatesRepository
    {
        private readonly ItemTemplateFactory _itemFactory;

        private Dictionary<int, WeaponTemplate> _weapons;

        public ItemTemplatesRepository(ItemTemplateFactory itemFactory)
        {
            _itemFactory = itemFactory;
            _weapons = new();
            LoadData();
        }

        public void LoadData()
        {
            var weaponTempaltes = _itemFactory.LoadWeapons();
            foreach (var v in weaponTempaltes) 
            {
                _weapons[v.ID] = v;
            }
        }

        public List<WeaponTemplate> GetAllWeaponTempaltes()
        {
            return _weapons.Values.ToList();
        }
    }
}
