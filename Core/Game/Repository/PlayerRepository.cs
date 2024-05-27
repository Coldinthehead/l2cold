using Core.Game.Data;
using Core.Game.Services;

namespace Core.Game.Repository
{
    public class PlayerRepository
    {
        private readonly List<GameCharacterModel> _savedPlayers = new List<GameCharacterModel>();

        private readonly ObjectIdFactory _idFactory;

        public PlayerRepository(ObjectIdFactory idFactory)
        {
            _idFactory = idFactory;
            for (int i = 0; i < 5; i++)
                _savedPlayers.Add(BuildPlayerModel());
        }

        public GameCharacterModel LoadCharacter(int charId)
        {
            return _savedPlayers[charId];
        }

        public List<GameCharacterModel> LoadCharacterList()
        {
            return _savedPlayers;
        }

  /*      public GhostPlayer BuildGhost()
        {
            var data = GameCharacterModel.BuildMockCharacter();
            data.Info.ObjectId = _idFactory.GetFreeId();
            data.Info.Name = "Ghost" + data.Info.ObjectId;

            var ghost = new GhostPlayer(data.Info.ObjectId, new Vec2(data.x, data.y)
                , (float)data.z, data);

            ghost.AddMoveNode(new Vec2(data.x, data.y));
            ghost.AddMoveNode(new Vec2(7835, 7208));
            ghost.AddMoveNode(new Vec2(8469, 7328));
            ghost.AddMoveNode(new Vec2(7052, 7393));

            return ghost;
        }*/

        public GameCharacterModel BuildPlayerModel()
        {
            var characterData = GameCharacterModel.BuildMockCharacter();
            characterData.Info.ObjectId = _idFactory.GetFreeId();
            characterData.Info.Name = "Hello" + characterData.Info.ObjectId;

            return characterData;

        }

        public GameCharacterModel LoadGhostData()
        {
            var ghost = GameCharacterModel.BuildMockCharacter();
            ghost.Info.ObjectId = _idFactory.GetFreeId();
            ghost.Info.Name = "Ghost" + ghost.Info.ObjectId;
            return ghost;
        }
    }
}
