using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PathPass: ScriptableRenderPass
{
    private readonly string _profilerTag = "Path Feature";
    
    private readonly Material _waypointMaterial;
    private readonly Material _selectionMaterial;
    private readonly Mesh _pathMesh;
    private readonly float _offsetY;
    private readonly DataTransmitter _dataTransmitter;
    
    public PathPass(Material waypointMaterial, Material selectionMaterial, Mesh pathMesh, float offsetY, DataTransmitter dataTransmitter)
    {
        _waypointMaterial = waypointMaterial;
        _selectionMaterial = selectionMaterial;
        _pathMesh = pathMesh;
        _offsetY = offsetY;
        _dataTransmitter = dataTransmitter;
    }
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        var buffer = CommandBufferPool.Get(_profilerTag);
        using (new ProfilingScope(buffer, new ProfilingSampler(_profilerTag)))
        {
            DrawHoverNode(buffer);
            DrawPath(buffer);
        }
        context.ExecuteCommandBuffer(buffer);
        CommandBufferPool.Release(buffer);    
    }

    private void DrawAvailableNodes(CommandBuffer buffer)
    {
        
    }
    
    private void DrawHoverNode(CommandBuffer buffer)
    {
        var position = InputManager.Instance.GetWorldNodePosition();
        if (position == Vector3.down) return;
        position.y = _offsetY;
        var matrix = Matrix4x4.Translate(position);
        buffer.DrawMesh(_pathMesh, matrix, _selectionMaterial, 0, 0);
    }
    private void DrawPath(CommandBuffer buffer)
    {
        if (_dataTransmitter.SelectedUnitView == null)
        {
            return;
        }
        var unitView = (UnitView)_dataTransmitter.SelectedUnitView;

        if (unitView == null || !unitView.Selected)
        {
            return;
        }
        
        var unitController = unitView.GetComponent<UnitController>();
        var pathController = unitController.PathController;
        var path = pathController.Path;
        
        var nodePosition = InputManager.Instance.GetWorldNodePosition();
        
        if (path == null || path.Count <= 0 || nodePosition == Vector3.down)
        {
            return;
        }
        
        for (var i = 0; i < path.Count - 1; i++)
        {
            var waypoint = path[i];
            waypoint.y = _offsetY;
            var matrix = Matrix4x4.Translate(waypoint);
            buffer.DrawMesh(_pathMesh, matrix, _waypointMaterial, 0, 0);
        }
    }
}