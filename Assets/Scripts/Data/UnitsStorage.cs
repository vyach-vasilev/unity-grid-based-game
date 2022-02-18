using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Units Storage", menuName = "Data/Unit Storage", order = 1)]
public class UnitsStorage: DataStorage
{
    [SerializeField] private List<UnitData> _unitDataList;
    public List<UnitData> UnitDataList => _unitDataList;
}