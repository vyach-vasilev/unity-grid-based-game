using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Units Storage", menuName = "Settings/Unit Storage", order = 1)]
public class UnitsStorage: ScriptableObject
{
    [SerializeField] private List<UnitData> _unitDataList;
    public List<UnitData> UnitDataList => _unitDataList;
}