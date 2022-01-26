using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GridFeature: ScriptableRendererFeature
{
    private GridPass _pass;
    
    [SerializeField] private DataTransmitter _dataTransmitter;
    [SerializeField] private Mesh _gridMesh;
    [SerializeField] private Material _gridMaterial;
    [SerializeField] private RenderPassEvent _renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
    
    public override void Create()
    {
        _pass = new GridPass(_gridMesh, _gridMaterial, _dataTransmitter);
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
        return _dataTransmitter && _dataTransmitter.GridTransform &&_gridMaterial && _gridMesh;
    }
}