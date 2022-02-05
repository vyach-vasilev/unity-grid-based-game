using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GridFeature: ScriptableRendererFeature
{
    private GridPass _pass;
    private Material _material;
    private DataProxy _dataProxy;
    
    [SerializeField] private Color _color;
    [SerializeField] private Vector2Int _tiling = new(10, 10);
    [SerializeField, Range(0,1)] private float _thickness = 0.95f;
    [SerializeField] private RenderPassEvent _renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
    
    public override void Create()
    {
        _pass = new GridPass(_color, _tiling, _thickness, _material, _dataProxy);
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
        return _dataProxy && _dataProxy.GridTransform &&_material;
    }

    private void OnEnable()
    {
        if(_material == null)
            _material = CoreUtils.CreateEngineMaterial("Shader Graphs/Grid");
    }

    private void OnDisable()
    {
        _material = null;
    }
}