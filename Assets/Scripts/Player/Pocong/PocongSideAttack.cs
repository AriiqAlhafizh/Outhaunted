using UnityEngine;

public class PocongSideAttack : SideAttack
{
    private static readonly int PogoHash = Animator.StringToHash("Pogo");
    private static readonly int Attack1Hash = Animator.StringToHash("Attack1");

    public override void TriggerAttack()
    {
        if (direction == AttackDirection.Left || direction == AttackDirection.Right)
            animator.SetTrigger(Attack1Hash);
        if (direction == AttackDirection.Down)
            animator.SetTrigger(PogoHash);
    }
}
