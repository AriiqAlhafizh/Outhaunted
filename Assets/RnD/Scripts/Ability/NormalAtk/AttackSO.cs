using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackSO", menuName = "ScriptableObjects/Ability/Normal")]
public class AttackSO : AbilitySO
{
    [Header("References")]
    [SerializeField] private CharacterData characterData;
    private InputReader input;
    private EnvironmentSensor2D envSensor;

    [Header("Settings")]
    private bool canAttack = true;

    [Header("Hitbox Configuration")]
    [SerializeField] protected Vector2 hitboxSize = new Vector2(2f, 1.5f); //nanti diganti
    [SerializeField] protected Vector2 hitboxOffset = new Vector2(1f, 0f); //nanti diganti
    protected float damage;
    [SerializeField] protected float activeDuration = 0.15f; // Berapa lama hitbox aktif

    [Header("VFX Configuration")]
    [SerializeField] protected Vector3 vfxOffset = new Vector3(1f, 0f, 0f); //nanti diganti
    [SerializeField] protected string vfxAnimationName;
    protected int _vfxAnimHash;

    [Header("Debug")]
    [SerializeField] protected AttackDirection atkDir = AttackDirection.Right;
    [SerializeField] protected AttackDirection lastXDir = AttackDirection.Right;
    [SerializeField] protected float lastAttackTime = -Mathf.Infinity;
    [SerializeField] protected float lastPogoAttackTime = -Mathf.Infinity;

    public event Action<AttackDirection> AttackDirectionChanged;
    public virtual event Action OnAttack;
    public event Action<GameObject> OnAttackHit;

    public override void Initialize(GameObject player, InputReader _input)
    {
        base.Initialize(player, _input);
        envSensor = player.GetComponent<EnvironmentSensor2D>();

        _vfxAnimHash = Animator.StringToHash(vfxAnimationName);

        input = _input;

        cooldownDuration = characterData.attackCooldown;
        damage = characterData.attackDamage;

        input.AttackPressed += Attack;
        input.MovementChanged += GetAttackDirection;
    }

    public override void OnDestroyAbility()
    {
        input.AttackPressed -= Attack;
        input.MovementChanged -= GetAttackDirection;
    }

    public void Attack()
    {
        float currentCooldown = cooldownDuration;
        float lastTime = lastAttackTime;

        if (Time.time >= lastTime + currentCooldown
            && canAttack)
        {
            lastAttackTime = Time.time;
            StartAttack();
        }
    }
    protected virtual void StartAttack()
    {
        Debug.Log("Attack");
        StartCooldown();

        if (effectsHandler != null) {
            effectsHandler.TriggerHitbox(hitboxSize, hitboxOffset, damage, activeDuration, atkDir);
            effectsHandler.PlaySpriteVFX(_vfxAnimHash, vfxOffset);
            effectsHandler.PlaySound(abilitySound);
        }
        TriggerAnimation(animationHash);

        OnAttack?.Invoke();
    }

    public void RegisterHit(GameObject enemy)
    {
        OnAttackHit?.Invoke(enemy);

        Debug.Log($"Hit {enemy.name} for {damage} damage with a {atkDir} attack!");
    }

    public void GetAttackDirection(Vector2 movementVector)
    {
        // Update lastXDir if X input is non-zero
        if (movementVector.x < 0)
            lastXDir = AttackDirection.Left;
        else if (movementVector.x > 0)
            lastXDir = AttackDirection.Right;

        // Set atkDir based on Y input.MovementVector, otherwise use lastXDir
        if (movementVector.y > 0)
            atkDir = lastXDir;
        else if (movementVector.y < 0)
            atkDir = lastXDir;
        else
        {
            atkDir = lastXDir;
        }
    }
}
