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
            Debug.Log("Attack 1");
            if (effectsHandler != null)
            {
                effectsHandler.TriggerHitbox(hitboxSize, hitboxOffset, damage, activeDuration, atkDir);
                effectsHandler.PlaySpriteVFX(_vfxAnimHash, vfxOffset);
            }

            TriggerAnimation(animationHash);
            attackIndex = 1;
        }
        else
        {
            Debug.Log("Attack 2");

            if (effectsHandler != null)
            {
                effectsHandler.TriggerHitbox(hitboxSize, hitboxOffset, damage, activeDuration, atkDir);
                effectsHandler.PlaySpriteVFX(_ComboVFXAnimHash, ComboVFXOffset);
            }
            TriggerAnimation(comboAnimationHash);
            attackIndex = 0;
        }
    }
}
