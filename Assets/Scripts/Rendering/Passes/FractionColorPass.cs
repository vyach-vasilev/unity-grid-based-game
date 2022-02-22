using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FractionColorPass : ScriptableRenderPass
{
    private readonly string _profilerTag = "Fraction Color Pass";
    private readonly List<UnitController> _units;
    private readonly Material _friendMaterial;
    private readonly Material _enemyMaterial;
    private readonly Color _friendColor;
    private readonly Color _enemyColor;
    
    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

    public FractionColorPass(DataProvider dataProvider, Material friendMaterial, Material enemyMaterial, Color friendColor, Color enemyColor)
    {
        _units = dataProvider.Units;
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
            foreach (var unit in _units)
            {
                var view = unit.View;
                var model = unit.Model;
                
                if (view == null || model == null)
                    return;

                var material = model.Type == UnitType.Friendly ? _friendMaterial : _enemyMaterial;
                material.SetColor(BaseColorId, model.Type == UnitType.Friendly ? _friendColor : _enemyColor);
                foreach (var renderer in view.Renderers)
                {
                    buffer.DrawRenderer(renderer, material);
                }
            }
        }
        
        context.ExecuteCommandBuffer(buffer);
        CommandBufferPool.Release(buffer);
    }
}