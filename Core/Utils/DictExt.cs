
using System.Globalization;

namespace Core.Utils
{
    public static class DictExt
    {
        public static int GetInt<T>(this Dictionary<T, string> map, T key)
        {
            return int.Parse(map[key]);
        }

        public static int GetIntOrDefault<T>(this Dictionary<T, string> map, T key, int def= 0)
        {
            if (map.ContainsKey(key))
                return int.Parse(map[key]);
            else
                return def;
        }

        public static float GetFloat<T>(this Dictionary<T, string> map, T key)
        {
            return float.Parse(map[key], NumberStyles.Any, CultureInfo.InvariantCulture);
        }
    }
}
