using Core.Utils;

namespace Core.Security
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
            return new SessionKeys(333
                , 555
                , 666
                , 777);
        }

        public static SessionKeys GetEmptyKeys()
        {
            return new SessionKeys(1, 2, 3, 4);
        }
    }
}
