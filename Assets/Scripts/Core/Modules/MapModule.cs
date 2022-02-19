using UnityEngine;

public class MapModule : Module<DataStorage, ModuleType>
{
    private readonly MapData _mapData;
    private GameObject _map;
    
    public MapModule(ModuleType id, MapData mapData) : base(id)
    {
        _mapData = mapData;
    }
    
    public override void Execute(DataStorage data)
    {
        Debug.Log("Execute: " + Id);

        _map = Object.Instantiate(_mapData.Prefab);
        _map.name = "Map";
        
        NodeMap.Create(_mapData.Size, _map.transform.position, _mapData.NodeRadius, _mapData.UnwalkableMask);
        Pathfinder.Create(NodeMap.Instance);
        _mapData.Transform = _map.transform;
    }
}