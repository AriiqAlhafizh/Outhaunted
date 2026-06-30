using UnityEngine;

public class DoubleJumpAbility : Ability
{
    private static readonly int DoubleJumpHash = Animator.StringToHash("DoubleJump");
    public int extraJumps = 1;
    public int jumps = 0;

    private void Start()
    {
        context.Input.JumpPressed += Jump;
        context.Movement.OnLand += ResetJumps;
        context.Attack.OnPogo += ResetJumps;
        context.Movement.OnJump += IncreaseJumps;
    }

    private void OnDisable()
    {
        context.Input.JumpPressed -= Jump;
        context.Movement.OnLand -= ResetJumps;
        context.Attack.OnPogo -= ResetJumps;
        context.Movement.OnJump -= IncreaseJumps;
    }

    private void ResetJumps()
    {
        jumps = 0;
    }

    private void IncreaseJumps()
    {
        if (!context.Movement.IsGrounded())
            jumps++;
    }

    private void Jump()
    {
        if (!context.Movement.IsGrounded() && jumps < extraJumps)
        {
            context.Movement.ResetCoyoteTime();
            context.Movement.JumpPressed();

            context.Attack.pAnimation.animator.Play(DoubleJumpHash);
        }
    }
}
