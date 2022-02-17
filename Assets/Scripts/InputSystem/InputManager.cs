using UnityEngine;

public class InputManager
{
    public static InputManager Instance;

    private static KeyBindings _keyBindings;
    
    public bool Select => Input.GetKeyDown(_keyBindings.Select);
    public bool MoveAction => Input.GetKeyDown(_keyBindings.Action);
    public bool PrepareToAttack => Input.GetKeyDown(_keyBindings.PrepareToAttack);
    public bool Deselect => Input.GetKeyDown(_keyBindings.Deselect);
    public bool Minimap => Input.GetKeyDown(_keyBindings.Minimap);
    public bool Highlight => Input.GetKey(_keyBindings.Highlighting);
    
    public static void Initialize(KeyBindings keyBindings)
    {
        Instance = new InputManager();
        _keyBindings = keyBindings;
    }

    public T OnUnitHover<T>() where T : class
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out var hit) ? hit.transform.GetComponent<T>() : null;
    }
    
    public bool TrySelectUnit<T>(out T selectedUnit)
    {
        selectedUnit = default;
        if (Select)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                selectedUnit = hit.transform.GetComponent<T>();
                return true;
            }
        }
        return false;
    }

    public bool IsNeedDeselect<T>()
    {
        if(Select)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return !Physics.Raycast(ray, out var hit) || !hit.transform.TryGetComponent<T>(out _);
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