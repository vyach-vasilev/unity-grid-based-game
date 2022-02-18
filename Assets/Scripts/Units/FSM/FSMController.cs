using System.Collections.Generic;

public class FSMController
{
    private readonly UnitStateMachine<UnitController, UnitState> _stateMachine;

    public FSMController(UnitController controller)
    {
        _stateMachine = new UnitStateMachine<UnitController, UnitState>(controller);

        var idleState = new IdleState(UnitState.Idle, _stateMachine);
        var movingState = new MovingState(UnitState.Moving, _stateMachine);
        var attackState = new AttackState(UnitState.Attack, _stateMachine);
        
        _stateMachine.Subscribe(new List<State<UnitController, UnitState>>
        {
            idleState,
            movingState,
            attackState
        });
        
        _stateMachine.ChangeState(idleState.Id);
    }
    
    public void Update()
    {
        _stateMachine.Update();
    }
}