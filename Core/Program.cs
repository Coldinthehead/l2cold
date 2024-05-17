using System.Net;
using System.Net.Sockets;
using Core.Login;
using Core.Game;
using Core.Common.Services;
using System.Diagnostics;


namespace Core
{
    public class Program
    {
        public class Time()
        {
            private readonly Stopwatch _time = new();


            public void Start()
            {
                _time.Start();
            }

            public void Restart()
            {
                _time.Restart();
            }
        }
        static void Main(string[] args)
        {
            var login = new LoginServer(new TcpListener(IPAddress.Parse("127.0.0.1"), 2106));
            var loginService = new LoginServerService(login);
            var game = new GameServer(new TcpListener(IPAddress.Parse("127.0.0.1"), 7777), loginService);
            var time = new Stopwatch();
            game.Start();
            login.Start();
            var dt = 0.0f;
            time.Start();
            float delta = 0f;
            float targetDelta = 0.064f;
            while (login.Running && game.Runing)
            {
                delta += dt;
                login.Tick();
                if (delta >= targetDelta)
                {
                    delta -= targetDelta;
                    game.Tick(targetDelta);
                }
                Thread.Sleep(25);
                dt = time.ElapsedMilliseconds * 0.001f;
                time.Restart();
            }

            game.Stop();
            login.Stop();
        }
    }
}
