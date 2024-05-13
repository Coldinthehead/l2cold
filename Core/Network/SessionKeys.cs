namespace Core.Network
{
    public class SessionKeys
    {
        public readonly int Login1;
        public readonly int Login2;
        public readonly int Play1;
        public readonly int Play2;


        private SessionKeys(int a, int b, int c, int d)
        {
            Login1 = a;
            Login2 = b;
            Play1 = c;
            Play2 = d;
        }

        public static SessionKeys GenerateSessionKeys()
        {
            var rand = new Random();
            return new SessionKeys(rand.Next(2, Int32.MaxValue), rand.Next(3, Int32.MaxValue), rand.Next(4, int.MaxValue), rand.Next(5, Int32.MaxValue));
        }

        public static SessionKeys GetEmptyKeys()
        {
            return new SessionKeys(1, 2, 3, 4);
        }
    }
}
