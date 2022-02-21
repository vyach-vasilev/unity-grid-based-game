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
        if (_inputManager.GetKeyDown(KeybindingActions.Skill1))
        {
            owner.AttackVariants = AttackVariants.SmallCross;
            _stateMachine.ChangeState(UnitState.Attack1);
        }
        if(_inputManager.GetKeyDown(KeybindingActions.Skill2))
        {
            owner.AttackVariants = AttackVariants.Octogram;
            _stateMachine.ChangeState(UnitState.Attack2);
        }
        if(_inputManager.GetKeyDown(KeybindingActions.Skill3))
        {
            owner.AttackVariants = AttackVariants.GiantCross;
            _stateMachine.ChangeState(UnitState.Attack3);
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