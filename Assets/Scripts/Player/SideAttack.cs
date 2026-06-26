using UnityEditor.Animations;
using UnityEngine;

public class SideAttack : Ability
{
    [Header("Attack Settings")]
    public Vector2 spriteOffset;

    Animator animator;
    Collider2D col;

    protected override void Awake()
    {
        context = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerContext>();
    }
    private void Start()
    {
        animator = GetComponent<Animator>();

        context.Attack.OnAttack += TriggerAttack;
        context.Attack.AttackDirectionChanged += ChangeAttackDirection;
    }

    private void OnDisable()
    {
        context.Attack.OnAttack -= TriggerAttack;
        context.Attack.AttackDirectionChanged -= ChangeAttackDirection;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            context.Attack.RegisterHit(collision.gameObject);
            Debug.Log("Hit " + collision.gameObject.name);
        }
    }

    public void TriggerAttack(AttackDirection dir)
    {
        animator.SetTrigger("Attack1");
    }

    public void ChangeAttackDirection(AttackDirection dir)
    {
        if (dir == AttackDirection.Right)
        {
            transform.localPosition = new Vector2(1, 1) * spriteOffset;
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        else if (dir == AttackDirection.Left)
        {
            transform.localPosition = new Vector2(-1, 1) * spriteOffset;
            transform.rotation = new Quaternion(0, 180, 0, 0);
        }
    }
}
