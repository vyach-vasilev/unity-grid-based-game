using UnityEngine;

public class HUDManager: MonoBehaviour
{
    [SerializeField] private Camera _minimapCamera;
    [SerializeField] private GameObject _minimapOverlay;
    
    private void Update()
    {
        MinimapHandler();
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
}