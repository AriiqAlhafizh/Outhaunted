using System.Collections;
using UnityEngine;

public class GlideAbility : Ability
{
    public float glideFallSpeed = 2f;
    public bool isGliding = false;

    private void Start()
    {
        context.Input.JumpPressed += StartGlide;
        context.Input.JumpReleased += StopGlide;
    }

    private void OnDisable()
    {
        context.Input.JumpPressed -= StartGlide;
        context.Input.JumpReleased -= StopGlide;
    }
    private void Glide()
    {
        if (context.Movement.isJumping && !context.Movement.IsGrounded() && context.Rigidbody.linearVelocityY < 0)
        {
            float velocityY = context.Rigidbody.linearVelocityY;
            // Only limit downward speed, preserve horizontal momentum
            if (velocityY < -glideFallSpeed)
            {
                velocityY = -glideFallSpeed;
                context.Rigidbody.linearVelocityY = velocityY;
            }
        }
    }

    private void StartGlide()
    {
        isGliding = true;
        StartCoroutine(GlideCoroutine());
    }

    private void StopGlide()
    {
        isGliding = false;
    }

    private IEnumerator GlideCoroutine()
    {
        while (isGliding)
        {
            Glide();
            yield return null;
        }
    }
}
