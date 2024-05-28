using Core.Game.Data;
using Core.Game.Data.Static;
using Core.Game.Data.User;
using Core.Game.Services;

namespace Core.Game.Repository
{
    public class PlayerRepository
    {

        private readonly ObjectIdFactory _idFactory;

        private readonly Dictionary<string, List<GameCharacterModel>> _playersDatabase = new Dictionary<string, List<GameCharacterModel>>();

        public PlayerRepository(ObjectIdFactory idFactory)
        {
            _idFactory = idFactory;
        }

        public GameCharacterModel LoadGhostData()
        {
            var ghost = GameCharacterModel.BuildMockCharacter();
            ghost.ObjectId = _idFactory.GetFreeId();
            ghost.Name = "Ghost" + ghost.ObjectId;
            return ghost;
        }

        public List<GameCharacterModel> LoadCharacterListByAccount(string accountName)
        {
            if (!_playersDatabase.ContainsKey(accountName))
            {
                _playersDatabase[accountName] = new List<GameCharacterModel>();
            }

            return _playersDatabase[accountName];
        }

        public void CreateRecord(string accountName, PlayerAppearance playerAppearance, CharacterTemplate template)
        {
            var model = new GameCharacterModel();
            model.Title = "";
            model.Name = playerAppearance.Name;
            model.ObjectId = _idFactory.GetFreeId();
            model.ClanId = 0;
            model.Female = playerAppearance.Female;
            model.Race = playerAppearance.Race;
            model.BaseClass = template.ID;
            model.CurrentHealth = template.GetHalth(1);
            model.CurrentMana = template.GetHalth(1);
            model.Sp = 0;
            model.Exp = 0;
            model.Level = 1;
            model.Karma = 0;
            model.HairStyle = playerAppearance.HairStyle;
            model.HairColor = playerAppearance.HairColor;
            model.Face = playerAppearance.Face;
            model.MaxHealth = model.CurrentHealth;
            model.MaxMana = model.CurrentMana;
            model.CurrentClass = template.ID;
            model.x = template.x;
            model.y = template.y;
            model.z = template.z;
            model.Stats = BuildStat(template);
            model.GearObjectId = new CharacterGear();
            model.GearItemId = new CharacterGear();

            if (!_playersDatabase.ContainsKey(accountName))
            {
                _playersDatabase[accountName] = new List<GameCharacterModel>();
            }
            _playersDatabase[accountName].Add(model);
        }

        private CharacterStats BuildStat(CharacterTemplate template)
        {
            var stats = new CharacterStats();
            stats.STR = template.STR;
            stats.DEX = template.DEX;
            stats.CON = template.CON;
            stats.INT = template.INT;
            stats.WIT = template.WIT;
            stats.MEN = template.MEN;

            stats.Patk = template.PAtk;
            stats.PatkSpd = template.PAtkSpd;
            stats.Pdef = template.PDef;
            stats.Evasion = 0;
            stats.Accuracy = 0;
            stats.Crit = template.CritRate;
            stats.Matk = template.MAtk;
            stats.MatkSpd = template.MAtkSpd;
            stats.Mdef = template.MDef;

            stats.RunSpd = template.RunSpeed;
            stats.WalkSpd = template.RunSpeed / 2;

            return stats;
        }

        internal GameCharacterModel LoadSingleCharacter(string accountName, int charId)
        {
            return _playersDatabase[accountName][charId];
        }
    }
}
