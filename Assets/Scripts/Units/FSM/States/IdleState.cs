using UnityEngine;

public class IdleState: State<UnitController, UnitState>
{
    private readonly UnitStateMachine<UnitController, UnitState> _stateMachine;
    
    public IdleState(UnitState id, UnitStateMachine<UnitController, UnitState> stateMachine) : base(id)
    {
        _stateMachine = stateMachine;
    }
    
    public override void Enter(UnitController entity)
    {
        Debug.Log("Enter Idle: " + entity.name);
    }

    public override void Execute(UnitController entity)
    {
        if (entity.View.Selected && entity.InMove)
        {
            _stateMachine.ChangeState(UnitState.Moving);
        }
        else if (entity.View.Selected && entity.CanAttack)
        {
            _stateMachine.ChangeState(UnitState.Attack);
        }
    }

    public override void Exit(UnitController entity)
    {
        Debug.Log("Exit Idle: " + entity.name);
    }
}