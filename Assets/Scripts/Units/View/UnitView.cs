using UnityEngine;

public class UnitView: MonoBehaviour, IUnitView
{
    private Renderer _renderer;
    
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
    
    public void SetOutline(bool outline)
    {
        _renderer.gameObject.layer = outline ? LayerMask.NameToLayer("Hovered") : LayerMask.NameToLayer("Unit");
    }
}