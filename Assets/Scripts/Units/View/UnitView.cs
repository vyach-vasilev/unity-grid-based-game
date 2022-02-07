using System;
using UnityEngine;

public class UnitView: MonoBehaviour, IUnitView
{
    private bool _selected;
    private Renderer _renderer;
    
    public event EventHandler<UnitSelectEvent> OnSelect = (_, _) => {};
    public event EventHandler<UnitSelectEvent> OnDeselect = (_, _) => {};

    public Renderer Renderer => _renderer;
    
    private void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
    }

    public Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }
    
    public Quaternion Rotation
    {
        get => transform.rotation;
        set => transform.rotation = value;
    }
    
    public bool Selected
    {
        get => _selected;
        set
        {
            _selected = value;
            var evt = new UnitSelectEvent();
            if (_selected)
            {
                OnSelect.Invoke(this, evt);
            }
            else
            {
                OnDeselect.Invoke(this, evt);
            }
        }
    }
    
    public void SetOutline(bool outline)
    {
        _renderer.gameObject.layer = outline ? LayerMask.NameToLayer("Hovered") : LayerMask.NameToLayer("Unit");
    }
}