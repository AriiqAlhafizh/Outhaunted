using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ComboAttackSO", menuName = "ScriptableObjects/Ability/Attack/Combo")]
public class ComboAttackSO : AttackSO
{
    [SerializeField] private int attackIndex = 0;

    [Header("Additional Animation Configuration")]
    [SerializeField] private string ComboAnimationName;
    protected int comboAnimationHash;

    public override event Action OnAttack;

    [Header("Additional VFX Configuration")]
    [SerializeField] private Vector3 ComboVFXOffset = new Vector3(0f, 0f, 0f); //nanti diganti
    [SerializeField] private string ComboVFXAnimationName;
    private int _ComboVFXAnimHash;

    [Header ("Additional SFX Configuration")]
    [SerializeField] private AudioClip comboSound;

    public override void Initialize(GameObject player, InputReader _input)
    {
        base.Initialize(player, _input);
        comboAnimationHash = Animator.StringToHash(ComboAnimationName);
        _ComboVFXAnimHash = Animator.StringToHash(ComboVFXAnimationName);
    }
    protected override void StartAttack()
    {
        StartCooldown();
        OnAttack?.Invoke();

        if (attackIndex == 0)
        {
            if (effectsHandler != null)
            {
                effectsHandler.TriggerHitbox(hitboxSize, hitboxOffset, damage, activeDuration, atkDir);
                effectsHandler.PlaySpriteVFX(_vfxAnimHash, vfxOffset);
                effectsHandler.PlaySound(abilitySound);
            }

            TriggerAnimation(animationHash);
            attackIndex = 1;
        }
        else
        {
            if (effectsHandler != null)
            {
                effectsHandler.TriggerHitbox(hitboxSize, hitboxOffset, damage, activeDuration, atkDir);
                effectsHandler.PlaySpriteVFX(_ComboVFXAnimHash, ComboVFXOffset);
                effectsHandler.PlaySound(comboSound);
            }
            TriggerAnimation(comboAnimationHash);
            attackIndex = 0;
        }
    }
}
