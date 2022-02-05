using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SeeThroughFeature: ScriptableRendererFeature
{
    private SeeThoughPass _pass;
    private Material _material; 
    private DataProxy _dataProxy;
    
    [SerializeField] private RenderPassEvent _renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
    
    public override void Create()
    {
        if (_dataProxy == null)
        {
            return;
        }
        _pass = new SeeThoughPass(_material, _dataProxy);
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
        return _dataProxy && _material && _dataProxy.UnitsRenderer is { Count: > 0 };
    }
    
    public void OnEnable()
    {
        if (_dataProxy == null)
        {
            _dataProxy = Resources.Load<DataProxy>("GameData/DataProxy");
        }
        if(_material == null)
            _material = CoreUtils.CreateEngineMaterial("Hidden/SeeThrough");    
    }

    public void OnDisable()
    {
        _material = null;
    }
}