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
        var dataProvider = (DataProvider)data;
        
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
            
            var controllerFactory = new UnitControllerFactory(unitModel, unitView);
            var unitController = (UnitController)controllerFactory.Controller;
            
            unitController.DataProvider = dataProvider;
            
            if (!dataProvider.Units.Contains(unitController)) dataProvider.Units.Add(unitController);
            
            var unitPathController = new UnitPathController(unitController, unitData.Speed);
            var unitSelectionController = new UnitSelectionController(unitView, dataProvider);
            var fsmController = new FSMController(unitController);

            unitController.UnitPathController = unitPathController;
            unitController.UnitSelectionController = unitSelectionController;
            unitController.FSMController = fsmController;
            
            unitController.transform.position = unitModel.Position;
            unitController.Subscribe();
        }
    }
}