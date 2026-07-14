using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackSO", menuName = "ScriptableObjects/Ability/Normal")]
public class AttackSO : AbilitySO
{
    [Header("References")]
    [SerializeField] private CharacterData characterData;
    private NewPlayerMovement pMovement;
    private InputReader input;
    private Rigidbody2D rb;

    [Header("Settings")]
    private float attackDamage;
    private bool canAttack = true;

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
        pMovement = player.GetComponent<NewPlayerMovement>();
        input = _input;

        cooldownDuration = characterData.attackCooldown;
        attackDamage = characterData.attackDamage;

        rb = player.GetComponent<Rigidbody2D>();

        input.AttackPressed += Attack;
        input.MovementChanged += GetAttackDirection;
    }

    public override void OnDestroyAbility()
    {
        input.AttackPressed -= Attack;
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
        Debug.Log($"Attacking in direction: {atkDir}");
        StartCooldown();
        OnAttack?.Invoke();
    }

    protected IEnumerator AttackCooldown() //fix later
    {
        //Animation
        yield return null;
    }

    public void RegisterHit(GameObject enemy)
    {
        OnAttackHit?.Invoke(enemy);

        Debug.Log($"Hit {enemy.name} for {attackDamage} damage with a {atkDir} attack!");
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
            atkDir = AttackDirection.Up;
        else if (movementVector.y < 0)
            atkDir = AttackDirection.Down;
        else
        {
            atkDir = lastXDir;
        }

        if (atkDir == AttackDirection.Down && !pMovement.IsGrounded())
        {
            return;
        }
        else
        {
            AttackDirectionChanged?.Invoke(lastXDir);
        }
    }
}
