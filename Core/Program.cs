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
using Core.Game.World.Factory;
using Core.Game.Data.Static;


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
            var playerFactory = new PlayerFactory(activePlayers);
            var charTempaltesFactory = new CharacterTemplateFactory(new DataConfig());
            var playerTemplateRepository = new PlayerTempaltesRepository(charTempaltesFactory);
            var characterService = new CharacterService(playerRepos, playerTemplateRepository);

            var game = new GameServer(
                new TcpListener(IPAddress.Parse("127.0.0.1"), 7777)
                , loginService
                , new GameClientFactory()
                , new GamePacketHandler(
                    loginService
                    , activePlayers
                    , playerRepos
                    , playerFactory
                    , playerTemplateRepository
                    , characterService));
            game.OnStart += () =>
            {
                Console.WriteLine($"GS listening on : {game.LocalEndPoint}");
                charTempaltesFactory.LoadTemplates();
          /*      for (int i = 0; i < 3; i++)
                {
                    var ghost = playerFactory.BuildGhostPlayer(playerRepos.LoadGhostData());
                    activePlayers.AddGhost(ghost);
                }
                activePlayers.AddGhost(playerFactory.BuildPlayer(null, playerRepos.LoadCharacter(0)));*/

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
