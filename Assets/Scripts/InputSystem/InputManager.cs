using UnityEngine;

public class InputManager
{
    public static InputManager Instance;
    
    public bool CanMove => Input.GetMouseButtonDown(1);
    public bool CanAttack => Input.GetKeyDown(KeyCode.Space);
    public bool DeselectAll => Input.GetKeyDown(KeyCode.Escape);
    
    public static void Initialize()
    {
        Instance = new InputManager();
    }

    public IUnitView OnUnitHover()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out var hit) ? hit.transform.GetComponent<IUnitView>() : null;
    }
    
    public bool TrySelectUnit(out IUnitView selectedUnit)
    {
        selectedUnit = default;
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                selectedUnit = hit.transform.GetComponent<IUnitView>();
                return true;
            }
        }
        return false;
    }

    public bool IsNeedDeselect()
    {
        if(Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return !Physics.Raycast(ray, out var hit) || !hit.transform.TryGetComponent<IUnitView>(out _);
        }
        return false;
    }
    
    public Vector3 GetWorldNodePosition()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hit))
        {
            if (!hit.transform.TryGetComponent<MapController>(out _)) return Vector3.down;
                
            var point = hit.point;
            var position = NodeMap.Instance.NodeFromWorldPoint(point).WorldPosition;
            return position;
        }

        return Vector3.down;
    }

    public Node GetWorldNode()
    {
        var position = GetWorldNodePosition();
        return NodeMap.Instance.NodeFromWorldPoint(position);
    }
    
    public bool IsWalkableNode()
    {
        var node = GetWorldNode();
        return node.Walkable;
    }
    
    public Quaternion RotateOnMouseDirection(UnitController entity)
    {
        var mousePosition = Camera.main.WorldToScreenPoint(entity.transform.position);
        mousePosition = Input.mousePosition - mousePosition;
        var angle = Mathf.Atan2(mousePosition.y, -mousePosition.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle - 120, Vector3.up);
    }
}