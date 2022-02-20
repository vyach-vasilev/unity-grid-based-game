using System.Collections.Generic;

public class FSMController
{
    private readonly UnitStateMachine<UnitController, UnitState> _stateMachine;

    public FSMController(UnitController controller)
    {
        _stateMachine = new UnitStateMachine<UnitController, UnitState>(controller);

        var idleState = new IdleState(UnitState.Idle, _stateMachine);
        var movingState = new MovingState(UnitState.Moving, _stateMachine);
        var attackFirstState = new AttackFirstState(UnitState.Attack1, _stateMachine);
        var attackSecondState = new AttackSecondState(UnitState.Attack2, _stateMachine);
        var attackThirdState = new AttackThirdState(UnitState.Attack3, _stateMachine);
        
        _stateMachine.Subscribe(new List<State<UnitController, UnitState>>
        {
            idleState,
            movingState,
            attackFirstState,
            attackSecondState,
            attackThirdState,
        });
        
        _stateMachine.ChangeState(idleState.Id);
    }
    
    public void Update()
    {
        _stateMachine.Update();
    }
}