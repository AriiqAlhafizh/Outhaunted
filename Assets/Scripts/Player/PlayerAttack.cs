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
    public PlayerInputHandler input;
    public PlayerAnimations pAnimation;

    [Header("Settings")]
    public float attackCooldown = 0.5f; // Cooldown in seconds
    public float attackDamage;

    [Header("Debug")]
    public AttackDirection atkDir = AttackDirection.Right;
    [SerializeField] private AttackDirection lastXDir = AttackDirection.Right;
    [SerializeField] private float lastAttackTime = -Mathf.Infinity;

    // Events
    public event Action OnPogo;

    public event Action<AttackDirection> AttackDirectionChanged;
    public event Action<AttackDirection> OnAttack;
    public event Action<GameObject> OnAttackHit;


    private void Start()
    {
        //attackAnim = GetComponentInChildren<SideAttack>();
        input = GetComponent<PlayerInputHandler>();
        pAnimation = GetComponent<PlayerAnimations>();
        
        attackCooldown = PlayerStatsManager.Instance.CurrentCharacter.attackCooldown;
        attackDamage = PlayerStatsManager.Instance.CurrentCharacter.attackDamage;

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
            AttackDirectionChanged?.Invoke(lastXDir);
            atkDir = lastXDir;
        }
    }
    public void Attack()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            StartAttack(atkDir);
            lastAttackTime = Time.time;
        }
    }
    private void StartAttack(AttackDirection dir)
    {
        StartCoroutine(AttackCooldown());
        OnAttack?.Invoke(dir);
        pAnimation.StartAttack();
    }

    private IEnumerator AttackCooldown()
    {
        pAnimation.SetIsAttacking(true);
        yield return new WaitForSeconds(attackCooldown);
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
    public void DecreaseDamage(float amount)
    {
        attackDamage -= amount;
    }
    public void ResetDamage()
    {
        attackDamage = PlayerStatsManager.Instance.CurrentCharacter.attackDamage;
    }
}
