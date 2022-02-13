using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GridPass: ScriptableRenderPass
{
    private static readonly int ColorId = Shader.PropertyToID("_Color");
    private static readonly int TilingId = Shader.PropertyToID("_Tiling");
    private static readonly int ThicknessId = Shader.PropertyToID("_Thickness");
    
    private readonly string _profilerTag = "Grid Feature";
    
    private readonly Color _color;
    private readonly Vector2Int _tiling;
    private readonly float _thickness;
    private readonly Material _material;
    private readonly MapData _mapData;
    
    public GridPass(Color color, float thickness, Material material, MapData mapData)
    {
        _mapData = mapData;
        _color = color;
        _tiling = _mapData.Size;
        _thickness = thickness;
        _material = material;
    }
    
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        var buffer = CommandBufferPool.Get(_profilerTag);
        var mesh = ResourcesUtilities.GetDefaultPrimitiveMesh(PrimitiveType.Plane);

        using (new ProfilingScope(buffer, new ProfilingSampler(_profilerTag)))
        {
            var position = _mapData.Transform.position;
            var matrix = Matrix4x4.Translate(position);
            
            _material.SetColor(ColorId, _color);
            _material.SetVector(TilingId, new Vector4(_tiling.x, _tiling.y));
            _material.SetFloat(ThicknessId, _thickness);
            
            buffer.DrawMesh(mesh, matrix, _material, 0, 0);
        }
        context.ExecuteCommandBuffer(buffer);
        CommandBufferPool.Release(buffer);    
    }
}