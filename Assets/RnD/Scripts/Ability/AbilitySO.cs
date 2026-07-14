using System;
using UnityEngine;
public abstract class AbilitySO : ScriptableObject
{
    public string abilityName;

    public float cooldownDuration;
    private float _cooldownTimer;
    public bool IsOnCooldown => _cooldownTimer > 0f;

    [SerializeField] private string animationStateName;
    protected int animationHash;

    public event Action<int> OnAbilityExecuted;

    public virtual void Initialize(GameObject player, InputReader input)
    {
        animationHash = Animator.StringToHash(animationStateName);
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