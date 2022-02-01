using System.Collections.Generic;
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
        if (!IsPathValid(entity, out var path))
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

    private bool IsPathValid(UnitController entity, out List<Vector3> path)
    {
        path = entity.PathController.Path;
        return path is { Count: > 0 };
    }
}