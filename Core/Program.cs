using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Core.Login;
using Core.Game;


namespace Core
{
    public class Program
    {
        static void Main(string[] args)
        {
            var login = new LoginServer(new TcpListener(IPAddress.Parse("127.0.0.1"), 2106));
            var game = new GameServer(new TcpListener(IPAddress.Parse("127.0.0.1"), 7777));
            game.Start();
            login.Start();
            while (login.Running && game.Runing)
            {
                login.Tick();
                game.Tick();
                Thread.Sleep(100);
            }

            game.Stop();
            login.Stop();
        }
    }
}
