using TMPro;
using UnityEngine;

public class UnitPreviewPanel: MonoBehaviour
{
    [SerializeField] private GuiPreviewManager _guiPreviewManager;
    [SerializeField] private TextMeshProUGUI _nameField;
    [SerializeField] private TextMeshProUGUI _fractionField;

    private void OnEnable()
    {
        _guiPreviewManager.OnSelect += UpdateInfo;
    }

    private void UpdateInfo(object sender, UnitSelectionEvent e)
    {
        var data = (UnitPreview)sender;
        _nameField.text = data.Name;
        _fractionField.text = data.Type.ToString();
    }
    
    private void OnDisable()
    {
        _guiPreviewManager.OnSelect -= UpdateInfo;
    }
}