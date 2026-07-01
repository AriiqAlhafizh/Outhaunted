using System.Collections;
using UnityEngine;

public class SideAttack : Ability
{
    [Header("Attack Settings")]
    public Vector2 spriteOffset;

    public Animator animator;
    public AttackDirection direction;
    public Collider2D sideAttackCol;

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

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            context.Attack.RegisterHit(collision.gameObject);
            BossManager.Instance.TakeDamage((int)context.Attack.attackDamage);
            Debug.Log("Hit " + collision.gameObject.name);
        }
    }

    public virtual void TriggerAttack()
    {
        if (direction == AttackDirection.Left || direction == AttackDirection.Right)
        {
            animator.SetTrigger("Attack1");
            StartCoroutine(ToggleCollider(sideAttackCol, 0.2f));
        }
    }

    public virtual void ChangeAttackDirection(AttackDirection dir)
    {
        direction = dir;
        transform.localPosition = new Vector2(1, 1) * spriteOffset;
        sideAttackCol.gameObject.transform.localPosition = new Vector2(1, 1) * spriteOffset;
        //if (dir == AttackDirection.Right)
        //{
        //    transform.rotation = new Quaternion(0, 0, 0, 0);
        //    sideAttackCol.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        //}
        //else if (dir == AttackDirection.Left)
        //{
        //    transform.localPosition = new Vector2(-1, 1) * spriteOffset;
        //    transform.rotation = new Quaternion(0, 180, 0, 0);
        //    sideAttackCol.gameObject.transform.localPosition = new Vector2(-1, 1) * spriteOffset;
        //    sideAttackCol.gameObject.transform.rotation = new Quaternion(0, 180, 0, 0);
        //}

        //Debug.Log("Attack direction changed to: " + dir);
    }

    public IEnumerator ToggleCollider(Collider2D col, float s)
    {
        col.enabled = true;
        yield return new WaitForSeconds(s);
        col.enabled = false;
    }
}
