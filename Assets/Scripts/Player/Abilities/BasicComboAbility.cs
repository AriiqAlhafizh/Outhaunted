using System.Collections;
using UnityEngine;

public class BasicComboAbility : Ability
{
    public float dmgMultiplier = .15f;
    public int maxCombo = 10;
    public int curCombo = 0;

    private void Start()
    {
        context.Attack.OnAttackHit += Combo;
        PlayerStatsManager.Instance.OnDamaged += ResetCombo;
    }
    private void OnDisable()
    {
        context.Attack.OnAttackHit -= Combo;
        PlayerStatsManager.Instance.OnDamaged -= ResetCombo;
    }
    private void Combo(GameObject enemy)
    {
        if (curCombo < maxCombo)
        {
            curCombo++;
            context.Attack.IncreaseDamage(dmgMultiplier * context.Attack.attackDamage);
        }
    }

    private void ResetCombo()
    {
        curCombo = 0;
        context.Attack.ResetDamage();
    }
}
