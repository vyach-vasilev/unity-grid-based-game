using UnityEngine;

public class UnitController : MonoBehaviour, IUnitController
{
    private IUnitModel _model;
    private IUnitView _view;
    
    private InputManager _inputManager;
    private UnitPathController _pathController;
    private UnitSelectionController _selectionController;
    private DataTransmitter _dataTransmitter;
    private FSMController _fsmController;
    private VFXMarker _vfxMarker;
    
    [SerializeField, Range(1, 20)] private float _movementSpeed = 10;

    public Animator Animator => GetComponentInChildren<Animator>();
    public UnitView View => (UnitView)_view;
    public UnitPathController PathController => _pathController;

    public bool CanMove => Input.GetMouseButtonDown(1);

    public bool InMove => _pathController.IsMoving;
    public bool CanAttack => Input.GetKeyDown(KeyCode.Space);
    public bool InAttack { get; set; }
    
    public void Initialize(IUnitModel model, IUnitView view)
    {
        _model = model;
        _view = view;

        _inputManager = InputManager.Instance;
        
        _pathController = new UnitPathController(this, _inputManager, transform);
        _selectionController = new UnitSelectionController(_inputManager, _view);
        _fsmController = new FSMController(this);
        
        _view.Position = _model.Position;
        _vfxMarker = GetComponentInChildren<VFXMarker>();
    }
    
    public void TransferData(DataTransmitter dataTransmitter)
    {
        _dataTransmitter = dataTransmitter;
        _selectionController.SetTransferData(_dataTransmitter);
    }

    public void OnMoved(Vector3 destination)
    {
        var selectedView = (UnitView)_dataTransmitter.SelectedUnitView;
        _pathController.Update(selectedView, _movementSpeed);
        _pathController.OnMoved(destination);
    }
    
    public void ActivateVFX()
    {
        _vfxMarker.Slash.Play();
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