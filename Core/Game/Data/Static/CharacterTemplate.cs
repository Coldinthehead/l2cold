using System.Xml;
using Core.Utils;
using System.Globalization;

namespace Core.Game.Data.Static
{
    public class DataConfig
    {
        public string CharacterTemplatesPath = "E:\\dotnet\\l2cold\\Core\\data\\";
    }

    public class CharacterTemplateFactory
    {
        private readonly DataConfig _config;
        public List<CharacterTemplate> Templates = new();

        public CharacterTemplateFactory(DataConfig config)
        {
            _config = config;
        }

        public void LoadTemplates()
        {
            XmlDocument doc = new XmlDocument();
            var path = _config.CharacterTemplatesPath + "playerTemplates.xml";
            doc.Load(File.OpenRead(path));

            var root = doc.DocumentElement;
            foreach (XmlElement item in root.ChildNodes)
            {
                var attrs = item.Attributes;
                var template = new Dictionary<string, string>();
                foreach (XmlAttribute attr in attrs)
                {
                    template[attr.Name] = attr.Value;
                }
                Templates.Add(new CharacterTemplate(template));
            }
            Console.WriteLine(doc);
        }
    }

    public class CharacterTemplate
    {
        public readonly int ID;
        public readonly string Name;
        public readonly string Race;
        public readonly int STR, CON, DEX, INT, WIT, MEN;
        public readonly int PAtk, PDef, MAtk, MDef, PAtkSpd, MAtkSpd, CritRate, RunSpeed;
        public readonly float x, y, z;
        public readonly float CollisionHeight, CollisionRadius, CollisionHeightFemale, CollisionRadiusFemale;
        public readonly Dictionary<int, float> HpTable, MpTable, CpTable;
        public readonly int BaseLevel;

        private static Dictionary<string, int> Races = new()
        {
            {"HUMAN", 0 },
            {"ELF", 1 },
            {"DARK_ELF", 2 },
            {"ORC", 3 },
            {"DWARF", 4 },
        };

        public CharacterTemplate(Dictionary<string, string> templateMap)
        {
            ID = templateMap.GetInt("id");
            Name = templateMap["name"];
            Race = templateMap["race"];
            STR = templateMap.GetInt("baseSTR");
            CON = templateMap.GetInt("baseCON");
            DEX = templateMap.GetInt("baseDEX");
            INT = templateMap.GetInt("baseINT");
            WIT = templateMap.GetInt("baseWIT");
            MEN = templateMap.GetInt("baseMEN");
            PAtk = templateMap.GetInt("basePAtk");
            PDef = templateMap.GetInt("basePDef");
            MAtk = templateMap.GetInt("baseMAtk");
            MDef = templateMap.GetInt("baseMDef");
            PAtkSpd = templateMap.GetInt("basePAtkSpd");
            MAtkSpd = templateMap.GetInt("baseMAtkSpd");
            CritRate = templateMap.GetInt("baseCritRate");
            RunSpeed = templateMap.GetInt("baseRunSpd");
            x = templateMap.GetInt("spawnX");
            y = templateMap.GetInt("spawnY");
            z = templateMap.GetInt("spawnZ");
            CollisionRadius = templateMap.GetFloat("collision_radius");
            CollisionHeight = templateMap.GetFloat("collision_height");
            CollisionRadiusFemale = templateMap.GetFloat("collision_radius_female");
            CollisionHeightFemale = templateMap.GetFloat("collision_height_female");
            BaseLevel = templateMap.GetInt("baseLevel");
            HpTable = TableFrom(templateMap["hpTable"], BaseLevel);
            MpTable = TableFrom(templateMap["mpTable"], BaseLevel);
            CpTable = TableFrom(templateMap["cpTable"], BaseLevel);
        }

        private Dictionary<int, float> TableFrom(string str, int baseLevel)
        {
            var values = ParseTable(str);
            var table = new Dictionary<int, float>();

            for (int i = 0; i < values.Length; i++)
            {
                table[baseLevel + i] = values[i];
            }

            return table;
        }

        private float[] ParseTable(string table)
        {
            var split = table.Split(";");
            var result = new float[split.Length];

            for (int i = 0; i < split.Length; i++)
            {
                result[i] = float.Parse(split[i], NumberStyles.Any, CultureInfo.InvariantCulture);
            }

            return result;
        }

        public int GetRace()
        {
            return Races[Race];
        }

        public double GetHalth(int level)
        {
            return (double)HpTable[level];
        }
        public double GetMana(int level)
        {
            return (double)MpTable[level];
        }

        public double GetCp(int level)
        {
            return (double)CpTable[level];
        }
    }
}
