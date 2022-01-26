using UnityEngine;

public class MapController : MonoBehaviour
{
    private NodeMap _map;
    private Pathfinder _pathfinder;
    private PathRequestManager _pathRequestManager;
    
    [SerializeField] private LayerMask _unWalkableMask;
    [SerializeField, Min(0.5f)] private float _nodeRadius = 0.5f;
    [SerializeField] private Vector2Int _mapSize = new(10,10);
    
    [Space]
    [SerializeField] private DataTransmitter _dataTransmitter;

    private void Awake()
    {
        NodeMap.Create(_mapSize, transform.position, _nodeRadius, _unWalkableMask);
        _pathfinder = gameObject.AddComponent<Pathfinder>();
        _pathfinder.Setup(NodeMap.Instance);
        _dataTransmitter.GridTransform = transform;
    }
}
