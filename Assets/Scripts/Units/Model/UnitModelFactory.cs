public class UnitModelFactory: IUnitModelFactory
{
    public IUnitModel Model { get; }

    public UnitModelFactory()
    {
        Model = new UnitModel();
    }
}