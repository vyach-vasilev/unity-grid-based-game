using System.Threading.Tasks;
using UnityEngine;

public class AttackState : State<UnitController, UnitState>
{
    private readonly UnitStateMachine<UnitController, UnitState> _stateMachine;

    public AttackState(UnitState id, UnitStateMachine<UnitController, UnitState> stateMachine) : base(id)
    {
        _stateMachine = stateMachine;
    }

    public override void Enter(UnitController entity)
    {
        //Debug.Log("Enter Attack: " + entity.name);
    }

    public override void Execute(UnitController entity)
    {
        if (!entity.InAttack)
        {
            entity.Animator.SetState(UnitState.Attack);
            entity.Animator.SetFloat(AnimatorIds.StateSpeedId, 0f);

            entity.InAttack = true;

            if (!entity.InMove)
            {
                AttackProcess(entity);
            }
        }
    }

    public override void Exit(UnitController entity)
    {
        //Debug.Log("Exit Attack: " + entity.name);
    }

    private async void AttackProcess(UnitController entity)
    {
        while (entity.InAttack)
        {
            await Task.Yield();
            
            if (!entity)
            {
                Interrupt();
                break;
            }
            
            entity.transform.rotation = InputManager.Instance.RotateOnMouseDirection(entity);
            
            if (Input.GetMouseButtonDown(0))
            {
                Interrupt();
                break;
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                var targetRotation = InputManager.Instance.RotateOnMouseDirection(entity);

                entity.Animator.SetFloat(AnimatorIds.StateSpeedId, 1f);
                var elapsedTime = 0f;
                while (elapsedTime < 2f)
                {
                    if (!entity)
                    {
                        Interrupt();
                        break;
                    }
                    
                    entity.transform.rotation = Quaternion.Lerp(
                        entity.transform.rotation, 
                        targetRotation, elapsedTime);
                    
                    elapsedTime += Time.deltaTime;
                    
                    if (Mathf.Abs(elapsedTime - 2f) <= 0.01f)
                    {
                        //InputHandler.IsSkillProcess = false;
                    }
                    
                    await Task.Yield();
                }
                entity.InAttack = false;
                _stateMachine.ChangeState(UnitState.Idle);
            }
        }
    }

    private void Interrupt()
    {
        _stateMachine.ChangeState(UnitState.Idle);
    }
}