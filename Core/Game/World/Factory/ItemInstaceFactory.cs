using Core.Game.Repository;
using Core.Game.Services;
using Core.Game.World.Items;

namespace Core.Game.World.Factory
{
    public class ItemInstaceFactory
    {
        private readonly ObjectIdFactory _idFactory;
        private readonly ItemTemplatesRepository _itemTempaltes;
        public ItemInstaceFactory(ObjectIdFactory idFactory, ItemTemplatesRepository itemTempaltes)
        {
            _idFactory = idFactory;
            _itemTempaltes = itemTempaltes;
        }

        internal List<ItemInstance> CreateSomeWeapons()
        {
            var res = _itemTempaltes.GetAllWeaponTempaltes()
                .Select((x) => new ItemInstance(_idFactory.GetFreeId(), x)).ToList();


            return res;
        }
    }
}
