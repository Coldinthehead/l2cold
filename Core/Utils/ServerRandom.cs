namespace Core.Utils
{
    public static class ServerRandom
    {
        private static Random _random = new Random(DateTime.Now.Millisecond);

        public static int Next(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }

        public static byte[] NextBytes(int len)
        {
            var buffer = new byte[len];
             _random.NextBytes(buffer);
            return buffer;
        }
    }
}
