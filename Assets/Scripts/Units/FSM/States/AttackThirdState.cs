using System.Threading.Tasks;
using UnityEngine;

public class AttackThirdState : State<UnitController, UnitState>
{
    private static readonly int Attack3 = Animator.StringToHash("Attack");
    private readonly UnitStateMachine<UnitController, UnitState> _stateMachine;

    public AttackThirdState(UnitState id, UnitStateMachine<UnitController, UnitState> stateMachine) : base(id)
    {
        _stateMachine = stateMachine;
    }

    public override void Enter(UnitController owner)
    {
    }

    public override void Execute(UnitController owner)
    {
        if (owner.InAttack) return;
        
        owner.Animator.SetTrigger(Attack3);
        owner.Animator.SetFloat(AnimatorIds.StateSpeedId, 0f);

        owner.InAttack = true;

        if (!owner.InMove)
        {
            AttackProcess(owner);
        }
    }

    public override void Exit(UnitController owner)
    {
    }

    private async void AttackProcess(UnitController owner)
    {
        while (owner.InAttack)
        {
            await Task.Yield();
            
            if (!owner) break;
            if (!owner.Selected || InputManager.Instance.Select)
            {
                Interrupt(owner);
                break;
            }
            
            if (InputManager.Instance.MoveAction)
            {
                await Attack(owner);
            }
        }
    }

    private async Task Attack(UnitController owner)
    {
        owner.Animator.SetFloat(AnimatorIds.StateSpeedId, 1f);
        var elapsedTime = 0f;
        while (elapsedTime < 2f)
        {
            if (!owner) break;
            elapsedTime += Time.deltaTime;
            await Task.Yield();
        }
        owner.InAttack = false;
        _stateMachine.ChangeState(UnitState.Idle);
    }
    
    private void Interrupt(UnitController owner)
    {
        owner.InAttack = false;
        owner.Animator.Rebind();
        _stateMachine.ChangeState(UnitState.Idle);
    }
}