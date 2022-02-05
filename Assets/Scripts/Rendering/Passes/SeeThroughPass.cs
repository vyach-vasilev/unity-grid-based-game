using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SeeThoughPass: ScriptableRenderPass
{
    private readonly string _profilerTag = "SeeThrough Feature";
    private readonly Material _material;
    private readonly Dictionary<int, UnitController> _unitsCollection;
    
    public SeeThoughPass(Material material, Dictionary<int, UnitController> unitsCollection)
    {
        _material = material;
        _unitsCollection = unitsCollection;
    }
    
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        var buffer = CommandBufferPool.Get(_profilerTag);

        using (new ProfilingScope(buffer, new ProfilingSampler(_profilerTag)))
        {
            foreach (var unit in _unitsCollection.Values)
            {
                if(unit.View.Renderer == null) return;
                
                buffer.DrawRenderer(unit.View.Renderer, _material, 0, 0);
            }
            
            foreach (var unit in _unitsCollection.Values)
            {
                if(unit.View.Renderer == null) return;
                
                buffer.DrawRenderer(unit.View.Renderer, _material, 0, 1);
            }
        }
        context.ExecuteCommandBuffer(buffer);
        CommandBufferPool.Release(buffer);    
    }
}