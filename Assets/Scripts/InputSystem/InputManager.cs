using UnityEngine;

public class InputManager
{
    public static InputManager Instance;
    private static Keybindings _keybindings;
    
    public static void Create(Keybindings keybindings)
    {
        Instance = new InputManager();
        _keybindings = keybindings;
    }

    public KeyCode GetKeyForAction(KeybindingActions keybindingAction)
    {
        foreach (var keybindingPair in _keybindings.KeybindingPairs)
        {
            if (keybindingPair.KeybindingAction == keybindingAction)
            {
                return keybindingPair.KeyCode;
            }
        }
        return KeyCode.None;
    }

    public bool GetKeyDown(KeybindingActions keybindingAction)
    {
        foreach (var keybindingPair in _keybindings.KeybindingPairs)
        {
            if (keybindingPair.KeybindingAction == keybindingAction)
            {
                return Input.GetKeyDown(keybindingPair.KeyCode);
            }
        }
        return false;
    }

    public bool GetKeyUp(KeybindingActions keybindingAction)
    {
        foreach (var keybindingPair in _keybindings.KeybindingPairs)
        {
            if (keybindingPair.KeybindingAction == keybindingAction)
            {
                return Input.GetKeyUp(keybindingPair.KeyCode);
            }
        }
        return false;
    }

    public bool GetKey(KeybindingActions keybindingAction)
    {
        foreach (var keybindingPair in _keybindings.KeybindingPairs)
        {
            if (keybindingPair.KeybindingAction == keybindingAction)
            {
                return Input.GetKey(keybindingPair.KeyCode);
            }
        }
        return false;
    }
    
    public T OnUnitHover<T>() where T : class
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out var hit) ? hit.transform.GetComponent<T>() : null;
    }
    
    public T OnUnitSelect<T>() where T : class
    {
        return !GetKeyDown(KeybindingActions.Select) ? null : OnUnitHover<T>();
    }
    
    public bool TryUnitSelect<T>(out T selectedUnit)
    {
        selectedUnit = default;
        if (GetKeyDown(KeybindingActions.Select))
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
        if(GetKeyDown(KeybindingActions.Select))
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