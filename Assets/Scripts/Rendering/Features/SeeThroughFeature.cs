using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SeeThroughFeature: ScriptableRendererFeature
{
    private SeeThoughPass _pass;
    private Material _material;

    [SerializeField] private DataTransmitter _dataTransmitter;
    [SerializeField] private RenderPassEvent _renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
    
    public override void Create()
    {
        _pass = new SeeThoughPass(_material, _dataTransmitter.UnitsCollection);
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
        return _material && _dataTransmitter != null && _dataTransmitter.UnitsCollection != null && _dataTransmitter.UnitsCollection.Count > 0;
    }
    
    public void OnEnable()
    {
        CreateMaterials();
    }

    public void OnDisable()
    {
        DestroyMaterials();
    }
    
    private void CreateMaterials()
    {
        if(_material == null)
            _material = CoreUtils.CreateEngineMaterial("Hidden/SeeThrough");
    }
    
    private void DestroyMaterials()
    {
        _material = null;
    }
}