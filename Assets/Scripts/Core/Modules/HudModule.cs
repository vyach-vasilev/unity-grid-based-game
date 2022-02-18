using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

public class HudModule: Module<DataStorage, ModuleType>
{
    private readonly HudData _hudData;
    private readonly Game _game;
    private readonly UnitsStorage _unitsStorage;
    private readonly List<UnitPreview> _unitPreviews = new();
    
    private Canvas _canvas;
    private EventSystem _eventSystem;
    private Camera _minimapCamera;
    private Camera _previewCamera;
    private GameObject _minimapOverlay;
    private GameObject _unitPreview;

    private Transform _unitPlacer;
    private UnitPreviewPanel _previewPanel;

    public event EventHandler<UnitSelectionEvent> OnSelect = (_, _) => {};

    public HudModule(ModuleType id, HudData hudData, UnitsStorage unitsStorage, Game game) : base(id)
    {
        _hudData = hudData;
        _game = game;
        _unitsStorage = unitsStorage;
    }
    
    public override void Execute(DataStorage data)
    {
        var o = new GameObject();
        
        o.name = "Canvas";
        
        _canvas = Object.Instantiate(_hudData.Canvas);
        _eventSystem = Object.Instantiate(_hudData.EventSystem);
        _minimapCamera = Object.Instantiate(_hudData.MinimapCamera);
        _previewCamera = Object.Instantiate(_hudData.PreviewCamera);
        
        _minimapOverlay = Object.Instantiate(_hudData.MinimapOverlay, _canvas.transform, true);
        _unitPreview = Object.Instantiate(_hudData.UnitPreview, _canvas.transform, true);
        _previewPanel = _unitPreview.GetComponent<UnitPreviewPanel>();
        _previewPanel.Game = _game;
        
        _unitPlacer = Object.Instantiate(_hudData.UnitPlacer);
        
        foreach (var unitData in _unitsStorage.UnitDataList)
        {
            var go = Object.Instantiate(unitData.Preview, _unitPlacer, false);
            go.SetActive(false);
            var preview = go.AddComponent<UnitPreview>();
            preview.Name = unitData.Name;
            preview.Type = unitData.Type;
            _unitPreviews.Add(preview);
        }
        
        Debug.Log("Activate " + this);
    }
    
    public override void Update(DataStorage data)
    {
        PreviewHandler();
        MinimapHandler();
        UnitPreviewHandler((DataProvider)data);
    }

    private void PreviewHandler()
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
    
    private void MinimapHandler()
    {
        if (InputManager.Instance.Minimap)
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
    
    private void UnitPreviewHandler(DataProvider dataProvider)
    {
        if (dataProvider.SelectedUnitView != null && !_unitPreview.activeSelf)
        {
            _previewCamera.gameObject.SetActive(true);
            _unitPreview.SetActive(true);
        }
        else if(dataProvider.SelectedUnitView == null && _unitPreview.activeSelf)
        {
            _previewCamera.gameObject.SetActive(false);
            _unitPreview.SetActive(false);
        }
    }
}