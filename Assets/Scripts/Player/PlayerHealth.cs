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

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!PlayerManager.Instance.inIFrame)
            { 
                PlayerManager.Instance.TakeDamage();
                movement.OnHitKnockback(collision.transform.position);
            }
        }
    }
}