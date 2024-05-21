namespace Core.Utils.Logs
{
    public class Logger<T>
    {
        private readonly string _name;
        public static Logger<T> BuildLogger() => new Logger<T>();

        public Logger()
        {
            _name = GetType().GenericTypeArguments[0].Name;
        }

        public void Log(string msg)
        {
            Console.WriteLine($"[{_name}] {msg}");
        }

        public void Log<V>(string msg, V payload)
        {
            Console.WriteLine($"[{_name}] {msg} <{payload}>");
        }
    }
}
