using UnityEngine;

public class GameManager: MonoBehaviour
{
    private MapController _mapController;
    private InputManager _inputManager;

    [SerializeField] private UnitsStorage _unitsStorage;
    [SerializeField] private DataProvider _dataProvider;
    [SerializeField] private KeyBindings _keyBindings;
    
    private void Awake()
    { 
        PrepareData();
        InputManager.Initialize(_keyBindings);
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
        unitController.SetData(_dataProvider);
        unitController.Subscribe();
    }

    private void PrepareData()
    {
        if (_dataProvider == null)
            _dataProvider = Resources.Load<DataProvider>("GameData/DataProxy");
        
        if (_unitsStorage == null)
            _unitsStorage = Resources.Load<UnitsStorage>("GameData/UnitsStorage");
        
        if (_keyBindings == null)
            _keyBindings = Resources.Load<KeyBindings>("GameData/KeyBindings");
    }
}