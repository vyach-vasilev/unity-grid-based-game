using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MinimapFeature: ScriptableRendererFeature
{
    private static readonly int SurfaceId = Shader.PropertyToID("_Surface");

    private MinimapPass _pass;
    private DataProvider _dataProvider;
    private Material _friendMaterial;
    private Material _enemyMaterial;
    
    [SerializeField] private Color _friendColor = Color.green;
    [SerializeField] private Color _enemyColor = Color.red;
    
    [SerializeField, Range(0.5f, 1)] private float _pointRadius = 0.5f;
    [SerializeField, Range(3, 20)] private int _pointSides = 3;
    [SerializeField] private RenderPassEvent _renderPassEvent = RenderPassEvent.AfterRenderingOpaques;

    public override void Create()
    {
        if(_dataProvider == null)
            return;
        
        _pass = new MinimapPass(_dataProvider, _pointRadius, _pointSides);
        _pass.renderPassEvent = _renderPassEvent;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (!IsValid())
        {
            return;
        }
        _pass.Setup(_friendMaterial, _enemyMaterial, _friendColor, _enemyColor);
        renderer.EnqueuePass(_pass);
    }
    
    private bool IsValid()
    {
        return _friendMaterial && _enemyMaterial && _dataProvider && _dataProvider.Units != null;
    }
    
    public void OnEnable()
    {
        if (_dataProvider == null)
            _dataProvider = Resources.Load<DataProvider>("GameData/DataProxy");

        if (_friendMaterial == null)
        {
            _friendMaterial = CoreUtils.CreateEngineMaterial("Universal Render Pipeline/Unlit");
            _friendMaterial.SetFloat(SurfaceId, 1);
        }
        
        if (_enemyMaterial == null)
        {
            _enemyMaterial = CoreUtils.CreateEngineMaterial("Universal Render Pipeline/Unlit");
            _enemyMaterial.SetFloat(SurfaceId, 1);
        }
    }
}