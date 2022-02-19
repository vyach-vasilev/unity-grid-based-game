public class IdleState: State<UnitController, UnitState>
{
    private readonly UnitStateMachine<UnitController, UnitState> _stateMachine;
    private InputManager _inputManager;
    
    public IdleState(UnitState id, UnitStateMachine<UnitController, UnitState> stateMachine) : base(id)
    {
        _stateMachine = stateMachine;
    }
    
    public override void Enter(UnitController owner)
    {
        _inputManager = InputManager.Instance;
    }

    public override void Execute(UnitController owner)
    {
        if (owner != owner.DataProvider.SelectedUnit) return;
        if (!owner.Selected) return;

        if(!_inputManager.PrepareToAttack)
        {
            var position = _inputManager.GetWorldNodePosition();
            owner.OnMoved(position);
        }
        
        if (_inputManager.MoveAction && AvailableMove(owner))
        {
            _stateMachine.ChangeState(UnitState.Moving);
        }
        else if (_inputManager.PrepareToAttack)
        {
            _stateMachine.ChangeState(UnitState.Attack);
        }
    }

    public override void Exit(UnitController owner)
    {
    }

    private bool AvailableMove(UnitController owner)
    {
        var nodePosition = _inputManager.GetWorldNodePosition();
        return
            nodePosition != owner.View.Position && 
            _inputManager.IsWalkableNode();
    }
}