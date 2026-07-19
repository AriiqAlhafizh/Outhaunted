using System;
using UnityEngine;
public abstract class AbilitySO : ScriptableObject
{
    public string abilityName;

    [Header("Cooldown")]
    public float cooldownDuration;
    private float _cooldownTimer;
    public bool IsOnCooldown => _cooldownTimer > 0f;

    [Header("Player Animation")]
    [SerializeField] private string animationStateName;
    protected int animationHash;

    [Header("SFX")]
    [SerializeField] protected AudioClip abilitySound;

    public event Action<int> OnAbilityExecuted;

    protected EffectsHandler effectsHandler;

    public virtual void Initialize(GameObject player, InputReader input)
    {
        animationHash = Animator.StringToHash(animationStateName);
        effectsHandler = player.GetComponent<EffectsHandler>();
    }

    public abstract void OnDestroyAbility();
    public void Tick(float deltaTime)
    {
        if (_cooldownTimer > 0f)
        {
            _cooldownTimer -= deltaTime;
        }
    }

    protected void StartCooldown()
    {
        _cooldownTimer = cooldownDuration;
    }

    public float GetCooldownPercentage()
    {
        if (cooldownDuration <= 0) return 0;
        return Mathf.Clamp01(_cooldownTimer / cooldownDuration);
    }
    protected void TriggerAnimation(int _animationHash)
    {
        OnAbilityExecuted?.Invoke(_animationHash);
    }
}