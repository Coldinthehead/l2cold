﻿using Core.Game.Data;
using Core.Game.Services;
using Core.Game.World.Actor;
using Core.Utils.Math;

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
                _savedPlayers.Add(BuildPlayer());
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

        private GameCharacterModel BuildPlayer()
        {
            var characterData = GameCharacterModel.BuildMockCharacter();
            characterData.Info.ObjectId = _idFactory.GetFreeId();
            characterData.Info.Name = "Hello" + characterData.Info.ObjectId;

            return characterData;

        }
    }
}
