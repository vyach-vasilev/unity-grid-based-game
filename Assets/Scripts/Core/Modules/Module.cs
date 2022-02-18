public abstract class Module<T, U>
{
    public U Id { get; set; }

    protected Module(U id)
    {
        Id = id;
    }
    
    public abstract void Execute(T data);
    
    public virtual void Update(T data){}
}