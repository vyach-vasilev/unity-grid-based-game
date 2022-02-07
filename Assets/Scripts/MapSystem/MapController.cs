using UnityEngine;

public class MapController : MonoBehaviour
{
    private NodeMap _map;
    private Pathfinder _pathfinder;
    private PathRequestManager _pathRequestManager;
    
    [SerializeField] private LayerMask _unwalkableMask;
    [SerializeField, Min(0.5f)] private float _nodeRadius = 0.5f;
    [SerializeField] private Vector2Int _mapSize = new(10,10);
    
    [Space]
    [SerializeField] private DataProxy _dataProxy;

    private void Awake()
    {
        CheckData();
        NodeMap.Create(_mapSize, transform.position, _nodeRadius, _unwalkableMask);
        _pathfinder = gameObject.AddComponent<Pathfinder>();
        _pathfinder.Setup(NodeMap.Instance);
        _dataProxy.GridTransform = transform;
    }
    
    private void CheckData()
    {
        if (_dataProxy == null)
            _dataProxy = Resources.Load<DataProxy>("GameData/DataProxy");
        
    }
}
