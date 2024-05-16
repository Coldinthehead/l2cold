
using Core.Game.Data;

namespace Core.Game
{
    public class PlayerRepository
    {
        private readonly List<Player> _savedPlayers = new List<Player>();

        private readonly ObjectIdFactory _idFactory;

        public PlayerRepository(ObjectIdFactory idFactory)
        {
            _idFactory = idFactory;
            for (int i = 0; i < 5; i++)
                _savedPlayers.Add(BuildPlayer());
        }

        public Player LoadCharacter(int charId)
        {
            return _savedPlayers[charId];
        }

        public List<Player> LoadCharacterList()
        {
            return _savedPlayers;
        }

        private Player BuildPlayer()
        {
            var characterData = GameCharacter.BuildMockCharacter();
            characterData.Info.ObjectId = _idFactory.GetFreeId();
            characterData.Info.Name = "Hello" + characterData.Info.ObjectId;

            return new Player(characterData.Info.ObjectId
                , new Math.Vec2(characterData.x, characterData.y)
                , (float)characterData.z
                , characterData);

        }
    }
}
