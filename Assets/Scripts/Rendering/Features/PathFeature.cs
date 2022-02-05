using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PathFeature: ScriptableRendererFeature
{
    private PathPass _pass;
    private DataProxy _dataProxy;
    
    [SerializeField, Min(0.01f)] private float _offsetY = 0.01f;
    [SerializeField] private Mesh _pathMesh;
    [SerializeField] private Material _selectionMaterial;
    [SerializeField] private Material _waypointMaterial;
    [SerializeField] private RenderPassEvent _renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
    
    public override void Create()
    {
        _pass = new PathPass(_waypointMaterial, _selectionMaterial, _pathMesh, _offsetY, _dataProxy);
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
            _selectionMaterial && 
            _waypointMaterial &&
            _pathMesh &&
            _dataProxy &&
            InputManager.Instance != null;
    }
    
    public void OnEnable()
    {
        if (_dataProxy == null)
        {
            _dataProxy = Resources.Load<DataProxy>("GameData/DataProxy");
        }
    }
}