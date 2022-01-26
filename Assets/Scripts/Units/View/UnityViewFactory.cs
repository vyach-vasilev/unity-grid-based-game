using UnityEngine;

public class UnitViewFactory: IUnitViewFactory
{
    public IUnitView View { get; }

    public UnitViewFactory(GameObject source, UnitType type = UnitType.Friendly, Transform transform = default)
    {
        var prefab = source;
        var instance = Object.Instantiate(prefab, transform);
        //SetLayer(instance, type);
        View = instance.GetComponent<IUnitView>();
    }

    private void SetLayer(GameObject instance, UnitType type)
    {
        var objects = instance.GetComponentsInChildren<Transform>();
        foreach (var o in objects)
        {
            o.gameObject.layer = LayerMask.NameToLayer(type.ToString());
        }
    }
}