public enum UnitState
{
    Idle = 0,
    Moving = 1,
    Attack1 = 2,
    Attack2 = 3,
    Attack3 = 4,
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