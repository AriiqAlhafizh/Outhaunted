using UnityEngine;

public class PogoComboAbility : Ability
{
    private static readonly int PogoUpgradeHash = Animator.StringToHash("PogoUpgrade");
    public PocongSideAttack pocongSideAttack;

    public float dmgMultiplier = .15f;
    public float sizeMultiplier = .5f;
    public int maxCombo = 10;
    public int curCombo = 0;

    public GameObject pogoHB;

    private SkillBarUI sbUI;

    private void Start()
    {
        pocongSideAttack = GetComponentInChildren<PocongSideAttack>();
        sbUI = GameObject.FindGameObjectWithTag("SkilbarUI").GetComponent<SkillBarUI>(); // Ada typo dari ananas, jgn lupa ganti SkilbarUI jadi SkillBarUI

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
        Debug.Log("Pogo Combo Called");
        if (curCombo < maxCombo)
        {
            curCombo++;
            sbUI.AddProgress(100f / maxCombo);
        }

        if (curCombo == maxCombo)
        {
            context.Attack.IncreaseDamage(dmgMultiplier * context.Attack.attackDamage);
            context.Attack.IncreaseSize(pogoHB, sizeMultiplier);
            pocongSideAttack.animator.SetBool(PogoUpgradeHash, true);
        }
    }

    private void ResetCombo()
    {
        pocongSideAttack.animator.SetBool(PogoUpgradeHash, false);
        curCombo = 0;
        sbUI.ResetProgress();
        context.Attack.ResetDamage();
        context.Attack.ResetSize(pogoHB, sizeMultiplier);
    }
}
