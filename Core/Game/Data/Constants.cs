using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Game.Data
{

    public static class Constants
    {
        public enum ItemType1
        {
            WeaponJewel = 0,
            ShieldArmour = 1,
            QuestItem = 4,
        }

        public enum ItemType2
        {
            Weapon = 0,
            ShieldArmour = 1,
            Jewel = 2,
            QuestItem = 3,
            Money = 4, 
            Other = 5,
        }

        public enum ItemChangeType
        {
            None = 0,
            Added = 1,
            Modify = 2,
            Remove = 3
        }
    }
}
