namespace Core.Engine
{
    public abstract class UpdatableComponent : Component
    {
        protected UpdatableComponent(GameObject gameObject) : base(gameObject)
        {
        }

        public abstract void Update(float dt);
    }
}
