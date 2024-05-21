using System.Net;
using System.Net.Sockets;
using Core.Login;
using Core.Game;
using Core.Common.Services;
using System.Diagnostics;
using Core.Login.Network;
using Core.Game.Network;
using Core.Game.World;
using Core.Game.Services;
using Core.Game.Repository;


namespace Core
{
    public class Program
    {
        static void Main(string[] args)
        {
            var login = new LoginServer(new TcpListener(IPAddress.Parse("127.0.0.1"), 2106)
                , new LoginClientFactory(), new LoginPacketHandler());

            var loginService = new LoginServerService(login);
            var activePlayers = new ActivePlayers();
            var idFactory = new ObjectIdFactory();
            var playerRepos = new PlayerRepository(idFactory);
            var game = new GameServer(
                new TcpListener(IPAddress.Parse("127.0.0.1"), 7777)
                , loginService
                , new GameClientFactory()
                , new GamePacketHandler(loginService, activePlayers, idFactory, playerRepos));
            game.OnStart += () =>
            {
                Console.WriteLine($"GS listening on : {game.LocalEndPoint}");
                activePlayers.AddGhost(playerRepos.BuildGhost());
            };
            var time = new Stopwatch();
            game.Start();
            login.Start();
            time.Start();
            var dt = 0.0f;
            float delta = 0f;
            float targetDelta = 1.0f / 30.0f ;
            while (login.IsRunning && game.IsRunning)
            {
                delta += dt;
                login.Tick();
                game.Tick();
                while (delta >= targetDelta)
                {
                    delta -= targetDelta;
                    activePlayers.Tick(targetDelta);
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
