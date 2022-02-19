using System.Threading.Tasks;
using UnityEngine;

public class AttackState : State<UnitController, UnitState>
{
    private readonly UnitStateMachine<UnitController, UnitState> _stateMachine;

    public AttackState(UnitState id, UnitStateMachine<UnitController, UnitState> stateMachine) : base(id)
    {
        _stateMachine = stateMachine;
    }

    public override void Enter(UnitController owner)
    {
    }

    public override void Execute(UnitController owner)
    {
        if (owner.InAttack) return;
        
        owner.Animator.SetState(UnitState.Attack);
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
            if (!owner.View.Selected || InputManager.Instance.Select)
            {
                Interrupt(owner);
                break;
            }
            
            owner.transform.rotation = RotateOnMouseDirection(owner);
            if (InputManager.Instance.MoveAction)
            {
                await Attack(owner);
            }
        }
    }

    private async Task Attack(UnitController owner)
    {
        var targetRotation = RotateOnMouseDirection(owner);
        owner.Animator.SetFloat(AnimatorIds.StateSpeedId, 1f);
        var elapsedTime = 0f;
        while (elapsedTime < 2f)
        {
            if (!owner) break;
            owner.transform.rotation = Quaternion.Lerp(owner.transform.rotation, targetRotation, elapsedTime);
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
    
    private Quaternion RotateOnMouseDirection(UnitController owner)
    {
        var mousePosition = Camera.main.WorldToScreenPoint(owner.transform.position);
        mousePosition = Input.mousePosition - mousePosition;
        var angle = Mathf.Atan2(mousePosition.y, -mousePosition.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle - 120, Vector3.up);
    }
}