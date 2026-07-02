using System.Collections;
using UnityEngine;

public class JumpAttack : BossAttack
{
    Vector2 playerPos;

    public string AnimationOnAir = "OnAir";
    public string AnimationLanding = "Landing";

    [Header("Jump Attack Settings")]
    public float jumpHeight = 2f;
    public float jumpDuration = 1f;

    public override void Start()
    {
        base.Start();
        ActionEvent += ExecuteAttack;
        Duration = jumpDuration;
    }

    private void OnDisable()
    {
        ActionEvent -= ExecuteAttack;
    }

    private void ExecuteAttack()
    {
        float startY = transform.position.y;
        playerPos = PlayerManager.Instance.PlayerPosition;
        float playerVelocity = PlayerManager.Instance.CurrentPlayerContext.Rigidbody.linearVelocity.x;

        Vector2 mapBounds = new(8f, 8f); // Example map bounds, adjust as needed

        // Predict where the player will be at the end of the jump duration
        float predictedPos = playerPos.x + playerVelocity * jumpDuration;

        // Clamp the predicted position to the map bounds
        predictedPos = Mathf.Clamp(predictedPos, -mapBounds.x / 2, mapBounds.x / 2);

        StartCoroutine(JumpCoroutine(transform.position, new Vector2(predictedPos, startY)));
    }

    private IEnumerator JumpCoroutine(Vector2 startPos, Vector2 targetPos)
    {
        float timeElapsed = 0f;
        animator.Play(AnimationOnAir);

        while (timeElapsed < jumpDuration)
        {
            float t = timeElapsed / jumpDuration;

            // Linear interpolation for X and base Y
            Vector2 currentPos = Vector2.Lerp(startPos, targetPos, t);

            // Add the parabolic arc height
            // Formula: 4 * h * t * (1 - t) gives a peak of h at t = 0.5
            float arcHeight = 4 * jumpHeight * t * (1 - t);
            currentPos.y += arcHeight;

            transform.position = currentPos;

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        animator.Play(AnimationLanding);
    }
}
