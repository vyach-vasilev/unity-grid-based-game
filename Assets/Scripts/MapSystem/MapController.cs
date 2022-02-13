using UnityEngine;

public class MapController: MonoBehaviour
{
    public void Initialize(MapData mapData)
    {
        NodeMap.Create(mapData.Size, transform.position, mapData.NodeRadius, mapData.UnwalkableMask);
        Pathfinder.Create(NodeMap.Instance);
        mapData.Transform = transform;
    }
}
