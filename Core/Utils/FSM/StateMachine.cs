namespace Core.Utils.FSM
{
    public interface IPayloadedState<T> : IExitableState
    {
        public void OnEnter(T state);
    }


    public interface IState : IExitableState
    {
        public void OnEnter();
       
    }

    public interface IExitableState
    {
        public void OnExit();
    }

    public class StateMachine<IStateParameter> where IStateParameter : IExitableState
    {
        private Dictionary<Type, IStateParameter> _stateMap = new();
        protected IStateParameter CurrentState;


        public void AddState<V>(V state ) where V : IStateParameter
        {
            _stateMap[state.GetType()] = state;
        }

        public void ChangeState<TState>() where TState: class, IStateParameter, IState
        {
            CurrentState?.OnExit();
            TState state = GetState<TState>();
            state.OnEnter();
            CurrentState = state;
        }

        public void ChangeState<TState,TPayload>(TPayload payload) 
            where TState :class, IPayloadedState<TPayload>, IStateParameter
        {
            CurrentState?.OnExit();
            TState state = GetState<TState>();
            state.OnEnter(payload);
            CurrentState = state;
        }

        public T GetState<T>() where T : class
        {
            return _stateMap[typeof(T)] as T;
        }


    }
}
