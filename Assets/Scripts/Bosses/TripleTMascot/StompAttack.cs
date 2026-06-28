using System.Collections;
using UnityEngine;

public class StompAttack : BossAttack
{
    public string AnimationPrepare = "Slam_GoUp_Preparation";
    public string AnimationGoUp= "Slam_GoUp";
    public string AnimationOnAir = "Roundabout";
    public string AnimationGoDown = "Slam_Down";
    public string AnimationStomp= "Slam_Landing";

    [Header("Stomp Attack Settings")]
    public int stompCount;
    public float hoverDuration; // Duration of the hover phase
    public float hoverHeight;
    public float hoverSpeed;
    public float delayBetweenStomps; // Delay between each stomp

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
        animator.Play(AnimationPrepare);
        yield return new WaitForSeconds(delayBetweenStomps);
        float groundY = transform.position.y; // Assume it starts on the ground

        for (int i = 0; i < stompCount; i++)
        {
            // Part 1: Jump to hoverHeight
            animator.Play(AnimationGoUp);
            Vector3 startPos = transform.position;
            Vector3 playerPos = PlayerManager.Instance.PlayerPosition;
            Vector3 targetHoverPos = new Vector3(playerPos.x, groundY + hoverHeight, startPos.z);

            float timeElapsed = 0f;
            while (timeElapsed < jumpUpDuration)
            {
                transform.position = Vector3.Lerp(startPos, targetHoverPos, timeElapsed / jumpUpDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            transform.position = targetHoverPos;

            // Part 2: Hover and move to player at hoverSpeed
            animator.Play(AnimationOnAir);
            timeElapsed = 0f;
            while (timeElapsed < hoverDuration)
            {
                Vector3 curPlayerPos = PlayerManager.Instance.PlayerPosition;
                // Move towards player on X-axis at hoverSpeed
                float newX = Mathf.MoveTowards(transform.position.x, curPlayerPos.x, hoverSpeed * Time.deltaTime);
                transform.position = new Vector3(newX, transform.position.y, transform.position.z);

                timeElapsed += Time.deltaTime;
                yield return null;
            }

            // Part 3: Stomp to the ground in a straight line
            animator.Play(AnimationGoDown);
            Vector3 hoverPos = transform.position;
            Vector3 groundPos = new Vector3(hoverPos.x, groundY, hoverPos.z);

            timeElapsed = 0f;
            while (timeElapsed < stompDownDuration)
            {
                transform.position = Vector3.Lerp(hoverPos, groundPos, timeElapsed / stompDownDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            animator.Play(AnimationStomp);
            transform.position = groundPos;

            yield return new WaitForSeconds(delayBetweenStomps);
        }
    }
}
