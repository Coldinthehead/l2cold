using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Core.Logs
{
    public class Logger<T>
    {
        private readonly string _name;
        public static Logger<T> BuildLogger() => new Logger<T>();

        public Logger()
        {
            _name = this.GetType().GenericTypeArguments[0].Name;
        }

        public void Log(string msg)
        {
            Console.WriteLine($"[{_name}] {msg}");
        }
    }
}
