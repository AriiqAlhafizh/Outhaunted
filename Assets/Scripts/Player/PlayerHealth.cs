using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("References")]
    public PlayerMovement movement;

    //[Header("Health Settings")]

    // Event
    public event Action<int> OnDamaged;
    public event Action OnDeath;

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }
    public void TakeDamage(int damage, GameObject source)
    {
        PlayerStatsManager.Instance.CurrentHealth -= damage;
        OnDamaged?.Invoke(damage);
        movement.OnHitKnockback(source.transform.position);

        if (PlayerStatsManager.Instance.CurrentHealth <= 0)
        {
            PlayerStatsManager.Instance.CurrentHealth = 0;
            OnDeath?.Invoke();
        }

        Debug.Log($"Player took {damage} damage from {source.name}. Current health: {PlayerStatsManager.Instance.CurrentHealth}");
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (!PlayerStatsManager.Instance.inIFrame)
            {
                TakeDamage(1, collision.gameObject);
            }
        }
    }
}