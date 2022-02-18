using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FootstepFeature: ScriptableRendererFeature
{
    private FootstepPass _footstepPass;
    private DataProvider _dataProvider;
    
    [SerializeField] private Material _material;
    [SerializeField] private RenderPassEvent _renderPassEvent = RenderPassEvent.AfterRendering;

    public override void Create()
    {
        _footstepPass = new FootstepPass(_dataProvider, _material);
        _footstepPass.renderPassEvent = _renderPassEvent;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (!IsValid())
        {
            return;
        }
        renderer.EnqueuePass(_footstepPass);
    }
    
    private void OnEnable()
    {
        if (_dataProvider == null)
            _dataProvider = Resources.Load<DataProvider>("GameData/DataProvider");
    }
    
    private bool IsValid()
    {
        return _material && _dataProvider.Footsteps != null && _dataProvider.Footsteps.Count > 0;
    }
}