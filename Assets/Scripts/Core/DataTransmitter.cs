using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Storage", order = 0)]
public class DataTransmitter : ScriptableObject
{
    public IUnitView SelectedUnit { get; set; }

}