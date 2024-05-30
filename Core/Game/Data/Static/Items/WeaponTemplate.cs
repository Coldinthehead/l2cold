using Core.Utils;
using System.Xml;

namespace Core.Game.Data.Static.Items
{
    public class ItemTemplateFactory
    {
        private readonly DataConfig _config;
        private readonly List<string> _weaponSlots = new();
        public ItemTemplateFactory(DataConfig config)
        {
            _config = config;
            _weaponSlots.Add("rhand");
            _weaponSlots.Add("lrhand");
        }

        public List<WeaponTemplate> LoadWeapons()
        {
            var result = new List<WeaponTemplate>();

            var filepath = _config.DataRootPath + "items\\0000-0099.xml";

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(filepath, settings);
            XmlDocument doc = new XmlDocument();

            doc.Load(reader);
            var root = doc.DocumentElement;

            foreach (XmlElement itemElement in root.ChildNodes)
            {
                var itemAttributes = itemElement.Attributes;
                var type = itemAttributes["type"].Value;
                if (type.Equals("Weapon"))
                {
                    var name = itemAttributes["name"].Value;
                    var id = itemAttributes["id"].Value;

                    var map = new Dictionary<string, string>();
                    map["name"] = name;
                    map["id"] = id;
                    foreach (XmlElement itemStat in itemElement.ChildNodes)
                    {
                        var attrs = itemStat.Attributes;
                        if (attrs.Count == 2)
                        {
                            map.Add(attrs[0].Value, attrs[1].Value);
                        }
                    }
                    if (_weaponSlots.Contains(map["bodypart"]))
                    {
                        var template = new WeaponTemplate(map);
                        result.Add(template);
                    }
                }
            }
            return result;
        }
    }


    public class WeaponTemplate
    {
        public int ID;
        public string Name;
        public Constants.Bodypart Bodypart;
        public int PAtk;
        public string WeaponType;
        public int Crit;
        public int PAtkSpd;
        public int MAtk;

        public WeaponTemplate(Dictionary<string, string> templateMap)
        {
            ID = templateMap.GetInt("id");
            Name = templateMap["name"];
            Bodypart = templateMap["bodypart"].ParseAsBodypart();
            PAtk = templateMap.GetIntOrDefault("p_dam");
            WeaponType = templateMap["weapon_type"];
            Crit = templateMap.GetIntOrDefault("critical");
            PAtk = templateMap.GetIntOrDefault("atk_speed");
            MAtk = templateMap.GetIntOrDefault("m_dam");
        }
    }
}
