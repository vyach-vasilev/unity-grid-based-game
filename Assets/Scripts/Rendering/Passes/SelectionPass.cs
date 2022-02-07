using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SelectionPass: ScriptableRenderPass
{
 private readonly string _profilerTag = "Selection Feature";

    private readonly Material _material;
    private readonly Mesh _mesh;
    private readonly DataProxy _dataProxy;
    private readonly float _offset;
    
    public SelectionPass(Material material, Mesh mesh, DataProxy dataProxy, float offset)
    {
        _material = material;
        _mesh = mesh;
        _dataProxy = dataProxy;
        _offset = offset;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        var buffer = CommandBufferPool.Get(_profilerTag);
        using (new ProfilingScope(buffer, new ProfilingSampler(_profilerTag)))
        {
            if (_dataProxy.SelectedUnitView is not { Selected: true })
            {
                return;
            }
            var unit = (UnitView)_dataProxy.SelectedUnitView;

            if (unit == null)
            {
                return;
            }
            
            var matrix = Matrix4x4.TRS(unit.Position + Vector3.up * _offset, Quaternion.identity, Vector3.one);
            buffer.DrawMesh(_mesh, matrix, _material, 0, 0);
        }

        context.ExecuteCommandBuffer(buffer);
        CommandBufferPool.Release(buffer);
    }
}