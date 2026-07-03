using System.Collections;
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
        StartCoroutine(ResetJumpsCoroutine());
    }

    private IEnumerator ResetJumpsCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
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

            foreach (var item in context.Attack.pAnimation.animator)
            {
                item.Play(DoubleJumpHash);       
            }

        }
    }
}
