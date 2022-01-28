using System;
using UnityEngine;

public class InputManager
{
    public static InputManager Instance;
    
    public event Action<Vector3> OnMoved;
    public bool IsMoveLocked { get; set; }

    public static void Initialize()
    {
        Instance = new InputManager();
    }
    
    public bool TryGetUnit(out IUnitView selectedUnit)
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

            if (Physics.Raycast(ray, out var hit))
            {
                if (!hit.transform.TryGetComponent<IUnitView>(out _)) return true;
            }
            else
            {
                return true;
            }
        }

        return false;
    }
    
    public void GetDestinationPosition()
    {
        var position = GetWorldNodePosition();
        OnMoved?.Invoke(position);
    }
    
    public Vector3 GetWorldNodePosition()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hit))
        {
            if (!hit.transform.TryGetComponent<MapController>(out _)) return Vector3.down;
                
            var point = hit.point;
            var map = NodeMap.Instance;
            return map.NodeFromWorldPoint(point).WorldPosition;
        }

        return Vector3.down;
    }
    
    public Quaternion RotateOnMouseDirection(UnitController entity)
    {
        var mousePosition = Camera.main.WorldToScreenPoint(entity.transform.position);
        mousePosition = Input.mousePosition - mousePosition;
        var angle = Mathf.Atan2(mousePosition.y, -mousePosition.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle - 120, Vector3.up);
    }
}