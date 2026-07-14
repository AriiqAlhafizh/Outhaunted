using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ComboAttackSO", menuName = "ScriptableObjects/Ability/Attack/Combo")]
public class ComboAttackSO : AttackSO
{
    [SerializeField] private int attackIndex = 0;

    [SerializeField] private string ComboAnimationName;
    protected int comboAnimationHash;

    public override event Action OnAttack;

    public override void Initialize(GameObject player, InputReader _input)
    {
        base.Initialize(player, _input);
        comboAnimationHash = Animator.StringToHash(ComboAnimationName);
    }
    protected override void StartAttack()
    {
        Debug.Log($"Attacking in direction: {atkDir}");
        StartCooldown();
        OnAttack?.Invoke();

        if (attackIndex == 0)
        {
            TriggerAnimation(animationHash);
            attackIndex = 1;
        }
        else
        {
            TriggerAnimation(comboAnimationHash);
            attackIndex = 0;
        }
    }
}
