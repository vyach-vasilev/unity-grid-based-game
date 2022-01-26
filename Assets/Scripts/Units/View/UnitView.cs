using System;
using UnityEngine;
using UnityEngine.Rendering;

public class UnitView: MonoBehaviour, IUnitView
{
    private bool _selected;
    private Renderer _renderer;
    private Color _defaultColor;
    
    public event EventHandler<UnitSelectEvent> OnSelect = (_, _) => {};
    public event EventHandler<UnitSelectEvent> OnDeselect = (_, _) => {};

    public Color DefaultColor => _defaultColor;
    
    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _defaultColor = _renderer.material.color;
    }

    public Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
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

    public void SetEmission(bool emission)
    {
        if (emission)
        {
            _renderer.material.EnableKeyword("_EMISSION");
        }
        else
        {
            _renderer.material.DisableKeyword("_EMISSION");
        }
    }
    
    public void ChangeColor(Color color)
    {
        _renderer.material.color = color;
    }
}