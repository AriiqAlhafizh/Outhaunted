using System.Collections;
using UnityEngine;

public class PogoAbility : Ability
{
    
    private void Start()
    {
        context.Attack.OnPogo += Jump;
        context.Movement.OnLand += Land;
    }

    private void OnDisable()
    {
        context.Attack.OnPogo -= Jump;
        context.Movement.OnLand -= Land;
    }

    private void Jump()
    {
        if (!context.Movement.IsGrounded())
        {
            context.Movement.ResetCoyoteTime();
            context.Movement.JumpPressed();
        }
    }

    private void Land()
    {
        context.Movement.isJumping = false;
    }
}
