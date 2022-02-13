using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SeeThroughFeature: ScriptableRendererFeature
{
    private SeeThoughPass _pass;
    private Material _material; 
    private DataProvider _dataProvider;
    
    [SerializeField] private RenderPassEvent _renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
    
    public override void Create()
    {
        if (_dataProvider == null)
        {
            return;
        }
        _pass = new SeeThoughPass(_material, _dataProvider);
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
        return _dataProvider && _material && _dataProvider.Units is { Count: > 0 };
    }
    
    public void OnEnable()
    {
        if (_dataProvider == null)
            _dataProvider = Resources.Load<DataProvider>("GameData/DataProxy");
        
        if(_material == null)
            _material = CoreUtils.CreateEngineMaterial("Hidden/SeeThrough");    
    }

    public void OnDisable()
    {
        _material = null;
    }
}