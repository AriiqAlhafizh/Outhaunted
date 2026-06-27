using System.Collections;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public Animator animator;

    private int currentStateHash;
    private bool isAttacking;

    public readonly int PlayerIdle = Animator.StringToHash("Idle");
    public readonly int PlayerWalk = Animator.StringToHash("Walk");
    public readonly int PlayerJump = Animator.StringToHash("Jump");
    public readonly int PlayerOnAir = Animator.StringToHash("OnAir");
    public readonly int PlayerLanding = Animator.StringToHash("Landing");
    public readonly int PlayerAttack = Animator.StringToHash("Attack_1");

    public readonly int IsWalkingHash = Animator.StringToHash("isWalking");
    public readonly int JumpHash = Animator.StringToHash("Jump");
    public readonly int OnAirHash = Animator.StringToHash("OnAir");
    public readonly int LandingHash = Animator.StringToHash("Landing");

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }
    public void ChangeAnimationState(int newStateHash)
    {
        // Prevent restarting the animation if it is already playing
        if (currentStateHash == newStateHash) return;

        // Play the animation instantly without transitions
        animator.Play(newStateHash);
        currentStateHash = newStateHash;
    }

    public void SetTrigger(int triggerHash)
    {
        if ( !isAttacking)
            animator.SetTrigger(triggerHash);
    }

    public void SetBool(int boolHash, bool value)
    {
        if (!isAttacking)
            animator.SetBool(boolHash, value);
    }

    public void StartIdle()
    {
        if (!isAttacking)
        {
            ChangeAnimationState(PlayerIdle);
        }
    }

    public void StartWalk()
    {
        if (!isAttacking)
        {
            ChangeAnimationState(PlayerWalk);
        }
    }

    public void StartJump()
    {
        if (!isAttacking)
            ChangeAnimationState(PlayerJump);
    }

    public void StartOnAir()
    {
        if (!isAttacking)
            ChangeAnimationState(PlayerOnAir);
    }

    public void StartLanding()
    {
        if (!isAttacking)
            ChangeAnimationState(PlayerLanding);
    }

    public void StartAttack()
    {
        ChangeAnimationState(PlayerAttack);
    }

    public void SetIsAttacking(bool value)
    {
        isAttacking = value;
    }
}