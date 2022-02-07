using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MinimapPass: ScriptableRenderPass
{
    private readonly string _profilerTag = "Minimap Pass";
    private readonly Material _material;
    private readonly Dictionary<UnitModel, UnitView> _units;
    private readonly float _pointRadius;
    private readonly int _pointSides;
    
    public MinimapPass(Material material, DataProxy dataProxy, float pointRadius, int pointSides)
    {
        _material = material;
        _pointRadius = pointRadius;
        _pointSides = pointSides;
        _units = dataProxy.Units;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        var buffer = CommandBufferPool.Get(_profilerTag);
        
        using (new ProfilingScope(buffer, new ProfilingSampler(_profilerTag)))
        {
            var mesh = MeshUtility.Circle(_pointRadius, _pointSides);
            var offset = Vector3.one * 0.01f;
            
            foreach (var unit in _units)
            {
                var view = unit.Value;
                var model = unit.Key;
                
                if (view == null || model == null)
                    return;

                _material.SetColor("_BaseColor", model.Type == UnitType.Enemy ? Color.red : Color.green);

                var matrix = Matrix4x4.TRS(view.Position + offset, Quaternion.identity, Vector3.one);
                buffer.DrawMesh(mesh, matrix, _material);
            }
        }
        
        context.ExecuteCommandBuffer(buffer);
        CommandBufferPool.Release(buffer);
    }

    // private void UpdateMaterialColor()
    // {
    //     var _propBlock = new MaterialPropertyBlock();
    //     var _renderer = GetComponent<Renderer>();
    //     _renderer.GetPropertyBlock(_propBlock);
    //     _propBlock.SetColor("_BaseColor", Color.blue);
    //     _renderer.SetPropertyBlock(_propBlock);
    // }
}