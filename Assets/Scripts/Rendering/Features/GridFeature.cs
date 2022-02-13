using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GridFeature: ScriptableRendererFeature
{
    private GridPass _pass;
    private Material _material;

    [SerializeField] private MapData _mapData;
    [SerializeField] private Color _color;
    [SerializeField, Range(0,1)] private float _thickness = 0.95f;
    [SerializeField] private RenderPassEvent _renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
    
    public override void Create()
    {
        _pass = new GridPass(_color, _thickness, _material, _mapData);
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
        return _mapData && _mapData.Transform &&_material;
    }

    private void OnEnable()
    {
        if (_mapData == null)
            _mapData = Resources.Load<MapData>("GameData/MapData");
        
        if(_material == null)
            _material = CoreUtils.CreateEngineMaterial("Shader Graphs/Grid");
    }

    private void OnDisable()
    {
        _material = null;
    }
}