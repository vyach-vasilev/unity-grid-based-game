using UnityEngine;

public class UnitController : MonoBehaviour, IUnitController
{
    private IUnitModel _model;
    private IUnitView _view;
    
    private UnitPathController _pathController;
    private UnitSelectionController _selectionController;
    
    private DataTransmitter _dataTransmitter;
    private FSMController _fsmController;
    
    [SerializeField, Range(1, 20)] private float _movementSpeed = 10;

    public Animator Animator => GetComponentInChildren<Animator>();
    public UnitView View => (UnitView)_view;
    public UnitModel Model => (UnitModel)_model;
    
    public UnitPathController PathController => _pathController;
    public DataTransmitter DataTransmitter => _dataTransmitter;
    public bool InMove => _pathController.IsMoving;
    public bool InAttack { get; set; }
    
    public void Initialize(IUnitModel model, IUnitView view)
    {
        _model = model;
        _view = view;
        
        _pathController = new UnitPathController(this, transform, _movementSpeed);
        _selectionController = new UnitSelectionController(_view);
        _fsmController = new FSMController(this);
        
        _view.Position = _model.Position;
    }
    
    public void OnMoved(Vector3 destination) => _pathController.OnMoved(destination);
    
    public void SetData(DataTransmitter dataTransmitter)
    {
        _dataTransmitter = dataTransmitter;
        CacheData();
        _selectionController.SetTransferData(_dataTransmitter);
    }

    public void Subscribe()
    {
        _view.OnSelect += _selectionController.OnSelect;
        _view.OnDeselect += _selectionController.OnDeselect;
    }

    private void Unsubscribe()
    {
        _view.OnSelect -= _selectionController.OnSelect;
        _view.OnDeselect -= _selectionController.OnDeselect;
    }

    private void CacheData()
    {
        if (!_dataTransmitter.UnitsCollection.ContainsKey(Model.Owner))
        {
            _dataTransmitter.UnitsCollection.Add(Model.Owner, this);
        }
    }
    
    private void OnMouseDown()
    {
        _selectionController.Select();
    }
    
    private void Update()
    {
        _selectionController.Update();
        _fsmController.Update();
    }
    
    private void OnDestroy()
    {
        Unsubscribe();
    }
}