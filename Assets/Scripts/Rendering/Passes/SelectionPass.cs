using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SelectionPass: ScriptableRenderPass
{
 private readonly string _profilerTag = "Selection Feature";

    private readonly Material _material;
    private readonly Mesh _mesh;
    private readonly DataProvider _dataProvider;
    private readonly float _offset;
    
    public SelectionPass(Material material, Mesh mesh, DataProvider dataProvider, float offset)
    {
        _material = material;
        _mesh = mesh;
        _dataProvider = dataProvider;
        _offset = offset;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        var buffer = CommandBufferPool.Get(_profilerTag);
        using (new ProfilingScope(buffer, new ProfilingSampler(_profilerTag)))
        {
            var unit = _dataProvider.SelectedUnit;
            
            if (unit != null && !unit.Selected) return;
            if (unit == null) return;
            
            var matrix = Matrix4x4.TRS(unit.View.Position + Vector3.up * _offset, Quaternion.identity, Vector3.one);
            buffer.DrawMesh(_mesh, matrix, _material, 0, 0);
        }

        context.ExecuteCommandBuffer(buffer);
        CommandBufferPool.Release(buffer);
    }
}