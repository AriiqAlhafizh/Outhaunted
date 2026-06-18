using UnityEngine;

public class AttackAnim : MonoBehaviour
{
    Animator animator;
    SpriteRenderer sr;
    Collider2D col;

    private void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            
        }
    }

    public void TriggerAttack()
    { 
        animator.SetTrigger("Attack");
    }
}
