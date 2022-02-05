using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Storage", order = 0)]
public class DataTransmitter : ScriptableObject
{
    public IUnitView SelectedUnitView { get; set; }
    public Transform GridTransform { get; set; }
    public Dictionary<int, UnitController> UnitsCollection { get; } = new();
}