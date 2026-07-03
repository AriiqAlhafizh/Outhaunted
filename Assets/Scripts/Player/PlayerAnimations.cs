using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public List<Animator> animator;
    public RuntimeAnimatorController defaultController;
    public AnimatorOverrideController overrideController;

    private int currentStateHash;
    [SerializeField] private bool isAttacking;
    [SerializeField] private bool isInAbility;

    public readonly int PlayerHurt = Animator.StringToHash("Hurt");
    public readonly int PlayerIdle = Animator.StringToHash("Idle");
    public readonly int PlayerWalk = Animator.StringToHash("Walk");
    public readonly int PlayerJump = Animator.StringToHash("Jump");
    public readonly int PlayerOnAir = Animator.StringToHash("OnAir");
    public readonly int PlayerLanding = Animator.StringToHash("Landing");
    public readonly int PlayerAttack = Animator.StringToHash("Attack_1");

    public readonly int IsWalkingHash = Animator.StringToHash("isWalking");
    public readonly int IsFallingHash = Animator.StringToHash("isFalling");

    private void Start()
    {
        GameObject.FindGameObjectsWithTag("PlayerBodyAnimator").ToList().ForEach(
            anim => animator.Add(anim.GetComponent<Animator>())
            );
        defaultController = animator[0].runtimeAnimatorController;
    }

    public void OverrideAnimation()
    {
        foreach (var anim in animator)
        {
            anim.runtimeAnimatorController = overrideController;
        }
    }

    public void ResetAnimation()
    {
        foreach (var anim in animator)
        {
            anim.runtimeAnimatorController = defaultController;
        }
    }
    public void ChangeAnimationState(int newStateHash)
    {
        // Prevent restarting the animation if it is already playing
        if (currentStateHash == newStateHash) return;

        // Play the animation instantly without transitions
        foreach (var anim in animator)
        {
            anim.Play(newStateHash);
        }
        currentStateHash = newStateHash;
    }

    public void SetTrigger(int triggerHash)
    {
        if (!isAttacking && !isInAbility)
        {
            foreach (var anim in animator)
            {
                anim.SetTrigger(triggerHash);
            }
        }
    }

    public void SetBool(int boolHash, bool value)
    {
        if (!isAttacking && !isInAbility)
        {
            foreach (var anim in animator)
            {
                anim.SetBool(boolHash, value);
            }
        }
    }
    
    public void StartHurt()
    {
        ChangeAnimationState(PlayerHurt);
    }
    public void StartIdle()
    {
        if (!isAttacking && !isInAbility)
        {
            ChangeAnimationState(PlayerIdle);
        }
    }

    public void StartWalk()
    {
        if (!isAttacking && !isInAbility)
        {
            ChangeAnimationState(PlayerWalk);
        }
    }

    public void StartJump()
    {
        if (!isAttacking && !isInAbility)
            ChangeAnimationState(PlayerJump);
    }

    public void StartOnAir()
    {
        if (!isAttacking && !isInAbility)
            ChangeAnimationState(PlayerOnAir);
    }

    public void StartLanding()
    {
        if (!isAttacking && !isInAbility)
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

    public void SetInAbility(bool value)
    {
        isInAbility = value;
    }

    public float GetAnimationLength(string name)
    {
        // Get all clips assigned to the Animator Controller
        AnimationClip[] clips = animator[0].runtimeAnimatorController.animationClips;

        foreach (AnimationClip clip in clips)
        {
            if (clip.name == name)
            {
                return clip.length; // Returns length in seconds
            }
        }

        Debug.LogWarning($"Clip named {name} not found!");
        return 0f;
    }
}