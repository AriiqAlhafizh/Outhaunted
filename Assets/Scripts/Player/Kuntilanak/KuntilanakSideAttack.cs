using System.Collections;
using UnityEngine;

public class KuntilanakSideAttack : SideAttack
{
    private static readonly int Attack2Hash = Animator.StringToHash("Attack2");
    private static readonly int Attack1Hash = Animator.StringToHash("Attack1");

    private int attackIndex = 0;
    public override void TriggerAttack()
    {
        if (attackIndex == 0)
        {
            animator.SetTrigger(Attack1Hash);
            attackIndex = 1;
            StartCoroutine(ToggleCollider(sideAttackCol, context.Attack.attackCooldown));
        }
        else
        {
            animator.SetTrigger(Attack2Hash);
            attackIndex = 0;
            StartCoroutine(ToggleCollider(sideAttackCol, context.Attack.attackCooldown));
        }
    }
}