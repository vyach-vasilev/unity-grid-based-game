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

        owner.OnMoved(_inputManager.GetWorldNodePosition());
        
        if (_inputManager.GetKeyDown(KeybindingActions.Action))
        {
            if(AvailableMove(owner))
                _stateMachine.ChangeState(UnitState.Moving);
        }
        if (_inputManager.GetKeyDown(KeybindingActions.Attack))
        {
            owner.AttackVariants = AttackVariants.SmallCross;
            _stateMachine.ChangeState(UnitState.Attack);
        }
        if(_inputManager.GetKeyDown(KeybindingActions.HeavyAttack))
        {
            owner.AttackVariants = AttackVariants.Octogram;
            _stateMachine.ChangeState(UnitState.HeavyAttack);
        }
        if(_inputManager.GetKeyDown(KeybindingActions.MagicAttack))
        {
            owner.AttackVariants = AttackVariants.GiantCross;
            _stateMachine.ChangeState(UnitState.MagicAttack);
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