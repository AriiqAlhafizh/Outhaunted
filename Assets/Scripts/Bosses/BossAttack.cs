using System;
using System.Collections;
using UnityEngine;
public class BossAttack : MonoBehaviour 
{
    private static readonly int EndAttackHash = Animator.StringToHash("EndAttack");

    [Header("Attack Settings")]
    public Action ActionEvent;
    public float DelayBeforeAttack;
    public float Duration;
    public float Cooldown;
    public bool IsReady;
    public AudioClip attackPrepSound;
    public AudioClip attackSound;

    BossSFX bossSFX;

    [Header("Animation Settings")]
    public Animator animator;
    public string AnimationPrep;
    public string AnimationAttack;

    public virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
        bossSFX = GetComponentInChildren<BossSFX>();
    }
    public IEnumerator Execute()
    {
        if (IsReady)
        {
            bossSFX.Play(attackPrepSound);
            animator.Play(AnimationPrep);
            yield return new WaitForSeconds(DelayBeforeAttack);

            bossSFX.Play(attackSound);
            animator.Play(AnimationAttack);
            ActionEvent?.Invoke();
            yield return new WaitForSeconds(Duration);

            StartCoroutine(StartCDCoroutine());
            animator.SetTrigger(EndAttackHash);
        }
    }

    private IEnumerator StartCDCoroutine()
    {
        IsReady = false;
        yield return new WaitForSeconds(Cooldown);
        IsReady = true;
    }
}