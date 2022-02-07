﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SeeThoughPass: ScriptableRenderPass
{
    private readonly string _profilerTag = "SeeThrough Feature";
    private readonly Material _material;
    private readonly Dictionary<int, UnitView> _views;
    
    public SeeThoughPass(Material material, DataProxy dataProxy)
    {
        _material = material;
        _views = dataProxy.Units;
    }
    
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        var buffer = CommandBufferPool.Get(_profilerTag);
        using (new ProfilingScope(buffer, new ProfilingSampler(_profilerTag)))
        {
            foreach (var view in _views.Values)
            {
                var renderer = view.Renderer;
                if(renderer == null) return;
                buffer.DrawRenderer(renderer, _material, 0, 0);
            }
            
            foreach (var view in _views.Values)
            {
                var renderer = view.Renderer;
                if(renderer == null) return;
                buffer.DrawRenderer(renderer, _material, 0, 1);
            }
        }
        context.ExecuteCommandBuffer(buffer);
        CommandBufferPool.Release(buffer);    
    }
}