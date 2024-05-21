namespace Core.Engine
{
    public abstract class Component
    {
        public int ObjectId => gameObject.ObjectId;
        protected readonly GameObject gameObject;

        public Component(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public virtual void Awake()
        {

        }
        public virtual void OnStart()
        {

        }
    }
}
