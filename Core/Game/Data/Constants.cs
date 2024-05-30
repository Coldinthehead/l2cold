using static Core.Game.Data.Constants;

namespace Core.Game.Data
{
    public static class EnumsExt
    {
        public static int AsInt(this Constants.Bodypart input)
        {
            return (int)input;
        }

        public static Constants.Bodypart ParseAsBodypart(this string str)
        {
            switch (str.ToLower())
            {
                case "rhand":
                    return Bodypart.RightHand;
                case "lhand":
                    return Bodypart.LeftHand;
                case "lrhand":
                    return Bodypart.TwoHand;
                case "chest":
                    return Bodypart.Chest;
                case "gloves":
                    return Bodypart.Gloves;
                case "legs":
                    return Bodypart.Legs;
                case "feet":
                    return Bodypart.Boots;
                case "head":
                    return Bodypart.Head;
                case "fullarmor":
                    return Bodypart.FullArmor;
                case "rear,lear":
                    return Bodypart.RightEarring;
                case "rfinger,lfinger":
                    return Bodypart.RightRing;
                case "neck":
                    return Bodypart.Neckless;
                default:
                    return Bodypart.None;
            }
        }
    }
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

        public enum Bodypart
        {
            Underwear = 0x0001,
            RightEarring = 0x0002,
            LeftEarring = 0x0004,
            Neckless = 0x0008,
            RightRing = 0x0010,
            LeftRing = 0x0020,
            Head = 0x0040,
            RightHand = 0x0080,
            LeftHand = 0x0100,
            Gloves = 0x0200,
            Chest = 0x0400,
            Legs = 0x0800,
            Boots = 0x1000,
            Back = 0x2000,
            TwoHand = 0x4000,
            FullArmor = 0x8000,
            Hair = 0x010000,
            Wolf = 0x020000,
            Hatchling = 0x100000,
            Strider = 0x200000,
            BabyPet = 0x400000,
            Face = 0x040000,
            DHair = 0x080000,
            None = 0x0000,
        }
    }
}
