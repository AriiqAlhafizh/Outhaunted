using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ComboAttackSO", menuName = "ScriptableObjects/Ability/Attack/Combo")]
public class ComboAttackSO : AttackSO
{
    [SerializeField] private int attackIndex = 0;

    public override event Action OnAttack;
    protected override void StartAttack()
    {
        Debug.Log($"Attacking in direction: {atkDir}");
        StartCooldown();
        OnAttack?.Invoke();

        if (attackIndex == 0)
        {
            Debug.Log("Attack 1");
            attackIndex = 1;
        }
        else
        {
            Debug.Log("Attack 2");
            attackIndex = 0;
        }
    }
}
