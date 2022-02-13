using System;
using UnityEngine;

public class UnitModel: IUnitModel
{
    private string _name;
    private UnitType _type;
    private Vector3 _position;
    
    public event EventHandler<UnitPositionChangedEvent> OnPositionChanged;
    
    public Vector3 Position
    {
        get => _position;
        set
        {
            if (_position != value)
            {
                _position = value;
                var changedEvent = new UnitPositionChangedEvent();
                OnPositionChanged?.Invoke(this, changedEvent);
            }
        }
    }

    public string Name
    {
        get => _name;
        set => _name = value;
    }

    public UnitType Type
    {
        get => _type;
        set => _type = value;
    }
    
    public UnitModel(): this(default, UnitType.Friendly)
    {
    }

    public UnitModel(string name, UnitType type)
    {
        _name = name;
        _type = type;
    }  
}