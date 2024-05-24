namespace Core.Engine
{
    public class GameObject
    {
        public int ObjectId { get; private set; }
        public readonly Transform transform;
        private Dictionary<Type, Component> _components = new();
        private List<UpdatableComponent> _updatableComponents = new();

        public GameObject(int id)
        {
            ObjectId = id;
            transform = new Transform(this);
        }

        public void Update(float dt)
        {
            foreach (var c in _updatableComponents)
            {
                c.Update(dt);
            }
        }

        public T GetComponent<T>() where T : Component
        {
            var c = _components[typeof(T)];

            return c as T;
        }

        public void AddComponent(Component component)
        {
            PutAbstractions(component);
            component.OnAdd(this);
        }

        public void AddComponent(UpdatableComponent component)
        {
            _updatableComponents.Add(component);
            PutAbstractions(component);
            component.OnAdd(this);
        }
        
        private void PutAbstractions(Component component)
        {
            var interfaces = component.GetType().GetInterfaces();
            foreach (var t in interfaces)
            {
                _components[t] = component;
            }
            var baseType = component.GetType();
            while (baseType != typeof(Component) && baseType != null)
            {
                _components[baseType] = component;
                baseType = baseType.BaseType;
            }
        }

        public void Start()
        {
            foreach (var c in _components.Values)
            {
                c.OnStart();
            }
        }
    }
}
