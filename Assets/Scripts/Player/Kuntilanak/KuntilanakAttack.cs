using System;
using UnityEngine;

public class KuntilanakAttack : PlayerAttack
{
    public readonly int PlayerAttack1 = Animator.StringToHash("Attack_1");
    public readonly int PlayerAttack2 = Animator.StringToHash("Attack_2");

    [SerializeField] private int attackIndex = 0;

    public override event Action OnAttack;

    protected override void StartAttack()
    {
        StartCoroutine(AttackCooldown());
        OnAttack?.Invoke();
        pAnimation.StartAttack();

        if (attackIndex == 0)
        {
            pAnimation.ChangeAnimationState(PlayerAttack1);
            attackIndex = 1;
        }
        else
        {
            pAnimation.ChangeAnimationState(PlayerAttack2);
            attackIndex = 0;
        }
    }
}
