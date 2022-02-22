using UnityEngine;

public class UnitView: MonoBehaviour, IUnitView
{
    private Renderer[] _renderers;
    
    public Renderer[] Renderers => _renderers;
    
    private void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>();
    }

    public Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }
    
    public void SetOutline(bool outline)
    {
        foreach (var rend in _renderers)
        {
            rend.gameObject.layer = outline ? LayerMask.NameToLayer("Hovered") : LayerMask.NameToLayer("Unit");
        }
    }
}