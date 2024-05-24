namespace Core.Engine
{
    public abstract class Component
    {
        public int ObjectId => _gameObject.ObjectId;
        public GameObject gameObject => _gameObject;

        private GameObject _gameObject;

        public void OnAdd(GameObject gameObject)
        {
            _gameObject = gameObject;
            Awake();
        }
        public virtual void Awake()
        {

        }
        public virtual void OnStart()
        {

        }
    }
}
