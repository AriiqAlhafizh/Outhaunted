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
    public string AnimationStart;
    public string AnimationAttack;

    public virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }
    public IEnumerator Execute()
    {
        if (IsReady)
        {
            animator.Play(AnimationStart);
            yield return new WaitForSeconds(DelayBeforeAttack);
            animator.Play(AnimationAttack);
            ActionEvent?.Invoke();
            yield return new WaitForSeconds(Duration);
            StartCoroutine(StartCDCoroutine());
            animator.SetTrigger("EndAttack");
        }
    }

    private IEnumerator StartCDCoroutine()
    {
        IsReady = false;
        yield return new WaitForSeconds(Cooldown);
        IsReady = true;
    }
}