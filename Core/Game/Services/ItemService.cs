using Core.Game.World.Factory;
using Core.Game.World.Items;


namespace Core.Game.Services
{
    public class ItemService
    {
        private readonly ItemInstaceFactory _factory;

        public ItemService(ItemInstaceFactory factory)
        {
            _factory = factory;
        }

        public List<ItemInstance> CreateItems()
        {
            return _factory.CreateSomeWeapons();
        }
    }
}
