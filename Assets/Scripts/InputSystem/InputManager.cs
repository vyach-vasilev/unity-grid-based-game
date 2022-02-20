using UnityEngine;

public class InputManager
{
    public static InputManager Instance;
    private static KeyBindings _keyBindings;
    
    public bool Select => Input.GetKeyDown(_keyBindings.Select);
    public bool MoveAction => Input.GetKeyDown(_keyBindings.Action);
    
    public bool Skill1 => Input.GetKeyDown(_keyBindings.Skill1);
    public bool Skill2 => Input.GetKeyDown(_keyBindings.Skill2);
    public bool Skill3 => Input.GetKeyDown(_keyBindings.Skill3);
    
    public bool Deselect => Input.GetKeyDown(_keyBindings.Deselect);
    public bool Minimap => Input.GetKeyDown(_keyBindings.Minimap);
    public bool Highlight => Input.GetKey(_keyBindings.Highlighting);
    
    public static void Create(KeyBindings keyBindings)
    {
        Instance = new InputManager();
        _keyBindings = keyBindings;
    }
    
    public T OnUnitHover<T>() where T : class
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out var hit) ? hit.transform.GetComponent<T>() : null;
    }
    
    public T OnUnitSelect<T>() where T : class
    {
        return !Select ? null : OnUnitHover<T>();
    }
    
    public bool TryUnitSelect<T>(out T selectedUnit)
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
            if (!hit.transform.TryGetComponent<Map>(out _)) return Vector3.down;
                
            var point = hit.point;
            var position = NodeMap.Instance.NodeFromWorldPoint(point).WorldPosition;
            return position;
        }
        return Vector3.down;
    }
    
    public bool IsWalkableNode()
    {
        var node = GetWorldNode();
        return node.Walkable;
    }
    
    private Node GetWorldNode()
    {
        var position = GetWorldNodePosition();
        return NodeMap.Instance.NodeFromWorldPoint(position);
    }
}