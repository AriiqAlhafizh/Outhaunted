using System.Collections;
using UnityEngine;

public class BounceAttack : BossAttack
{
    [Header("Bounce Attack Settings")]
    public Vector2 MapBoundTL;
    public Vector2 MapBoundBR;
    public int bounceCountMin;
    public int bounceCountMax;

    [Header("Jump Settings")]
    public float jumpDuration;
    public int jumpHeight;
    public int jumpDistance;
    public Vector3 jumpDirection; // 1 for right, -1 for left

    public override void Start()
    {
        base.Start();
        ActionEvent += ExecuteAttack;
    }

    private void OnDisable()
    {
        ActionEvent -= ExecuteAttack;
    }

    private void ExecuteAttack()
    {
        StartCoroutine(BounceCoroutine());
    }

    private IEnumerator BounceCoroutine()
    {
        int bounceCount = Random.Range(bounceCountMin, bounceCountMax + 1);
        Duration = jumpDuration * bounceCount;
        for (int i = 0; i < bounceCount; i++)
        {
            Vector2 targetPos = transform.position + new Vector3(jumpDistance * jumpDirection.x, 0);
            yield return StartCoroutine(JumpCoroutine(transform.position, targetPos));
        }
    }

    private IEnumerator JumpCoroutine(Vector2 startPos, Vector2 targetPos)
    {
        float timeElapsed = 0f;
        float minX = MapBoundTL.x;
        float maxX = MapBoundBR.x;

        while (timeElapsed < jumpDuration)
        {
            float t = timeElapsed / jumpDuration;

            // Calculate raw X unconstrained
            float rawX = Mathf.Lerp(startPos.x, targetPos.x, t);
            float actualX = rawX;

            // Reflect if exceeding bounds and reverse future direction
            if (rawX < minX)
            {
                actualX = minX + (minX - rawX); // Bounce right off the left wall
                jumpDirection = new Vector3(1, 0, 0);
            }
            else if (rawX > maxX)
            {
                actualX = maxX - (rawX - maxX); // Bounce left off the right wall
                jumpDirection = new Vector3(-1, 0, 0);
            }

            // Calculate Y with parabolic arc
            float currentY = Mathf.Lerp(startPos.y, targetPos.y, t);
            float arcHeight = 4 * jumpHeight * t * (1 - t);
            currentY += arcHeight;

            transform.position = new Vector3(actualX, currentY, transform.position.z);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Apply same reflection logic for the exact final frame
        float finalRawX = targetPos.x;
        float finalX = finalRawX;

        if (finalRawX < minX)
        {
            finalX = minX + (minX - finalRawX);
            jumpDirection = new Vector3(1, 0, 0);
        }
        else if (finalRawX > maxX)
        {
            finalX = maxX - (finalRawX - maxX);
            jumpDirection = new Vector3(-1, 0, 0);
        }

        transform.position = new Vector3(finalX, targetPos.y, transform.position.z);
    }
}