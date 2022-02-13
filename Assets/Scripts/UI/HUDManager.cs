using UnityEngine;

public class HUDManager: MonoBehaviour
{
    [SerializeField] private DataProvider dataProvider;
    [SerializeField] private Camera _minimapCamera;
    [SerializeField] private GameObject _minimapOverlay;
    [SerializeField] private GameObject _unitPreview;
    [SerializeField] private Camera _previewCamera;
    
    private void Update()
    {
        MinimapHandler();
        UnitPreviewHandler();
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
    
    private void UnitPreviewHandler()
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