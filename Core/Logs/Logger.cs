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
