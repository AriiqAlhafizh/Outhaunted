using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("References")]
    public List<AttackAnim> attackAnims;

    [Header("Settings")]
    public float attackCooldown = 0.5f; // Cooldown in seconds

    [Header("Debug")]
    [SerializeField] private PlayerDirection atkDir = PlayerDirection.Right;
    [SerializeField] private PlayerDirection lastXDir = PlayerDirection.Right;
    [SerializeField] private float lastAttackTime = -Mathf.Infinity;

    public void MoveAttackDirection(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        // Update lastXDir if X input is non-zero
        if (input.x < 0)
            lastXDir = PlayerDirection.Left;
        else if (input.x > 0)
            lastXDir = PlayerDirection.Right;

        // Set atkDir based on Y input, otherwise use lastXDir
        if (input.y > 0)
            atkDir = PlayerDirection.Up;
        else if (input.y < 0)
            atkDir = PlayerDirection.Down;
        else
            atkDir = lastXDir;
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.started && Time.time >= lastAttackTime + attackCooldown)
        {
            StartAttack(atkDir);
            lastAttackTime = Time.time;
            Debug.Log("Attack in direction: " + atkDir);
        }
    }

    private void StartAttack(PlayerDirection dir)
    {
        attackAnims[(int)dir].TriggerAttack();
    }
}
