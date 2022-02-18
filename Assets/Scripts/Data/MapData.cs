using UnityEngine;

[CreateAssetMenu(fileName = "Map Data", menuName = "Data/Map", order = 0)]
public class MapData : DataStorage
{
    [SerializeField] private LayerMask _unwalkableMask;
    [SerializeField, Min(0.5f)] private float _nodeRadius = 0.5f;
    [SerializeField] private Vector2Int _size = new(10,10);
    [SerializeField] private GameObject _prefab;
    
    public LayerMask UnwalkableMask
    {
        get => _unwalkableMask;
        set => _unwalkableMask = value;
    }

    public float NodeRadius
    {
        get => _nodeRadius;
        set => _nodeRadius = value;
    }

    public Vector2Int Size
    {
        get => _size;
        set => _size = value;
    }

    public Transform Transform { get; set; }
    public GameObject Prefab => _prefab;
}