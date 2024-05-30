using Core.Utils;
using System.Xml;

namespace Core.Game.Data.Static
{

    public class AttributeTableFactory
    {
        private readonly DataConfig _config;

        public AttributeTableFactory(DataConfig config)
        {
            _config = config;
        }

        public Dictionary<string, AttributeTable> Load()
        {
            var tables = new Dictionary<string, AttributeTable>();  
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(_config.DataRootPath + "statBonus.xml", settings);
            XmlDocument doc = new XmlDocument();
            
            doc.Load(reader);
            var root = doc.DocumentElement;

            foreach (XmlElement element in root.ChildNodes)
            {
                var name = element.Name;
                var map = new Dictionary<string, string>();
                foreach(XmlElement statElement in element.ChildNodes)
                {
                    var attributes = statElement.Attributes;
                    var value = attributes["value"].Value;
                    var bonus = attributes["bonus"].Value;
                    map[value] = bonus;
                }
                tables[name] = new AttributeTable(ParseTable(map));
            }
            return tables;
        }

        private Dictionary<int, float> ParseTable(Dictionary<string, string> map)
        {
            var result = new Dictionary<int, float>();

            foreach (var key in map.Keys)
            {
                var value = int.Parse(key);
                var bonus = map.GetFloat(key);
                result[value] = bonus;
            }

            return result;
        }
    }

    public class AttributeTable
    {
        public readonly Dictionary<int, float> _attributeTable;

        public AttributeTable(Dictionary<int, float> attributeTable)
        {
            _attributeTable = attributeTable;
        }

        public float this[int i]
        {
            get { return _attributeTable[i]; }
        }
    }
}
