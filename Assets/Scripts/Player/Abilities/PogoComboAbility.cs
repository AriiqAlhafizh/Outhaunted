using UnityEngine;

public class PogoComboAbility : Ability
{
    public float dmgMultiplier = .15f;
    public int maxCombo = 10;
    public int curCombo = 0;

    private void Start()
    {
        context.Attack.OnPogo += Combo;
        context.Movement.OnLand += ResetCombo;
    }
    private void OnDisable()
    {
        context.Attack.OnPogo -= Combo;
        context.Movement.OnLand -= ResetCombo;
    }
    private void Combo()
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
