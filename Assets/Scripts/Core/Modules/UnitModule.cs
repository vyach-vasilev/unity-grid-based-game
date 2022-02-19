using UnityEngine;

public class UnitModule : Module<DataStorage, ModuleType>
{
    private readonly UnitsStorage _unitsStorage;
    
    public UnitModule(ModuleType id, UnitsStorage unitsStorage) : base(id)
    {
        _unitsStorage = unitsStorage;
    }
    
    public override void Execute(DataStorage data)
    {
        Debug.Log("Execute: " + Id);
        foreach (var unitData in _unitsStorage.UnitDataList)
        {
            var modelFactory = new UnitModelFactory();
            var unitModel = (UnitModel)modelFactory.Model;

            unitModel.Name = unitData.Name;
            unitModel.Type = unitData.Type;
            unitModel.Position = unitData.Position;

            var viewFactory = new UnitViewFactory(unitData.Unit);
            var unitView = (UnitView)viewFactory.View;

            unitView.gameObject.name = unitModel.Name;

            var controllerFactory = new UnitControllerFactory(unitModel, unitView, (DataProvider)data);
            var unitController = (UnitController)controllerFactory.Controller;
            unitController.transform.position = unitModel.Position;
            unitController.Subscribe();
        }
    }
}