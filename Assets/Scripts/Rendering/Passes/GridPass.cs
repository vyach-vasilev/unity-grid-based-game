using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GridPass: ScriptableRenderPass
{
    private readonly string _profilerTag = "Grid Feature";
    
    private readonly Mesh _gridMesh;
    private readonly Material _gridMaterial;
    private readonly DataTransmitter _dataTransmitter;

    public GridPass(Mesh gridMesh, Material gridMaterial, DataTransmitter dataTransmitter)
    {
        _gridMesh = gridMesh;
        _gridMaterial = gridMaterial;
        _dataTransmitter = dataTransmitter;
    }
    
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        var buffer = CommandBufferPool.Get(_profilerTag);
        using (new ProfilingScope(buffer, new ProfilingSampler(_profilerTag)))
        {
            var position = _dataTransmitter.GridTransform.position;
            var matrix = Matrix4x4.Translate(position);
            buffer.DrawMesh(_gridMesh, matrix, _gridMaterial, 0, 0);
        }
        context.ExecuteCommandBuffer(buffer);
        CommandBufferPool.Release(buffer);    
    }
}