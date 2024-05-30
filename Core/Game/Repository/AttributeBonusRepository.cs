
using Core.Game.Data.Static;

namespace Core.Game.Repository
{
    public class AttributeBonusRepository
    {
        private readonly AttributeTableFactory _attributeFactory;

        private Dictionary<string, AttributeTable> _attributes;

        public AttributeBonusRepository(AttributeTableFactory attributeFactory)
        {
            _attributeFactory = attributeFactory;
            _attributes = _attributeFactory.Load();
        }

        public float GetStrBonus(int amount)
        {
            return GetBonus("STR", amount);
        }
        public float GetDexBonus(int amount)
        {
            return GetBonus("DEX", amount);
        } 
        public float GetConBonus(int amount)
        {
            return GetBonus("CON", amount);
        } 
        public float GetIntBonus(int amount)
        {
            return GetBonus("INT", amount);
        } 
        public float GetMenBonus(int amount)
        {
            return GetBonus("MEN", amount);
        } 
        public float GetWitBonus(int amount)
        {
            return GetBonus("WIT", amount);
        }

        private float GetBonus(string key, int amount)
        {
            return _attributes[key][amount];
        }
    }
}
