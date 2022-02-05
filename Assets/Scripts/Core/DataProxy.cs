using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataProxy", menuName = "Storage", order = 0)]
public class DataProxy : ScriptableObject
{
    public IUnitView SelectedUnitView { get; set; }
    public Transform GridTransform { get; set; }
    public Dictionary<int, Renderer> UnitsRenderer { get; } = new();
}