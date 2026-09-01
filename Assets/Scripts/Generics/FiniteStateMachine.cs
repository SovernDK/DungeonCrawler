public class FiniteStateMachine<T1, T2>
{
    protected T1[] states;
    private T1 currentState;

    public T1 CurrentState { get => currentState; set => currentState = value; }

    public virtual void Init(T2 context)
    {
        
    }

    public virtual void SetState(int id)
    {
        
    }
}
