public class UnitSelectionController
{
    private readonly InputManager _inputManager;
    private readonly UnitController _unitController;
    private readonly DataProvider _dataProvider;
    
    public UnitSelectionController(UnitController unitController, DataProvider dataProvider)
    {
        _inputManager = InputManager.Instance;
        _unitController = unitController;
        _dataProvider = dataProvider;
    }
    
    public void Select()
    {
        if (!_inputManager.TryUnitSelect<UnitController>(out var unitController)) return;
        if (_unitController != unitController) return;
        TryDeselect();
        _unitController.Selected = true;
    }

    public void Update()
    {
        OnHover();
        if (_inputManager.GetKeyDown(KeybindingActions.Deselect) || _inputManager.IsNeedDeselect<UnitController>()) TryDeselect();
    }

    private void OnHover()
    {
        var unitController = _inputManager.OnUnitHover<UnitController>();
        _unitController.View.SetOutline(unitController != null && _unitController == unitController);
    }

    public void OnDeselect(object sender, UnitSelectionEvent e)
    {
        if (_unitController != sender || _dataProvider.SelectedUnit != _unitController) return;
        _dataProvider.SelectedUnit = null;
    }

    public void OnSelect(object sender, UnitSelectionEvent e)
    {
        if (_unitController != sender || _dataProvider.SelectedUnit == _unitController) return;
        _dataProvider.SelectedUnit = _unitController;
    }
    
    private void TryDeselect()
    {
        if (!_unitController.Selected && _dataProvider.SelectedUnit == null) return;
        _dataProvider.SelectedUnit.Selected = false;
        _unitController.Selected = false;
    }
}