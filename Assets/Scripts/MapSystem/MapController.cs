using UnityEngine;

public class MapController : MonoBehaviour
{
    private NodeMap _map;
    private Pathfinder _pathfinder;
    private PathRequestManager _pathRequestManager;

    [SerializeField] private MapData _mapData;

    private void Awake()
    {
        CheckData();
        NodeMap.Create(_mapData.Size, transform.position, _mapData.NodeRadius, _mapData.UnwalkableMask);
        _pathfinder = gameObject.AddComponent<Pathfinder>();
        _pathfinder.Setup(NodeMap.Instance);
        _mapData.Transform = transform;
    }
    
    private void CheckData()
    {
        if (_mapData == null)
            _mapData = Resources.Load<MapData>("GameData/MapData");
        
    }
}
