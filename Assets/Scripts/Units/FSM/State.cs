public enum UnitState
{
    Idle = 0,
    Moving = 1,
    Attack = 2,
    HeavyAttack = 3,
    MagicAttack = 4,
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