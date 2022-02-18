using Unity.VisualScripting;
using UnityEngine;

public class UnitController : MonoBehaviour, IUnitController
{
    private IUnitModel _model;
    private IUnitView _view;
    
    private UnitPathController _pathController;
    private UnitSelectionController _selectionController;
    private UnitFootstepController _footstepController;
    
    private DataProvider _dataProvider;
    private FSMController _fsmController;
    
    [SerializeField, Range(1, 20)] private float _movementSpeed = 10;

    public Animator Animator => GetComponentInChildren<Animator>();
    public UnitView View => (UnitView)_view;
    public UnitModel Model => (UnitModel)_model;
    
    public UnitPathController PathController => _pathController;
    public DataProvider DataProvider => _dataProvider;
    public bool InMove => _pathController.IsMoving;
    public bool InAttack { get; set; }
    
    public void Initialize(IUnitModel model, IUnitView view, DataProvider dataProvider)
    {
        _model = model;
        _view = view;
        
        _dataProvider = dataProvider;
        
        if (!_dataProvider.Units.Contains(this))
            _dataProvider.Units.Add(this);
        
        _pathController = new UnitPathController(this, transform, _movementSpeed);
        _selectionController = new UnitSelectionController(_view, _dataProvider);
        _fsmController = new FSMController(this);
    }
    
    public void OnMoved(Vector3 destination) => _pathController.OnMoved(destination);
    
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