using System.Collections;
using UnityEngine;

public class BasicComboAbility : Ability
{
    public float dmgMultiplier = .15f;
    public float sizeMultiplier = .15f;
    public int maxCombo = 10;
    public int curCombo = 0;

    private Animator animator;
    public AnimatorOverrideController redMode;
    private RuntimeAnimatorController normalMode;

    GameObject spriteObj;
    GameObject sideHB;
    private SkillBarUI sbUI;

    private void Start()
    {
        spriteObj = GetComponentInChildren<SideAttack>().gameObject;
        sideHB = GetComponentInChildren<PlayerAttackHitbox>().gameObject;

        animator = GetComponentInChildren<SideAttack>().gameObject.GetComponent<Animator>();
        normalMode = animator.runtimeAnimatorController;

        sbUI = GameObject.FindGameObjectWithTag("SkilbarUI").GetComponent<SkillBarUI>(); // Ada typo dari ananas, jgn lupa ganti SkilbarUI jadi SkillBarUI


        context.Attack.OnAttackHit += Combo;
        PlayerManager.Instance.OnDamaged += ResetCombo;
    }
    private void OnDisable()
    {
        context.Attack.OnAttackHit -= Combo;
        PlayerManager.Instance.OnDamaged -= ResetCombo;
    }
    private void Combo(GameObject enemy)
    {
        curCombo++;
        if (curCombo < maxCombo)
        {
            sbUI.AddProgress(100f / maxCombo);
        }

        if (curCombo == maxCombo)
        {
            context.Attack.IncreaseDamage(dmgMultiplier * context.Attack.attackDamage);

            animator.runtimeAnimatorController = redMode;
            
            context.Attack.IncreaseSize(spriteObj, sizeMultiplier);
            context.Attack.IncreaseSize(sideHB, sizeMultiplier);
            
            context.Attack.pAnimation.OverrideAnimation();
        }
    }
    private void ResetCombo()
    {
        curCombo = 0;
        context.Attack.ResetDamage();

        sbUI.ResetProgress();

        context.Attack.pAnimation.ResetAnimation();
        animator.runtimeAnimatorController = normalMode;
        
        context.Attack.ResetSize(spriteObj, sizeMultiplier);
        context.Attack.ResetSize(sideHB, sizeMultiplier);
    }
}
