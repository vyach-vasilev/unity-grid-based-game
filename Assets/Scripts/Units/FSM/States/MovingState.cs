using System.Collections.Generic;
using UnityEngine;

public class MovingState: State<UnitController, UnitState>
{
    private readonly UnitStateMachine<UnitController, UnitState> _stateMachine;

    public MovingState(UnitState id, UnitStateMachine<UnitController, UnitState> stateMachine) : base(id)
    {
        _stateMachine = stateMachine;
    }
    public override void Enter(UnitController owner)
    {
    }

    public override void Execute(UnitController owner)
    {
        if (!IsPathValid(owner, out var path))
        {
            _stateMachine.ChangeState(UnitState.Idle);
            return;
        }
        
        owner.Animator.SetBool(AnimatorIds.MovingId, true);

        if (owner.View.Position.XZ() == path[^1].XZ())
        {
            _stateMachine.ChangeState(UnitState.Idle);
        }
    }

    public override void Exit(UnitController owner)
    {
        owner.Animator.SetBool(AnimatorIds.MovingId, false);
    }

    private bool IsPathValid(UnitController owner, out List<Vector3> path)
    {
        path = owner.PathController.Path;
        return path is { Count: > 0 };
    }
}