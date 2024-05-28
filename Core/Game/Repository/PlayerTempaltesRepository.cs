using Core.Game.Data.Static;

namespace Core.Game.Repository
{
    public class PlayerTempaltesRepository
    {
        private readonly CharacterTemplateFactory _templateFactory;

        private readonly Dictionary<int, CharacterTemplate> _allTemplates;
        private readonly List<CharacterTemplate> _startingTemplates;

        public PlayerTempaltesRepository(CharacterTemplateFactory templateFactory)
        {
            _templateFactory = templateFactory;
            _allTemplates = new();
            _startingTemplates = new();
            LoadData();
            _startingTemplates.Add(_allTemplates[0]);
            _startingTemplates.Add(_allTemplates[1]);
            _startingTemplates.Add(_allTemplates[18]);
            _startingTemplates.Add(_allTemplates[25]);
            _startingTemplates.Add(_allTemplates[31]);
            _startingTemplates.Add(_allTemplates[38]);
            _startingTemplates.Add(_allTemplates[44]);
            _startingTemplates.Add(_allTemplates[49]);
            _startingTemplates.Add(_allTemplates[53]);
        }

        public void LoadData()
        {
            _templateFactory.LoadTemplates();
            foreach (var template in _templateFactory.Templates)
            {
                _allTemplates[template.ID] = template;
            }
        }

        public CharacterTemplate GetTemplate(int id) => _allTemplates[id];

        public List<CharacterTemplate> GetStartingTempaltes()
        {
            return new List<CharacterTemplate>(_startingTemplates);
        }
    }
}
