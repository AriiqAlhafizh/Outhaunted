using System.Collections;
using UnityEngine;

public class PocongSideAttack : SideAttack
{
    private static readonly int AttackPogoHash = Animator.StringToHash("Attack_Pogo");
    private static readonly int Attack1Hash = Animator.StringToHash("Attack1");
    private static readonly int PogoHash = Animator.StringToHash("Pogo");
    
    public Collider2D pogoCol;

    public override void TriggerAttack()
    {
        if (direction == AttackDirection.Left || direction == AttackDirection.Right)
        {
            animator.SetTrigger(Attack1Hash);
            context.Attack.pAnimation.StartAttack();
            StartCoroutine(ToggleCollider(sideAttackCol, 0.2f));
        }
        if (direction == AttackDirection.Down)
        {
            animator.SetTrigger(PogoHash);
            context.Attack.pAnimation.animator.Play(AttackPogoHash);
            StartCoroutine(ToggleCollider(pogoCol, 0.2f));
        }
    }

    public override void ChangeAttackDirection(AttackDirection dir)
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
        //} else
        if (dir == AttackDirection.Down)
        {
            transform.localPosition = new Vector2(0, 0);
            transform.rotation = new Quaternion(0, 0, 0, 0);
            pogoCol.gameObject.transform.localPosition = new Vector2(0, 0);
        }
    }
}
