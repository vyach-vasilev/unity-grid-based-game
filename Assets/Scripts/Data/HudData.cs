using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "HudData", menuName = "Data/Hud", order = 0)]
public class HudData: DataStorage
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private Camera _minimapCamera;
    [SerializeField] private Camera _previewCamera;
    [SerializeField] private GameObject _minimapOverlay;
    [SerializeField] private GameObject _unitPreview;
    [SerializeField] private Transform _unitPlacer;
    
    public Camera MinimapCamera => _minimapCamera;
    public Camera PreviewCamera => _previewCamera;
    public GameObject MinimapOverlay => _minimapOverlay;
    public GameObject UnitPreview => _unitPreview;
    public Transform UnitPlacer => _unitPlacer;
    public Canvas Canvas => _canvas;
    public EventSystem EventSystem => _eventSystem;
}