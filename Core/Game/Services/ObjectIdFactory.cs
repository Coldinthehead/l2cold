using Core.Utils;

namespace Core.Game.Services
{
    public class ObjectIdFactory
    {
        private int current = ServerRandom.Next(10055, 100000555);


        public int GetFreeId()
        {
            return current++;
        }
    }
}
