using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDController: MonoBehaviour
{
    private readonly List<UnitPreview> _unitPreviews = new();
    private InputManager _inputManager;
    
    [SerializeField] private DataProvider _dataProvider;
    [SerializeField] private UnitsStorage _unitsStorage;

    [SerializeField] private TextMeshProUGUI _nameField;
    [SerializeField] private TextMeshProUGUI _fractionField;

    [SerializeField] private Camera _minimapCamera;
    [SerializeField] private Camera _previewCamera;
    [SerializeField] private GameObject _minimapOverlay;
    [SerializeField] private GameObject _unitPreview;
    [SerializeField] private Transform _unitPlacer;
    
    private void Awake()
    {
        foreach (var unitData in _unitsStorage.UnitDataList)
        {
            var go = Instantiate(unitData.Preview, _unitPlacer, false);
            go.SetActive(false);
            var preview = go.AddComponent<UnitPreview>();
            preview.Name = unitData.Name;
            preview.Type = unitData.Type;
            _unitPreviews.Add(preview);
        }
        
        _inputManager = InputManager.Instance;
    }
    
    private void Update()
    {
        PreviewHandler();
        MinimapHandler();
        UnitPreviewHandler();
    }

    private void PreviewHandler()
    {
        if (_inputManager.TryUnitSelect<UnitController>(out var unit))
        {
            if (unit == null)
                return;

            var currentUnitPreview = _unitPreviews.Find(u => u.Type == unit.Model.Type);
            var otherPreview = _unitPreviews.Find(u => u.Type != unit.Model.Type);
            currentUnitPreview.gameObject.SetActive(true);
            otherPreview.gameObject.SetActive(false);
            
            _nameField.text = currentUnitPreview.Name;
            _fractionField.text = currentUnitPreview.Type.ToString();
        }
    }
    
    private void MinimapHandler()
    {
        if (_inputManager.GetKeyDown(KeybindingActions.Minimap))
        {
            if (_minimapCamera.gameObject.activeSelf || _minimapOverlay.activeSelf)
            {
                _minimapCamera.gameObject.SetActive(false);
                _minimapOverlay.SetActive(false);
            }
            else
            {
                _minimapCamera.gameObject.SetActive(true);
                _minimapOverlay.SetActive(true);
            }
        }
    }
    
    private void UnitPreviewHandler()
    {
        if (_dataProvider.SelectedUnit != null && !_unitPreview.activeSelf)
        {
            _previewCamera.gameObject.SetActive(true);
            _unitPreview.SetActive(true);
        }
        else if(_dataProvider.SelectedUnit == null && _unitPreview.activeSelf)
        {
            _previewCamera.gameObject.SetActive(false);
            _unitPreview.SetActive(false);
        }
    }
}