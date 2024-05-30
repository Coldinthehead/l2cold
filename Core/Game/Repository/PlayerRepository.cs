using Core.Game.Data;
using Core.Game.Data.Static;
using Core.Game.Data.User;
using Core.Game.Services;
using System.Text.Json;

namespace Core.Game.Repository
{
    public class PlayerRepository
    {

        private readonly ObjectIdFactory _idFactory;
        
        private readonly Dictionary<string, List<GameCharacterModel>> _playersDatabase = new Dictionary<string, List<GameCharacterModel>>();

        public PlayerRepository(ObjectIdFactory idFactory)
        {
            _idFactory = idFactory;
            LoadData();
        }

        public void SaveData()
        {
            var data = JsonSerializer.Serialize(_playersDatabase);
            File.WriteAllText("./data.json", data);
            Console.WriteLine("data saved");
        }

        public void LoadData() 
        {
            if (File.Exists("./data.json"))
            {
                var json = File.ReadAllText("./data.json");
                var load = (Dictionary<string, List<GameCharacterModel>>)JsonSerializer.Deserialize(json, typeof(Dictionary<string, List<GameCharacterModel>>));
                foreach (var kp in load)
                {
                    _playersDatabase[kp.Key] = new List<GameCharacterModel>();
                    foreach (var character in kp.Value)
                    {
                        character.ObjectId = _idFactory.GetFreeId();
                        _playersDatabase[kp.Key].Add(character);
                    }
                }
            }
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
            model.CollisionHeight = model.Female ? template.CollisionHeightFemale : template.CollisionHeight;
            model.CollisionRadius = model.Female ? template.CollisionRadiusFemale : template.CollisionRadius;
            model.GearObjectId = new CharacterGear();
            model.GearItemId = new CharacterGear();

            if (!_playersDatabase.ContainsKey(accountName))
            {
                _playersDatabase[accountName] = new List<GameCharacterModel>();
            }
            _playersDatabase[accountName].Add(model);

            SaveData();
        }


        internal GameCharacterModel LoadSingleCharacter(string accountName, int charId)
        {
            return _playersDatabase[accountName][charId];
        }
    }
}
