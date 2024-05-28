using Core.Game.Data.Static;

namespace Core.Game.Repository
{
    public class PlayerTempaltesRepository
    {
        private readonly CharacterTemplateFactory _templateFactory;

        private readonly Dictionary<int, CharacterTemplate> _allTemplates;

        public PlayerTempaltesRepository(CharacterTemplateFactory templateFactory)
        {
            _templateFactory = templateFactory;
            _allTemplates = new();
            LoadData();
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
    }
}
