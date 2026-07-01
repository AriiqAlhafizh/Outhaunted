using System.Collections;
using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    PlayerContext context;

    private void Start()
    {
        context = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerContext>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            context.Attack.RegisterHit(collision.gameObject);
            context.Movement.OwnAttackKnockback(collision.gameObject);
            if (collision.gameObject.GetComponent<BossController>() != null) 
            {
                BossManager.Instance.TakeDamage((int)context.Attack.attackDamage);
            }
        }
    }
}