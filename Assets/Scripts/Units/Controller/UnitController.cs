using UnityEngine;

public class UnitController : MonoBehaviour, IUnitController
{
    private IUnitModel _model;
    private IUnitView _view;
    
    private InputManager _inputManager;
    private UnitPathController _pathController;
    private UnitSelectionController _selectionController;
    private DataTransmitter _dataTransmitter;
    
    [SerializeField, Range(1, 20)] private float _movementSpeed = 10;

    public UnitView View => (UnitView)_view;
    public UnitPathController PathController => _pathController;
    
    public void Initialize(IUnitModel model, IUnitView view)
    {
        _model = model;
        _view = view;

        _inputManager = InputManager.Instance;
        
        _pathController = new UnitPathController(_inputManager, _view, transform);
        _selectionController = new UnitSelectionController(_inputManager, _view);
        
        _view.Position = _model.Position;
    }
    
    public void TransferData(DataTransmitter dataTransmitter)
    {
        _dataTransmitter = dataTransmitter;
        _selectionController.SetTransferData(_dataTransmitter);
    }

    public void Subscribe()
    {
        _inputManager.OnMoved += OnMoved;
        _view.OnSelect += _selectionController.OnSelect;
        _view.OnDeselect += _selectionController.OnDeselect;
    }

    private void Unsubscribe()
    {
        _inputManager.OnMoved -= OnMoved;
        _view.OnSelect -= _selectionController.OnSelect;
        _view.OnDeselect -= _selectionController.OnDeselect;
    }

    private void OnMouseDown()
    {
        _selectionController.Select();
    }
    
    private void Update()
    {
        _selectionController.Update();
        
        if (_view.Selected)
        {
            _inputManager.GetDestinationPosition();
        }
    }
    
    private void OnMoved(Vector3 targetPosition)
    {
        var selectedView = _dataTransmitter.SelectedUnitView;
        _pathController.Update(selectedView, _movementSpeed);
        _pathController.OnMoved(targetPosition);
    }
    
    private void OnDestroy()
    {
        Unsubscribe();
    }
}