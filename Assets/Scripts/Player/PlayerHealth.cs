using System;
using System.Collections;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class PlayerHealth : MonoBehaviour
{
    [Header("References")]
    public PlayerMovement movement;

    //[Header("Health Settings")]

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (!PlayerStatsManager.Instance.inIFrame)
            { 
                PlayerStatsManager.Instance.TakeDamage();
                movement.OnHitKnockback(collision.transform.position);
            }
        }
    }
}