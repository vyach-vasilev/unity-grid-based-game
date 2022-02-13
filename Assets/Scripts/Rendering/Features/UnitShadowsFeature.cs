using UnityEngine;
using UnityEngine.Rendering.Universal;

public class UnitShadowsFeature: ScriptableRendererFeature
{
    private UnitShadowsPass _pass;
    private Material _material; 
    private DataProvider _dataProvider;
    
    [SerializeField] private RenderPassEvent _renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
    
    public override void Create()
    {
        _pass = new UnitShadowsPass(_material, _dataProvider);
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
            _material = Resources.Load<Material>("Materials/Shadows");
    }

    public void OnDisable()
    {
        _material = null;
    }
}