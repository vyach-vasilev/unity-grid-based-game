using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MinimapFeature: ScriptableRendererFeature
{
    private MinimapPass _pass;
    private DataProxy _dataProxy;
    
    [SerializeField] private Material _material;
    [SerializeField, Range(0.5f, 1)] private float _pointRadius = 0.5f;
    [SerializeField, Range(3, 20)] private int _pointSides = 3;
    [SerializeField] private RenderPassEvent _renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
    
    public override void Create()
    {
        _pass = new MinimapPass(_material, _dataProxy, _pointRadius, _pointSides);
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
        return _material && _dataProxy && _dataProxy.Units != null;
    }
    
    public void OnEnable()
    {
        if (_dataProxy == null)
            _dataProxy = Resources.Load<DataProxy>("GameData/DataProxy");
    }
}