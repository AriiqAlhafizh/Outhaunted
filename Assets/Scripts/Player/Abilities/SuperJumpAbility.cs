using System.Collections;
using UnityEngine;

public class SuperJumpAbility : Ability
{
    private static readonly int JumpPreparationHash = Animator.StringToHash("Jump_Preparation");
    [Header("Super Jump Settings")]
    public float maxJumpMultiplier = 1.5f;
    public float jumpMultiplier = .1f;
    public float chargeTime = 1f;
    public float defaultJumpForce;

    [Header("Debug")]
    public bool isCharging = false;
    public float previousYVector = 0f;
    public float chargeTimer = 0f;

    private void Start()
    {
        defaultJumpForce = PlayerManager.Instance.CurrentCharacter.jumpForce;
        context.Input.DownPressed += ChargeJump;
    }

    private void OnDisable()
    {
        context.Input.DownPressed -= ChargeJump;
    }
    public void SuperJump()
    {
        context.Movement.ForceJump(jumpMultiplier * defaultJumpForce);
    }

    public void ChargeJump()
    {
        if (context.Movement.IsGrounded())
        {
            isCharging = true;
            StartCoroutine(ChargeCoroutine());
        }
    }

    private IEnumerator ChargeCoroutine()
    {
        while (isCharging)
        {
            context.Movement.canMove = false;
            context.Rigidbody.linearVelocityX = 0f;

            foreach (var item in context.Attack.pAnimation.animators)
            {
                item.Play(JumpPreparationHash);
            }

            HandleCharge();
            
            yield return null;
        }
    }

    private void HandleCharge()
    {
        previousYVector = context.Input.MovementVector.y;
        // Start charging if grounded and holding down
        if (context.Movement.IsGrounded() && context.Input.MovementVector.y < 0)
        {
            if (!isCharging)
            {
                isCharging = true;
                chargeTimer = 0f;
                jumpMultiplier = 1f;
            }

            // Increase charge
            chargeTimer += Time.deltaTime;
            float t = Mathf.Clamp01(chargeTimer / chargeTime);
            jumpMultiplier = Mathf.Lerp(1f, maxJumpMultiplier, t);
        }
        else if (isCharging)
        {
            // Release charge and perform super jump
            SuperJump();    
            isCharging = false;
            context.Movement.canMove = true;
            chargeTimer = 0f;
            jumpMultiplier = 1f;
        }
    }
}
