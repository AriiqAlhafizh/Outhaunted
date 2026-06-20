using UnityEditor.Animations;
using UnityEngine;

public class Slash : MonoBehaviour
{
    AnimatorController animatorController;
    Animator animator;
    PlayerAttack pAttack;
    Collider2D col;

    private void Start()
    {
        animatorController = PlayerStatsManager.Instance.CurrentCharacter.attackAnimator;
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = animatorController;

        pAttack = GetComponentInParent<PlayerAttack>();

        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            pAttack.RegisterHit(collision.gameObject);
            Debug.Log("Hit " + collision.gameObject.name);
        }
    }

    public void TriggerAttack(AttackDirection dir)
    { 
        animator.SetTrigger("Attack" + dir.ToString());
    }
}
