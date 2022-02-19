public class UnitSelectionController
{
    private readonly InputManager _inputManager;
    private readonly IUnitView _view;
    private readonly DataProvider _dataProvider;
    
    private UnitView View => (UnitView)_view;

    public UnitSelectionController(IUnitView view, DataProvider dataProvider)
    {
        _inputManager = InputManager.Instance;
        _view = view;
        _dataProvider = dataProvider;
    }
    
    public void Select()
    {
        if (!_inputManager.TrySelectUnit<IUnitView>(out var unitView)) return;
        if (_view != unitView) return;
        TryDeselect();
        _view.Selected = true;
    }

    public void Update()
    {
        OnHover();
        if (_inputManager.Deselect || _inputManager.IsNeedDeselect<IUnitView>()) TryDeselect();
    }

    private void OnHover()
    {
        var view = _inputManager.OnUnitHover<IUnitView>();
        View.SetOutline(view != null && _view == view);
    }

    public void OnDeselect(object sender, UnitSelectionEvent e)
    {
        if (_view != sender || _dataProvider.SelectedUnitView != _view) return;
        _dataProvider.SelectedUnitView = null;
    }

    public void OnSelect(object sender, UnitSelectionEvent e)
    {
        if (_view != sender || _dataProvider.SelectedUnitView == _view) return;
        _dataProvider.SelectedUnitView = _view;
    }
    
    private void TryDeselect()
    {
        if (!_view.Selected && _dataProvider.SelectedUnitView == null) return;
        _dataProvider.SelectedUnitView.Selected = false;
        _view.Selected = false;
    }
}