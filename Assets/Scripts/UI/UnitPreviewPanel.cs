using TMPro;
using UnityEngine;

public class UnitPreviewPanel: MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private TextMeshProUGUI _nameField;
    [SerializeField] private TextMeshProUGUI _fractionField;

    public Game Game
    {
        get => _game;
        set => _game = value;
    }

    private void OnEnable()
    {
        var previewModule = (HudModule)_game.ModuleController.TryGetModule(ModuleType.HUD);
        previewModule.OnSelect += UpdateInfo;
    }

    // нужно зафиксить неактивацию обновления
    private void UpdateInfo(object sender, UnitSelectionEvent e)
    {
        var data = (UnitPreview)sender;
        _nameField.text = data.Name;
        _fractionField.text = data.Type.ToString();
    }
    
    private void OnDisable()
    {
        var previewModule = (HudModule)_game.ModuleController.TryGetModule(ModuleType.HUD);
        previewModule.OnSelect -= UpdateInfo;
    }
}