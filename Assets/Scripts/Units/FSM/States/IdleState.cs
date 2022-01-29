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
        //Debug.Log("Enter Idle: " + entity.name);
    }

    public override void Execute(UnitController entity)
    {
        if(entity.View.Selected)
        {
            if(!entity.CanAttack)
            {
                //TODO: режими переключения сетки (moving / attack)
                var position = InputManager.Instance.GetWorldNodePosition();
                entity.OnMoved(position);
            }
            
            if (entity.CanMove)
            {
                _stateMachine.ChangeState(UnitState.Moving);
            }
            else if (entity.CanAttack)
            {
                _stateMachine.ChangeState(UnitState.Attack);
            }
        }
    }

    public override void Exit(UnitController entity)
    {
        //Debug.Log("Exit Idle: " + entity.name);
    }
}