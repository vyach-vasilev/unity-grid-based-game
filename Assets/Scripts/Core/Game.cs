using System.Collections.Generic;
using UnityEngine;

public class Game: MonoBehaviour
{
    private ModuleController<DataStorage, ModuleType> _moduleController;
    private List<Module<DataStorage, ModuleType>> _modules;

    [SerializeField] private DataProvider _dataProvider;
    [SerializeField] private MapData _mapData;
    [SerializeField] private UnitsStorage _unitsStorage;
    [SerializeField] private KeyBindings _keyBindings;
    [SerializeField] private HudData _hudData;

    public ModuleController<DataStorage, ModuleType> ModuleController => _moduleController;
    
    private void Awake()
    {
        _moduleController = new ModuleController<DataStorage, ModuleType>(_dataProvider);

        var inputModule = new InputModule(ModuleType.Input, _keyBindings);
        var mapModule = new MapModule(ModuleType.Map, _mapData);
        var unitModule = new UnitModule(ModuleType.Unit, _unitsStorage);
        var hudModule = new HudModule(ModuleType.HUD, _hudData, _unitsStorage, this);

        _modules = new List<Module<DataStorage, ModuleType>>
        { 
            inputModule,
            mapModule,
            unitModule,
            hudModule,
        };
        
        _moduleController.Register(_modules);
    }

    private void Update()
    {
        _moduleController.Update(_modules);
    }
    
    private void OnDestroy()
    {
        _moduleController.Unregister(_modules);
    }
}