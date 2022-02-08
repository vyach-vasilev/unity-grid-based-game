using System;
using System.Collections.Generic;
using UnityEngine;

public class GuiPreviewManager : MonoBehaviour
{
    [SerializeField] private UnitsStorage _unitsStorage;
    [SerializeField] private Transform _unitPlacer;

    private readonly List<UnitPreview> _unitPreviews = new();
    public event EventHandler<UnitSelectionEvent> OnSelect = (_, _) => {};

    private void Awake()
    {
        var units = _unitsStorage.UnitDataList;
        foreach (var unitData in units)
        {
            var go = Instantiate(unitData.Preview, _unitPlacer, false);
            go.SetActive(false);
            var preview = go.AddComponent<UnitPreview>();
            preview.Name = unitData.Name;
            preview.Type = unitData.Type;
            _unitPreviews.Add(preview);
        }
    }

    private void LateUpdate()
    {
        if (InputManager.Instance.TrySelectUnit<UnitController>(out var unit))
        {
            if (unit == null)
                return;

            var currentPreview = _unitPreviews.Find(u => u.Type == unit.Model.Type);
            var otherPreview = _unitPreviews.Find(u => u.Type != unit.Model.Type);
            currentPreview.gameObject.SetActive(true);
            otherPreview.gameObject.SetActive(false);
            OnSelect(currentPreview, new UnitSelectionEvent());
        }
    }
}