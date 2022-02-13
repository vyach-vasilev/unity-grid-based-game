using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SelectionFeature: ScriptableRendererFeature
{
    private SelectionPass _pass;
    private DataProvider _dataProvider;
    
    [SerializeField] private Mesh _mesh;
    [SerializeField] private Material _material;
    [SerializeField, Range(0.01f, 1f)] private float _offset = 0.01f;
    [SerializeField] private RenderPassEvent _renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
    
    public override void Create()
    {
        _pass = new SelectionPass(_material, _mesh, _dataProvider, _offset);
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
            _material && _mesh && _dataProvider && _dataProvider.SelectedUnitView != null;
    }
    
    public void OnEnable()
    {
        if (_dataProvider == null)
            _dataProvider = Resources.Load<DataProvider>("GameData/DataProxy");
    }
}