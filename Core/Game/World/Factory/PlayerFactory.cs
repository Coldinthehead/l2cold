using Core.Engine;
using Core.Game.Data;
using Core.Game.Network;
using Core.Game.World.Actor;
using Core.Game.World.Actor.PlayerStates;
using Core.Game.World.Components;
using Core.Utils.Math;
using Core.Utils;
using Core.Game.Repository;
using Core.Game.Data.Static;

namespace Core.Game.World.Factory
{
    public class PlayerFactory
    {
        private readonly ActivePlayers _activePlayers;
        private readonly PlayerTempaltesRepository _playerTemplates;
        private readonly AttributeBonusRepository _attributeRepository;

        public PlayerFactory(ActivePlayers activePlayers
            , PlayerTempaltesRepository playerTempaltes
            , AttributeBonusRepository attributeRepository)
        {
            _activePlayers = activePlayers;
            _playerTemplates = playerTempaltes;
            _attributeRepository = attributeRepository;
        }

        public GameObject BuildPlayer(GameClient client, GameCharacterModel details)
        {
            var gameObject = new GameObject(details.ObjectId);
            gameObject.transform.Position = new Vec2(details.x, details.y);
            gameObject.transform.ZPosition = (float)details.z;

            var state = new PlayerState(details);
            gameObject.AddComponent(state);

            var stats = BuildState(_playerTemplates.GetTemplate(details.CurrentClass));
            gameObject.AddComponent(stats);

            var networkComponent = new PlayerNetwork(client, _activePlayers);
            gameObject.AddComponent(networkComponent);

            var movementComponent = new MovemventComponent();
            gameObject.AddComponent(movementComponent);

            var behaviour = new PlayerBehaviour(BuildPlayerFSM(gameObject));
            gameObject.AddComponent(behaviour);

            var invenotry = new PlayerInventory();
            gameObject.AddComponent(invenotry);

            gameObject.Start();
            return gameObject;
        }

        public GameObject BuildGhostPlayer(GameCharacterModel model)
        {
            var gameObject = new GameObject(model.ObjectId);
            
            gameObject.transform.Position = new Vec2(model.x, model.y );
            gameObject.transform.ZPosition = (float)model.z;

            gameObject.AddComponent(new PlayerState(model));
            gameObject.AddComponent(new GhostNetwork(_activePlayers));
            gameObject.AddComponent(new MovemventComponent());

            var beh = new GhostBehaviour(BuildPlayerFSM(gameObject));
            var movePoints = new List<Vec2>()
            {
                new Vec2(7835 + ServerRandom.Next(-2000, 2000), 7208+ ServerRandom.Next(-2000, 2000)),
                new Vec2(ServerRandom.Next(7000, 8500), ServerRandom.Next(7200, 7880)),
                new Vec2(8469, 7328),
                new Vec2(ServerRandom.Next(7000, 8500), ServerRandom.Next(7200, 7880)),
                new Vec2(7052, 7393),
                new Vec2(ServerRandom.Next(7000, 8500), ServerRandom.Next(7200, 7880)),
                new Vec2(ServerRandom.Next(7000, 8500), ServerRandom.Next(7200, 7880)),
            };
            beh.SetMovePoint(movePoints);
            gameObject.AddComponent(beh);

            gameObject.Start();
            return gameObject;
        }

        private PlayerBehaviourFSM BuildPlayerFSM(GameObject gameObject)
        {
            var fsm = new PlayerBehaviourFSM(gameObject);
            fsm.AddState(new PlayerIdleState(fsm));
            fsm.AddState(new PlayerMoveToPointState(fsm));
            fsm.AddState(new PlayerFollowTarget(fsm));
            fsm.AddState(new PlayerAttackState(fsm));
            fsm.AddState(new PostAttackState(fsm));
            return fsm;
        }

        private EntityStats BuildState(CharacterTemplate template)
        {
            var stats = new EntityStats();
            stats._mAtkSpd.Add(template.MAtkSpd);
            stats._mAtkSpd.SetBaseMultipilier(_attributeRepository.GetWitBonus(template.WIT));
            stats._pAtkSpd.Add(template.PAtkSpd); 
            stats._pAtkSpd.SetBaseMultipilier(_attributeRepository.GetDexBonus(template.DEX));
            stats._runSpd.Add(template.RunSpeed);
            stats._runSpd.SetBaseMultipilier(_attributeRepository.GetDexBonus(template.DEX));
            stats._str.Add(template.STR);
            stats._dex.Add(template.DEX);
            stats._con.Add(template.CON);
            stats._int.Add(template.INT);
            stats._men.Add(template.MEN);
            stats._wit.Add(template.WIT);
            stats._pAtk.Add(template.PAtk);
            stats._pAtk.SetBaseMultipilier(_attributeRepository.GetStrBonus(template.STR));
            stats._mAtk.Add(template.MAtk);
            stats._mAtk.SetBaseMultipilier(_attributeRepository.GetIntBonus(template.INT));
            stats._pDef.Add(template.PDef);
            stats._mDef.Add(template.MDef);
            stats._mDef.SetBaseMultipilier(_attributeRepository.GetMenBonus(template.MEN));
            stats._crit.Add(template.CritRate);
            stats._crit.SetBaseMultipilier(_attributeRepository.GetDexBonus(template.DEX));
            return stats;
        }

    }
}
