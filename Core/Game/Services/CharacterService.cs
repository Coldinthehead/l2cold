using Core.Game.Data;
using Core.Game.Data.User;
using Core.Game.Network;
using Core.Game.Repository;

namespace Core.Game.Services
{
    public class CharacterService
    {
        private readonly PlayerRepository _playerRepository;
        private readonly PlayerTempaltesRepository _templateRepository;

        public CharacterService(PlayerRepository playerRepository
            , PlayerTempaltesRepository templateRepository)
        {
            _playerRepository = playerRepository;
            _templateRepository = templateRepository;
        }

        public List<GameCharacterModel> LoadCharacterList(string accountName)
        {
            return _playerRepository.LoadCharacterListByAccount(accountName);
        }

        public void CreateNewCharacter(GameClient client, int templateId, PlayerAppearance playerAppearance)
        {
            var template = _templateRepository.GetTemplate(templateId);
            _playerRepository.CreateRecord(client.AccountName, playerAppearance, template);
        }

        public GameCharacterModel LoadSingleCharacter(string accountName, int charId)
        {
            return _playerRepository.LoadSingleCharacter(accountName, charId);
        }
    }
}
