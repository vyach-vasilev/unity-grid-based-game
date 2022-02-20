using System;
using UnityEngine;

public class UnitController : MonoBehaviour, IUnitController
{
    private IUnitModel _model;
    private IUnitView _view;
    private bool _selected;

    public event EventHandler<UnitSelectionEvent> OnSelect = (_, _) => {};
    public event EventHandler<UnitSelectionEvent> OnDeselect = (_, _) => {};
    
    public bool Selected
    {
        get => _selected;
        set
        {
            _selected = value;
            var evt = new UnitSelectionEvent();
            if (_selected) OnSelect.Invoke(this, evt);
            else OnDeselect.Invoke(this, evt);
        }
    }
    
    public Animator Animator => GetComponentInChildren<Animator>();
    public UnitView View => (UnitView)_view;
    public UnitModel Model => (UnitModel)_model;
    public UnitPathController UnitPathController { get; set; }
    public UnitSelectionController UnitSelectionController { get; set; }
    public FSMController FSMController { get; set; }
    public DataProvider DataProvider { get; set; }
    
    public bool InMove => UnitPathController.IsMoving;
    public bool InAttack { get; set; }
    public AttackVariants AttackVariants { get; set; }
    
    public void Initialize(IUnitModel model, IUnitView view)
    {
        _model = model;
        _view = view;
    }
    
    public void OnMoved(Vector3 destination) => UnitPathController.OnMoved(destination);
    
    public void Subscribe()
    {
        OnSelect += UnitSelectionController.OnSelect;
        OnDeselect += UnitSelectionController.OnDeselect;
    }

    private void Unsubscribe()
    {
        OnSelect -= UnitSelectionController.OnSelect;
        OnDeselect -= UnitSelectionController.OnDeselect;
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