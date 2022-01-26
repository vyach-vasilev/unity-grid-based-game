public class UnitControllerFactory: IUnitFactoryController
{
    public IUnitController Controller { get; private set; }
    
    public UnitControllerFactory(IUnitModel model, IUnitView view)
    {
        var unit = (UnitView)view;
        var controllerExist = unit.gameObject.TryGetComponent<UnitController>(out var controller);
        Controller = controllerExist ? controller : unit.gameObject.AddComponent<UnitController>();
        Controller.Initialize(model, view);
    }
}