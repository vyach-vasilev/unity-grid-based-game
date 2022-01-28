public static class AnimationExtensions
{
    public static void SetState(this UnityEngine.Animator animator, UnitState state)
    {
        animator.SetTrigger(state.ToString());
    }
    
    public static void ResetState(this UnityEngine.Animator animator, UnitState state)
    {
        animator.ResetTrigger(state.ToString());
    }
}