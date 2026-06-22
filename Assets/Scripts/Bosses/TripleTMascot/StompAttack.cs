using System.Collections;
using UnityEngine;

public class StompAttack : BossAttack
{
    [Header("Stomp Attack Settings")]
    public int stompCount;
    public float hoverDuration; // Duration of the hover phase
    public float hoverHeight;
    public float hoverSpeed;

    [Header("Durations")]
    public float jumpUpDuration = 0.25f;
    public float stompDownDuration = 0.1f;

    public override void Start()
    {
        base.Start();
        Duration = stompCount * (jumpUpDuration + hoverDuration + stompDownDuration);
        ActionEvent += ExecuteAttack;
    }

    private void OnDisable()
    {
        ActionEvent -= ExecuteAttack;
    }

    private void ExecuteAttack()
    {
        StartCoroutine(StompCoroutine());
    }

    private IEnumerator StompCoroutine()
    {
        float groundY = transform.position.y; // Assume it starts on the ground

        for (int i = 0; i < stompCount; i++)
        {
            // Part 1: Jump to hoverHeight
            Vector3 startPos = transform.position;
            Vector3 targetHoverPos = new Vector3(startPos.x, groundY + hoverHeight, startPos.z);

            float timeElapsed = 0f;
            while (timeElapsed < jumpUpDuration)
            {
                transform.position = Vector3.Lerp(startPos, targetHoverPos, timeElapsed / jumpUpDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            transform.position = targetHoverPos;

            // Part 2: Hover and move to player at hoverSpeed
            timeElapsed = 0f;
            while (timeElapsed < hoverDuration)
            {
                Vector3 playerPos = PlayerStatsManager.Instance.PlayerPosition;

                // Move towards player on X-axis at hoverSpeed
                float newX = Mathf.MoveTowards(transform.position.x, playerPos.x, hoverSpeed * Time.deltaTime);
                transform.position = new Vector3(newX, transform.position.y, transform.position.z);

                timeElapsed += Time.deltaTime;
                yield return null;
            }

            // Part 3: Stomp to the ground in a straight line
            Vector3 hoverPos = transform.position;
            Vector3 groundPos = new Vector3(hoverPos.x, groundY, hoverPos.z);

            timeElapsed = 0f;
            while (timeElapsed < stompDownDuration)
            {
                transform.position = Vector3.Lerp(hoverPos, groundPos, timeElapsed / stompDownDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            transform.position = groundPos;
        }
    }
}
