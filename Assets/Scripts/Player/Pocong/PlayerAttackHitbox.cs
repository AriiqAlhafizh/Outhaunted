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
            BossManager.Instance.TakeDamage((int)context.Attack.attackDamage);
            Debug.Log("Hit " + collision.gameObject.name);
        }
    }
}