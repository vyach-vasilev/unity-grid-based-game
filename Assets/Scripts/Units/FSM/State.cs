public enum UnitState
{
    Idle = 0,
    Attack = 1,
    Moving = 2,
}

public abstract class State<T, U>
{
    public U Id { get; }

    protected State(U id)
    {
        Id = id;
    }

    public abstract void Enter(T owner);
    public abstract void Execute(T owner);
    public abstract void Exit(T owner);
}