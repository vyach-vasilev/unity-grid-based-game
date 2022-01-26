using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MapFeature: ScriptableRendererFeature
{
    private MapPass _pass;

    [SerializeField] private DataTransmitter _dataTransmitter;
    [SerializeField] private Mesh _pathMesh;
    [SerializeField] private Mesh _selectionMesh;
    [SerializeField] private Material _selectionMaterial;
    [SerializeField] private Material _waypointMaterial;
    [SerializeField] private RenderPassEvent _renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
    
    public override void Create()
    {
        _pass = new MapPass(_waypointMaterial,_selectionMaterial, _selectionMesh, _pathMesh, _dataTransmitter);
        _pass.renderPassEvent = _renderPassEvent;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (!IsValid())
        {
            return;
        }
        renderer.EnqueuePass(_pass);
    }
    
    private bool IsValid()
    {
        return
            _selectionMaterial && _waypointMaterial &&
            _pathMesh != null && _selectionMesh &&
            _dataTransmitter != null &&
            InputManager.Instance != null;
    }
}