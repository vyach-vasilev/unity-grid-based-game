using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OutlineFeature : ScriptableRendererFeature
{
    private OutlinePass _pass;
    private Material _outlineMaterial;
    private Material _colorMaterial;
    
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Color _color = Color.white;
    [SerializeField] [Range(1, 32)] private int _width = 4;
    [SerializeField] private RenderPassEvent _renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
    [SerializeField] private string[] _shaderPassNames;
    
    private bool IsValid => _colorMaterial && _outlineMaterial;
    
    public override void Create()
    {
        _pass = new OutlinePass(_layerMask, _color, _width, _colorMaterial, _outlineMaterial, _shaderPassNames);
        _pass.renderPassEvent = _renderPassEvent;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (!IsValid)
        {
            return;
        }
        
        _pass.Setup(renderer);
        renderer.EnqueuePass(_pass);
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
        if(_colorMaterial == null)
            _colorMaterial = CoreUtils.CreateEngineMaterial("Hidden/Color");
        
        if(_outlineMaterial == null)
            _outlineMaterial = CoreUtils.CreateEngineMaterial("Hidden/Outline");
    }

    private void DestroyMaterials()
    {
        _colorMaterial = null;
        _outlineMaterial = null;
    }
}