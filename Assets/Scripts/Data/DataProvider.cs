using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataProvider", menuName = "Data/Provider", order = 0)]
public class DataProvider : DataStorage
{
    public IUnitView SelectedUnitView { get; set; }
    public List<UnitController> Units { get; } = new();
    public Queue<CustomTransform> Footsteps { get; } = new();
}