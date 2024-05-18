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
        static void Main(string[] args)
        {
            var login = new LoginServer(new TcpListener(IPAddress.Parse("127.0.0.1"), 2106));
            var loginService = new LoginServerService(login);
            var game = new GameServer(new TcpListener(IPAddress.Parse("127.0.0.1"), 7777), loginService);
            var time = new Stopwatch();
            game.Start();
            login.Start();
            time.Start();
            var dt = 0.0f;
            float delta = 0f;
            float targetDelta = 1.0f / 30.0f ;
            while (login.Running && game.Runing)
            {
                delta += dt;
                login.Tick();
                game.Tick();
                while (delta >= targetDelta)
                {
                    delta -= targetDelta;
                    game.UpdateWorld(targetDelta);
                }
                Thread.Sleep(1);
                dt = (float)time.Elapsed.TotalSeconds;
                time.Restart();
            }

            game.Stop();
            login.Stop();
        }
    }
}
