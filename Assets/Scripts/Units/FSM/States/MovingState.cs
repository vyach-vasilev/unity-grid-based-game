using UnityEngine;

public class MovingState: State<UnitController, UnitState>
{
    private readonly UnitStateMachine<UnitController, UnitState> _stateMachine;

    public MovingState(UnitState id, UnitStateMachine<UnitController, UnitState> stateMachine) : base(id)
    {
        _stateMachine = stateMachine;
    }
    public override void Enter(UnitController entity)
    {
        //Debug.Log("Enter Moving: " + entity.name);
    }

    public override void Execute(UnitController entity)
    {
        var path = entity.PathController.Path;
        
        if (path == null)
        {
            return;
        }

        if (path.Count<= 0)
        {
            return;
        }
        
        entity.Animator.SetBool(AnimatorIds.MovingId, true);

        if (entity.View.Position.XZ() == path[^1].XZ())
        {
            _stateMachine.ChangeState(UnitState.Idle);
        }
    }

    public override void Exit(UnitController entity)
    {
        //Debug.Log("Exit Moving: " + entity.name);
        entity.Animator.SetBool(AnimatorIds.MovingId, false);
    }
}