using System.Collections;
using UnityEngine;

public class PogoAbility : Ability
{
    PlayerSFX playerSFX;
    public AudioClip pogoSound;
    private void Start()
    {
        playerSFX = GetComponentInChildren<PlayerSFX>();
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
            playerSFX.PlayAudio(pogoSound);
            context.Movement.ResetCoyoteTime();
            context.Movement.JumpPressed();
        }
    }

    private void Land()
    {
        context.Movement.isJumping = false;
    }
}
