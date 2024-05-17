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
            var dt = 0.0f;
            time.Start();
            while (login.Running && game.Runing)
            { 
                login.Tick();
                game.Tick(dt);
                Thread.Sleep(25);
                dt = time.ElapsedMilliseconds * 0.001f;
                time.Restart();
            }

            game.Stop();
            login.Stop();
        }
    }
}
