using UnityEngine;

public class IdleState: State<UnitController, UnitState>
{
    private readonly UnitStateMachine<UnitController, UnitState> _stateMachine;
    private InputManager _inputManager;
    
    public IdleState(UnitState id, UnitStateMachine<UnitController, UnitState> stateMachine) : base(id)
    {
        _stateMachine = stateMachine;
    }
    
    public override void Enter(UnitController entity)
    {
        _inputManager = InputManager.Instance;
    }

    public override void Execute(UnitController entity)
    {
        if (entity.View != (UnitView)entity.DataProxy.SelectedUnitView) return;

        if (!entity.View.Selected) return;

        if(!_inputManager.CanAttack)
        {
            //TODO: режими переключения сетки (moving / attack)
            var position = _inputManager.GetWorldNodePosition();
            entity.OnMoved(position);
        }
        
        if (AvailableMove())
        {
            _stateMachine.ChangeState(UnitState.Moving);
        }
        else if (_inputManager.CanAttack)
        {
            _stateMachine.ChangeState(UnitState.Attack);
        }
    }

    public override void Exit(UnitController entity)
    {
    }

    private bool AvailableMove()
    {
        return
            _inputManager.CanMove &&
            _inputManager.GetWorldNodePosition() != Vector3.down && 
            _inputManager.IsWalkableNode();
    }
}