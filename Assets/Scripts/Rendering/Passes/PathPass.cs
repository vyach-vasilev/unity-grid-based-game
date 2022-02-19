using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PathPass : ScriptableRenderPass
{
    private readonly string _profilerTag = "Path Feature";
    private readonly Material _attackMaterial;
    private readonly Material _waypointMaterial;
    private readonly Material _selectionMaterial;
    private readonly Mesh _pathMesh;
    private readonly float _offsetY;
    private readonly DataProvider _dataProvider;
    
    public PathPass(Material attackMaterial, Material waypointMaterial, Material selectionMaterial, Mesh pathMesh, float offsetY, DataProvider dataProvider)
    {
        _attackMaterial = attackMaterial;
        _waypointMaterial = waypointMaterial;
        _selectionMaterial = selectionMaterial;
        _pathMesh = pathMesh;
        _offsetY = offsetY;
        _dataProvider = dataProvider;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        var buffer = CommandBufferPool.Get(_profilerTag);
        using (new ProfilingScope(buffer, new ProfilingSampler(_profilerTag)))
        {
            if (!_dataProvider.SelectedUnit.InAttack)
            {
                DrawHoverNode(buffer);
                DrawPath(buffer);
            }
            else
            {
                DrawAttackAvailableNodes(buffer);
            }
        }

        context.ExecuteCommandBuffer(buffer);
        CommandBufferPool.Release(buffer);
    }

    private void DrawAttackAvailableNodes(CommandBuffer buffer)
    {
        if (!TryGetPathController(out var pathController, true)) return;
        
        var positions = pathController.AttackAvailablePath;
        for (var i = 0; i < positions.Count; i++)
        {
            var position = positions[i];
            position.y = _offsetY;
            var matrix = Matrix4x4.Translate(position);
            buffer.DrawMesh(_pathMesh, matrix, _attackMaterial, 0, 0);
        }
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
        if (!TryGetPathController(out var pathController)) return;
        
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

    private bool TryGetPathController(out UnitPathController pathController, bool inAttack = false)
    {
        pathController = null;
        if (!TryGetUnitController(out var unitController)) return false;
        if(!inAttack && unitController.InAttack) return false;
        pathController = unitController.UnitPathController;
        return true;
    }
    
    private bool TryGetUnitController(out UnitController unitController)
    {
        unitController = null;
        if (_dataProvider.SelectedUnit == null) return false;
        var controller = _dataProvider.SelectedUnit;
        if (controller == null || !controller.Selected) return false;
        unitController = controller;
        return true;
    }
}