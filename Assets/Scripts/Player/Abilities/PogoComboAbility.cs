using UnityEngine;

public class PogoComboAbility : Ability
{
    private static readonly int PogoUpgradeHash = Animator.StringToHash("PogoUpgrade");
    public PocongSideAttack pocongSideAttack;

    public float dmgMultiplier = .15f;
    public int maxCombo = 10;
    public int curCombo = 0;

    private void Start()
    {
        pocongSideAttack = GetComponentInChildren<PocongSideAttack>();

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
        }

        if (curCombo == maxCombo)
        {
            context.Attack.IncreaseDamage(dmgMultiplier * context.Attack.attackDamage);
            pocongSideAttack.animator.SetBool(PogoUpgradeHash, true);
        }
    }

    private void ResetCombo()
    {
        pocongSideAttack.animator.SetBool(PogoUpgradeHash, false);
        curCombo = 0;
        context.Attack.ResetDamage();
    }
}
