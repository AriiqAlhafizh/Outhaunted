using System;
using System.Collections;
using UnityEngine;
public class BossAttack : MonoBehaviour 
{
    [Header("Attack Settings")]
    public Action ActionEvent;
    public float DelayBeforeAttack;
    public float Duration;
    public float Cooldown;
    public bool IsReady;

    [Header("Animation Settings")]
    public Animator animator;
    public string AnimationStartTrigger;
    public string AnimationAttackTrigger;

    public virtual void Start()
    {
        animator = GetComponent<Animator>();
    }
    public IEnumerator Execute()
    {
        if (IsReady)
        {
            animator.SetTrigger(AnimationStartTrigger);
            yield return new WaitForSeconds(DelayBeforeAttack);
            animator.SetTrigger(AnimationAttackTrigger);
            ActionEvent?.Invoke();
            yield return new WaitForSeconds(Duration);
            StartCoroutine(StartCDCoroutine());
        }
    }

    private IEnumerator StartCDCoroutine()
    {
        IsReady = false;
        yield return new WaitForSeconds(Cooldown);
        IsReady = true;
    }
}