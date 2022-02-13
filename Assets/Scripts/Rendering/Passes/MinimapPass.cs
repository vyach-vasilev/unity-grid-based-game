using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MinimapPass: ScriptableRenderPass
{
    private readonly string _profilerTag = "Minimap Pass";
    private readonly List<UnitController> _units;
    private readonly float _pointRadius;
    private readonly int _pointSides;
    
    private Material _friendMaterial;
    private Material _enemyMaterial;
    private Color _friendColor;
    private Color _enemyColor;
    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

    public MinimapPass(DataProvider dataProvider, float pointRadius, int pointSides)
    {
        _pointRadius = pointRadius;
        _pointSides = pointSides;
        _units = dataProvider.Units;
    }

    public void Setup(Material friendMaterial, Material enemyMaterial, Color friendColor, Color enemyColor)
    {
        _friendMaterial = friendMaterial;
        _enemyMaterial = enemyMaterial;
        _friendColor = friendColor;
        _enemyColor = enemyColor;
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
                var view = unit.View;
                var model = unit.Model;
                
                if (view == null || model == null)
                    return;

                var position = view.Position + offset;
                DrawMarker(buffer, mesh, position, model.Type);
            }
        }
        
        context.ExecuteCommandBuffer(buffer);
        CommandBufferPool.Release(buffer);
    }

    private void DrawMarker(CommandBuffer buffer, Mesh mesh, Vector3 position, UnitType type)
    {
        var material = type == UnitType.Friendly ? _friendMaterial : _enemyMaterial;
        material.SetColor(BaseColorId, type == UnitType.Friendly ? _friendColor : _enemyColor);
        var matrix = Matrix4x4.TRS(position, Quaternion.identity, Vector3.one);
        buffer.DrawMesh(mesh, matrix, material);
    }
}