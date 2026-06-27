using System.Collections;
using UnityEngine;

public class JumpAttack : BossAttack
{
    Vector3 playerPos;

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
        playerPos = PlayerStatsManager.Instance.PlayerPosition;
        Vector2 playerVelocity = PlayerStatsManager.Instance.CurrentPlayerContext.Rigidbody.linearVelocity;

        Vector2 mapBounds = new(18f, 10f); // Example map bounds, adjust as needed

        // Predict where the player will be at the end of the jump duration
        Vector3 predictedPos = playerPos + (Vector3)playerVelocity * jumpDuration;

        // Clamp the predicted position to the map bounds
        predictedPos.x = Mathf.Clamp(predictedPos.x, -mapBounds.x / 2, mapBounds.x / 2);
        predictedPos.y = Mathf.Clamp(predictedPos.y, -mapBounds.y / 2, mapBounds.y / 2);

        StartCoroutine(JumpCoroutine(transform.position, predictedPos));
    }

    private IEnumerator JumpCoroutine(Vector3 startPos, Vector3 targetPos)
    {
        float timeElapsed = 0f;
        animator.Play(AnimationOnAir);

        while (timeElapsed < jumpDuration)
        {
            float t = timeElapsed / jumpDuration;

            // Linear interpolation for X and base Y
            Vector3 currentPos = Vector3.Lerp(startPos, targetPos, t);

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
