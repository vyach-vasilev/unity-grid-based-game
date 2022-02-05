using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GridFeature: ScriptableRendererFeature
{
    private GridPass _pass;
    private Material _gridMaterial;
    
    [SerializeField] private DataTransmitter _dataTransmitter;
    [SerializeField] private Color _color;
    [SerializeField] private Vector2Int _tiling = new(10, 10);
    [SerializeField, Range(0,1)] private float _thickness = 0.95f;
    [SerializeField] private RenderPassEvent _renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
    
    public override void Create()
    {
        _pass = new GridPass(_color, _tiling, _thickness, _gridMaterial, _dataTransmitter);
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
        return _dataTransmitter && _dataTransmitter.GridTransform &&_gridMaterial;
    }

    private void OnEnable()
    {
        if(_gridMaterial == null)
            _gridMaterial = CoreUtils.CreateEngineMaterial("Shader Graphs/Grid");
    }

    private void OnDisable()
    {
        _gridMaterial = null;
    }
}