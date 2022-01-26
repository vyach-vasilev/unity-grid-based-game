using UnityEngine;

public interface IUnitModel
{
    string Name { get; set; }
    UnitType Type { get; set; }
    Vector3 Position { get; set; }
}