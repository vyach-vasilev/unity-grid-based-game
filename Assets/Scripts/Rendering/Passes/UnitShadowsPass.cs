﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class UnitShadowsPass: ScriptableRenderPass
{
    private readonly string _profilerTag = "UnitShadows Feature";
    private readonly Material _material;
    private readonly Dictionary<int, UnitView> _views;
    
    public UnitShadowsPass(Material material, DataProxy dataProxy)
    {
        if (dataProxy == null)
            return;
        
        _material = material;
        _views = dataProxy.Units;
    }
    
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        var buffer = CommandBufferPool.Get(_profilerTag);
        using (new ProfilingScope(buffer, new ProfilingSampler(_profilerTag)))
        {
            var mesh = ResourcesUtilities.GetDefaultPrimitiveMesh(PrimitiveType.Quad);
            
            foreach (var view in _views.Values)
            {
                if (view == null)
                    return;

                var position = view.Position + Vector3.up * 0.01f;
                var rotation = Quaternion.Euler(-90, 0, 0);
                var matrix = Matrix4x4.TRS(position, rotation, Vector3.one);
                buffer.DrawMesh(mesh, matrix, _material);
            }
        }
        context.ExecuteCommandBuffer(buffer);
        CommandBufferPool.Release(buffer);    
    }
}