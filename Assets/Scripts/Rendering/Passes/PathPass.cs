using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PathPass : ScriptableRenderPass
{
    private readonly string _profilerTag = "Path Feature";

    private readonly Material _waypointMaterial;
    private readonly Material _selectionMaterial;
    private readonly Mesh _pathMesh;
    private readonly float _offsetY;
    private readonly DataProxy _dataProxy;
    
    public PathPass(Material waypointMaterial, Material selectionMaterial, Mesh pathMesh, float offsetY, DataProxy dataProxy)
    {
        _waypointMaterial = waypointMaterial;
        _selectionMaterial = selectionMaterial;
        _pathMesh = pathMesh;
        _offsetY = offsetY;
        _dataProxy = dataProxy;
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
        if (!TryGetUnitController(out var pathController))
        {
            return;
        }

        var path = pathController.AvailablePath;
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
        if (!TryGetUnitController(out var pathController))
        {
            return;
        }
        
        var path = pathController.WaypointsPath;
        var nodePosition = InputManager.Instance.GetWorldNodePosition();

        if (path == null || path.Count <= 0 || nodePosition == Vector3.down) return;

        for (var i = 0; i < path.Count - 1; i++)
        {
            var waypoint = path[i];
            waypoint.y = _offsetY;
            var matrix = Matrix4x4.Translate(waypoint);
            buffer.DrawMesh(_pathMesh, matrix, _waypointMaterial, 0, 0);
        }
    }

    private bool TryGetUnitController(out UnitPathController pathController)
    {
        // TODO: подумать как отрефакторить. Надо получать pathController без всех этих усложнений и get component.
        pathController = null;
        if (_dataProxy.SelectedUnitView == null) return false;

        var unitView = (UnitView)_dataProxy.SelectedUnitView;
        if (unitView == null || !unitView.Selected) return false;

        var unitController = unitView.GetComponent<UnitController>();
        if (unitController.InAttack) return false;
        pathController = unitController.PathController;
        return true;
    }
}