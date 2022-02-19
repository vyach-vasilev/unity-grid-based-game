using UnityEngine;

public class UnitController : MonoBehaviour, IUnitController
{
    private IUnitModel _model;
    private IUnitView _view;
    
    
    public Animator Animator => GetComponentInChildren<Animator>();
    public UnitView View => (UnitView)_view;
    public UnitModel Model => (UnitModel)_model;
    
    public UnitPathController UnitPathController { get; set; }
    public UnitSelectionController UnitSelectionController { get; set; }
    public FSMController FSMController { get; set; }
    public DataProvider DataProvider { get; set; }
    
    public bool InMove => UnitPathController.IsMoving;
    public bool InAttack { get; set; }
    
    public void Initialize(IUnitModel model, IUnitView view)
    {
        _model = model;
        _view = view;
    }
    
    public void OnMoved(Vector3 destination) => UnitPathController.OnMoved(destination);
    
    public void Subscribe()
    {
        _view.OnSelect += UnitSelectionController.OnSelect;
        _view.OnDeselect += UnitSelectionController.OnDeselect;
    }

    private void Unsubscribe()
    {
        _view.OnSelect -= UnitSelectionController.OnSelect;
        _view.OnDeselect -= UnitSelectionController.OnDeselect;
    }
    
    private void OnMouseDown()
    {
        UnitSelectionController.Select();
    }
    
    private void Update()
    {
        UnitSelectionController.Update();
        FSMController.Update();
    }
    
    private void OnDestroy()
    {
        Unsubscribe();
    }
}