using UnityEngine;

public class UnitViewFactory: IUnitViewFactory
{
    public IUnitView View { get; }

    public UnitViewFactory(GameObject source, Transform transform = default)
    {
        var prefab = source;
        var instance = Object.Instantiate(prefab, transform);
        View = instance.GetComponent<IUnitView>();
    }
}