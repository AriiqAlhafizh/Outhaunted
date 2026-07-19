using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

public class NewPlayerAnimation : MonoBehaviour
{
    private NewPlayerMovement playerMovement;
    private AbilityHandler abilityHandler;

    private Animator animator;
    private RuntimeAnimatorController defaultController;
    private AnimatorOverrideController overrideController;

    private int currentStateHash;
    private bool isAttacking;
    private bool isInAbility;

    public readonly int PlayerHurt = Animator.StringToHash("Hurt");
    public readonly int PlayerIdle = Animator.StringToHash("Idle");
    public readonly int PlayerWalk = Animator.StringToHash("Walk");
    public readonly int PlayerJump = Animator.StringToHash("Jump");
    public readonly int PlayerOnAir = Animator.StringToHash("OnAir");
    public readonly int PlayerLanding = Animator.StringToHash("Landing");

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        playerMovement = GetComponent<NewPlayerMovement>();
        abilityHandler = GetComponent<AbilityHandler>();
    }

    private void OnEnable()
    {
        playerMovement.OnMove += HandleMovementAnimation;
        playerMovement.OnJump += PlayJump;
        playerMovement.OnOnAir += PlayOnAir;
        playerMovement.OnLand += PlayLanding;
        abilityHandler.OnPlayAbilityAnimation += PlayAbilityAnimation;
    }

    private void OnDisable()
    {
        playerMovement.OnMove -= HandleMovementAnimation;
        playerMovement.OnJump -= PlayJump;
        playerMovement.OnOnAir -= PlayOnAir;
        playerMovement.OnLand -= PlayLanding;
        abilityHandler.OnPlayAbilityAnimation -= PlayAbilityAnimation;
    }

    private void HandleMovementAnimation(Vector2 velocity)
    {
        if (IsAnimationLocked()) return;

        if (velocity.magnitude > 0.1f)
            ChangeAnimationState(PlayerWalk);
        else
            ChangeAnimationState(PlayerIdle);
    }

    private void PlayJump() => ChangeAnimationState(PlayerJump);
    private void PlayOnAir() {
        if (IsAnimationLocked()) return;
        ChangeAnimationState(PlayerOnAir); 
    } 
    private void PlayLanding() => ChangeAnimationState(PlayerLanding);
    private void PlayHurt() => ChangeAnimationState(PlayerHurt);
    private void PlayAbilityAnimation(int animHash)
    {
        ChangeAnimationState(animHash);
    }

    private void ChangeAnimationState(int newState)
    {
        if (currentStateHash == newState) return;

        animator.Play(newState, layer: 0, normalizedTime: 0f);
        currentStateHash = newState;
    }

    private bool IsAnimationLocked()
    {
        if (currentStateHash !=  PlayerWalk && currentStateHash != PlayerIdle && currentStateHash != PlayerOnAir)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.shortNameHash != currentStateHash)
                return true;

            if (stateInfo.normalizedTime < 1f)
            {
                return true; 
            }
        }

        return false; 
    }
}
