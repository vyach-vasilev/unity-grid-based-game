using UnityEngine;

public class GameManager: MonoBehaviour
{
    private InputManager _inputManager;

    [SerializeField] private DataProvider _dataProvider;
    [SerializeField] private MapData _mapData;
    [SerializeField] private UnitsStorage _unitsStorage;
    [SerializeField] private KeyBindings _keyBindings;
    
    [Space]
    [SerializeField] private MapController _mapController;
    private void Awake()
    { 
        PrepareData();
        InputManager.Initialize(_keyBindings);
        _mapController.Initialize(_mapData);
        InitializeUnits();
    }
    
    private void InitializeUnits()
    {
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

            var controllerFactory = new UnitControllerFactory(unitModel, unitView, _dataProvider);
            var unitController = (UnitController)controllerFactory.Controller;
            unitController.transform.position = unitModel.Position;
            unitController.Subscribe();
        }
    }

    private void PrepareData()
    {
        if (_dataProvider == null)
            _dataProvider = Resources.Load<DataProvider>("GameData/DataProvider");
        
        if (_mapData == null)
            _mapData = Resources.Load<MapData>("GameData/Map/MapData");
        
        if (_unitsStorage == null)
            _unitsStorage = Resources.Load<UnitsStorage>("GameData/Units/UnitsStorage");
        
        if (_keyBindings == null)
            _keyBindings = Resources.Load<KeyBindings>("GameData/Input/KeyBindings");
    }
}