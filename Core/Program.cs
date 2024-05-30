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
using Core.Game.Network.Controller;
using Core.Game.Network.Contorller;
using Core.Game.Network.ClientPacket;
using Core.Game.Data.Static.Items;


namespace Core
{
    public class Program
    {
        static void Main(string[] args)
        {
            var login = new LoginServer(new TcpListener(IPAddress.Parse("127.0.0.1"), 2106)
                , new LoginClientFactory(), new LoginPacketHandler());

            var dataConfig = new DataConfig();
            var loginService = new LoginServerService(login);
            var activePlayers = new ActivePlayers();
            var idFactory = new ObjectIdFactory();
            var playerRepos = new PlayerRepository(idFactory);
            var charTempaltesFactory = new CharacterTemplateFactory(dataConfig);
            var attributeTableFactory = new AttributeTableFactory(dataConfig);
            var playerTemplateRepository = new PlayerTempaltesRepository(charTempaltesFactory);
            var attributeRepository = new AttributeBonusRepository(attributeTableFactory);
            var itemTemplatesFactory = new ItemTemplateFactory(dataConfig);
            var itemTemplateRepos = new ItemTemplatesRepository(itemTemplatesFactory);
            var characterService = new CharacterService(playerRepos, playerTemplateRepository);
            var playerFactory = new PlayerFactory(activePlayers, playerTemplateRepository, attributeRepository);
            var itemInstaceFactory = new ItemInstaceFactory(idFactory, itemTemplateRepos);
            var itemService = new ItemService(itemInstaceFactory);
            var gameServerControllers = new Dictionary<int, IPacketController>()
            {
                { InPacket.PROTOCOL_VERISION, new ProtocolVersionController() },
                { InPacket.REQUEST_AUTHENTICATION, new RequestAuthController(loginService, characterService) },
                { InPacket.CHARACTER_SELECTED, new CharacterSelectedController(playerFactory, characterService) },
                { InPacket.ENTER_WORLD,new EnterWorldController(activePlayers, itemService) },
                { InPacket.CHARACTER_MOVE_TO_LOCATION, new CharMoveController() },
                { InPacket.VALIDATE_POSITION, new ValidatePositionController(activePlayers) },
                { InPacket.REQUEST_SKILL_CD, new SkillCdController() },
                { InPacket.ACTION, new ActionController(activePlayers) },
                { 0xA8, new NetPingController() },
                { 0x0A, new AttackRequestController(activePlayers) },
                { 0x38, new SayController() },
                { 0x0e, new CharacterCreateController(playerTemplateRepository) },
                { 0x0b, new NewCharacterController(characterService) },
                { 0x0F, new RequestItemListController() },
                { 0x14, new UseItemController() },

            };

            AppDomain.CurrentDomain.ProcessExit += (x, y) =>
            {
                playerRepos.SaveData();
            };

            var game = new GameServer(
                new TcpListener(IPAddress.Parse("127.0.0.1"), 7777)
                , loginService
                , new GameClientFactory()
                , new GamePacketHandler(gameServerControllers));
            game.OnStart += () =>
            {
                Console.WriteLine($"GS listening on : {game.LocalEndPoint}");
                charTempaltesFactory.LoadTemplates();
            };
            var time = new Stopwatch();
            game.Start();
            login.Start();
            time.Start();
            var dt = 0.0f;
            float delta = 0f;
            float targetDelta = 1.0f / 30.0f;
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
