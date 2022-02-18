using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FootstepPass: ScriptableRenderPass
{
    private readonly string _profilerTag = "Footstep Pass";
    private readonly DataProvider _dataProvider;
    private readonly Material _material;
    
    public FootstepPass(DataProvider dataProvider, Material material)
    {
        _dataProvider = dataProvider;
        _material = material;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        var buffer = CommandBufferPool.Get(_profilerTag);
        var mesh = MeshUtility.Quad();
        var matrices = new List<Matrix4x4>(_dataProvider.Footsteps.Count);
        
        using (new ProfilingScope(buffer, new ProfilingSampler(_profilerTag)))
        {
            foreach (var footstep in _dataProvider.Footsteps)
            {
                var matrix = MathUtility.TRS(footstep);
                matrices.Add(matrix);
            }
            buffer.DrawMeshInstanced(mesh, 0, _material, 0, matrices.ToArray(), _dataProvider.Footsteps.Count);
        }
        context.ExecuteCommandBuffer(buffer);
        CommandBufferPool.Release(buffer);    
    }
}