using UnityEngine;

public class UnitSelectionController
{
    private readonly InputManager _inputManager;
    private readonly IUnitView _view;
    
    private DataTransmitter _data;
    private UnitView View => (UnitView)_view;

    public UnitSelectionController(InputManager inputManager, IUnitView view)
    {
        _inputManager = inputManager;
        _view = view;
    }

    public void SetTransferData(DataTransmitter data)
    {
        _data = data;
    }
    
    public void Select()
    {
        if (_inputManager.TryGetUnit(out var unitView))
        {
            if (_view != unitView)
            {
                return;
            }
            
            TryDeselect();
            
            _view.Selected = true;
            _inputManager.IsMoveLocked = true;
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || _inputManager.IsNeedDeselect())
        {
            TryDeselect();
        }
    }
    
    public void OnDeselect(object sender, UnitSelectEvent e)
    {
        if (_view != sender || _data.SelectedUnitView != _view)
        {
            return;
        }
        View.SetEmission(false);
        _data.SelectedUnitView = null;
    }

    public void OnSelect(object sender, UnitSelectEvent e)
    {
        if (_view != sender || _data.SelectedUnitView == _view)
        {
            return;
        }
        View.SetEmission(true);
        _data.SelectedUnitView = _view;
    }
    
    private void TryDeselect()
    {
        if (_view.Selected || _data.SelectedUnitView != null)
        {
            _data.SelectedUnitView.Selected = false;
            _view.Selected = false;
            _inputManager.IsMoveLocked = false;
        }
    }
}