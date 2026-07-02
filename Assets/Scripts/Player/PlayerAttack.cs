using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
public enum AttackDirection
{
    Up,
    Right,
    Down,
    Left
}

public enum AttackType
{
    Basic,
    Pogo,
    Upslash
}
public class PlayerAttack : MonoBehaviour
{
    [Header("References")]
    //public SideAttack attackAnim;
    public PlayerMovement pMovement;
    public PlayerInputHandler input;
    public PlayerAnimations pAnimation;
    public PogoAbility playerPogo;
    Rigidbody2D rb;

    [Header("Settings")]
    public float attackCooldown = 0.5f; // Normal attack cooldown in seconds
    public float pogoCooldown = 0.5f; // Pogo attack cooldown in seconds
    public float attackDamage;
    public bool canAttack = true;

    [Header("Debug")]
    public AttackDirection atkDir = AttackDirection.Right;
    [SerializeField] protected AttackDirection lastXDir = AttackDirection.Right;
    [SerializeField] protected float lastAttackTime = -Mathf.Infinity;
    [SerializeField] protected float lastPogoAttackTime = -Mathf.Infinity;

    // Events
    public event Action OnPogo;

    public event Action<AttackDirection> AttackDirectionChanged;
    public virtual event Action OnAttack;
    public event Action<GameObject> OnAttackHit;


    private void Start()
    {
        try
        {
            playerPogo = GetComponent<PogoAbility>();
        }
        catch (Exception)
        {
            Debug.Log("Player doesn't have PogoAbility component");
        }
        //attackAnim = GetComponentInChildren<SideAttack>();
        pMovement = GetComponent<PlayerMovement>();
        input = GetComponent<PlayerInputHandler>();
        pAnimation = GetComponent<PlayerAnimations>();
        
        attackCooldown = PlayerManager.Instance.CurrentCharacter.attackCooldown;
        pogoCooldown = PlayerManager.Instance.CurrentCharacter.pogoCooldown;
        attackDamage = PlayerManager.Instance.CurrentCharacter.attackDamage;

        rb = GetComponent<Rigidbody2D>();

        input.AttackPressed += Attack;
    }
    private void OnDisable()
    {
        input.AttackPressed -= Attack;
    }
    private void Update()
    {
        GetAttackDirection();
    }
    public void GetAttackDirection()
    {
        // Update lastXDir if X input is non-zero
        if (input.MovementVector.x < 0)
            lastXDir = AttackDirection.Left;
        else if (input.MovementVector.x > 0)
            lastXDir = AttackDirection.Right;

        // Set atkDir based on Y input.MovementVector, otherwise use lastXDir
        if (input.MovementVector.y > 0)
            atkDir = AttackDirection.Up;
        else if (input.MovementVector.y < 0)
            atkDir = AttackDirection.Down;
        else
        {
            atkDir = lastXDir;
        }

        if (playerPogo != null && !pMovement.IsGrounded())
        {
            AttackDirectionChanged?.Invoke(atkDir);
        }
        else
        {
            AttackDirectionChanged?.Invoke(lastXDir);
        }
    }
    public void Attack()
    {
        bool isPogo = atkDir == AttackDirection.Down && playerPogo != null && !pMovement.IsGrounded();
        float currentCooldown = isPogo ? pogoCooldown : attackCooldown;
        float lastTime = isPogo ? lastPogoAttackTime : lastAttackTime;

        if (Time.time >= lastTime + currentCooldown 
            && canAttack 
            && !PlayerManager.Instance.inIFrame)
        {
            if (isPogo)
                lastPogoAttackTime = Time.time;
            else
                lastAttackTime = Time.time;

            StartAttack();
        }
    }
    protected virtual void StartAttack()
    {
        StartCoroutine(AttackCooldown());
        pAnimation.StartAttack();
        OnAttack?.Invoke();
    }

    protected IEnumerator AttackCooldown()
    {
        pAnimation.SetIsAttacking(true);
        yield return new WaitForSeconds(pAnimation.GetAnimationLength("Attack_1"));
        pAnimation.SetIsAttacking(false);
    }

    public void RegisterHit(GameObject enemy)
    {
        if (atkDir == AttackDirection.Down)
            OnPogo?.Invoke();

        OnAttackHit?.Invoke(enemy);

        Debug.Log($"Hit {enemy.name} for {attackDamage} damage with a {atkDir} attack!");
    }

    public void IncreaseDamage(float amount)
    {
        attackDamage += amount;
    }
    public void IncreaseSize( GameObject obj, float amount)
    {
        obj.transform.localScale = Vector3.one * (1 + amount);
    }
    public void ResetDamage()
    {
        attackDamage = PlayerManager.Instance.CurrentCharacter.attackDamage;
    }
    public void ResetSize(GameObject obj, float amount)
    {
        obj.transform.localScale = Vector3.one * (1 / (1 + amount));
    }
}
