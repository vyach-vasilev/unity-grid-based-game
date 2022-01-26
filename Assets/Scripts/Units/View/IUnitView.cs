using UnityEngine;

public interface IUnitView: ISelectable
{
    Vector3 Position { get; set; }
    bool Selected { get; set; }
}