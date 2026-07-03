using UnityEngine;

public class PogoComboAbility : Ability
{
    private static readonly int PogoUpgradeHash = Animator.StringToHash("PogoUpgrade");
    PocongSideAttack pocongSideAttack;
    PlayerSFX playerSFX;

    public float dmgMultiplier = .15f;
    public float sizeMultiplier = .5f;
    public float normalSize = 1.35f;
    public int maxCombo = 10;
    public int curCombo = 0;

    public GameObject pogoSprite;
    public GameObject pogoHB;

    private SkillBarUI sbUI;

    private void Start()
    {
        pocongSideAttack = GetComponentInChildren<PocongSideAttack>();
        playerSFX = GetComponentInChildren<PlayerSFX>();
        sbUI = GameObject.FindGameObjectWithTag("SkilbarUI").GetComponent<SkillBarUI>(); // Ada typo dari ananas, jgn lupa ganti SkilbarUI jadi SkillBarUI

        normalSize = pogoSprite.transform.localScale.x;

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
        
        curCombo++;
        if (curCombo < maxCombo)
        {
            sbUI.AddProgress(100f / maxCombo);
        }

        if (curCombo == maxCombo)
        {
            context.Attack.IncreaseDamage(dmgMultiplier * context.Attack.attackDamage);
            context.Attack.IncreaseSize(pogoSprite, sizeMultiplier);
            context.Attack.IncreaseSize(pogoHB, sizeMultiplier);
            pocongSideAttack.animator.SetBool(PogoUpgradeHash, true);

            playerSFX.PlayAudio(playerSFX.combo);
        }
    }

    private void ResetCombo()
    {
        pocongSideAttack.animator.SetBool(PogoUpgradeHash, false);
        curCombo = 0;
        sbUI.ResetProgress();
        context.Attack.ResetDamage();
        context.Attack.ResetSize(pogoSprite, normalSize);
        context.Attack.ResetSize(pogoHB, normalSize);
    }
}
