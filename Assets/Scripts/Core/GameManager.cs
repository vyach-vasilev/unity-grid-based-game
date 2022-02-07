using UnityEngine;

public class GameManager: MonoBehaviour
{
    private MapController _mapController;
    private InputManager _inputManager;

    [SerializeField] private UnitsStorage _unitsStorage;
    [SerializeField] private DataProxy _dataProxy;
    
    private void Awake()
    { 
        CheckData();
        InputManager.Initialize();
        foreach (var unitData in _unitsStorage.UnitDataList)
        {
            CreateUnits(unitData);
        }
    }
    
    private void CreateUnits(UnitData unitData)
    {
        var modelFactory = new UnitModelFactory();
        var unitModel = (UnitModel)modelFactory.Model;

        unitModel.Name = unitData.Name;
        unitModel.Type = unitData.Type;
        unitModel.Position = unitData.Position;
        
        var viewFactory = new UnitViewFactory(unitData.Unit, unitModel.Type);
        var unitView = (UnitView)viewFactory.View;

        unitView.gameObject.name = unitModel.Name;

        var controllerFactory = new UnitControllerFactory(unitModel, unitView);
        var unitController = (UnitController)controllerFactory.Controller;
        unitController.transform.position = unitModel.Position;
        unitController.SetData(_dataProxy);
        unitController.Subscribe();
    }

    private void CheckData()
    {
        if (_dataProxy == null)
            _dataProxy = Resources.Load<DataProxy>("GameData/DataProxy");
        
        
        if (_unitsStorage == null)
            _unitsStorage = Resources.Load<UnitsStorage>("GameData/UnitsStorage");
        
    }
}