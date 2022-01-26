using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MapPass: ScriptableRenderPass
{
    private readonly string _profilerTag = "NodeMap";
    private readonly Material _waypointMaterial;
    private readonly Material _selectionMaterial;
    private readonly Mesh _selectionMesh;
    private readonly Mesh _pathMesh;
    private readonly DataTransmitter _dataTransmitter;
    
    public MapPass(Material waypointMaterial, Material selectionMaterial, Mesh selectionMesh, Mesh pathMesh, DataTransmitter dataTransmitter)
    {
        _waypointMaterial = waypointMaterial;
        _selectionMaterial = selectionMaterial;
        _selectionMesh = selectionMesh;
        _pathMesh = pathMesh;
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
    private void DrawHoverNode(CommandBuffer buffer)
    {
        var position = InputManager.Instance.GetWorldNodePosition();
        if (position == Vector3.down) return;
        position.y = 0.01f;
        var matrix = Matrix4x4.Translate(position);
        buffer.DrawMesh(_selectionMesh, matrix, _selectionMaterial, 0, 0);
    }
    private void DrawPath(CommandBuffer buffer)
    {
        if (_dataTransmitter.SelectedUnit == null || !_dataTransmitter.SelectedUnit.Selected)
        {
            return;
        }
        var selectedUnit = (UnitView)_dataTransmitter.SelectedUnit;

        if (selectedUnit == null)
        {
            return;
        }
        
        var unitController = selectedUnit.GetComponent<UnitController>();

        if (unitController == null)
        {
            return;
        }
        
        var pathController = unitController.PathController;
        var path = pathController.Path;
        
        var nodePosition = InputManager.Instance.GetWorldNodePosition();
        
        if (path == null || path.Count <= 0 || nodePosition == Vector3.down)
        {
            return;
        }
        
        for (var i = 0; i < path.Count; i++)
        {
            var waypoint = path[i];
            waypoint.y = 0.01f;
            var matrix = Matrix4x4.Translate(waypoint);
            buffer.DrawMesh(_pathMesh, matrix, _waypointMaterial, 0, 0);
        }
    }
}