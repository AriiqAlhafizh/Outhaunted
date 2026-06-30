using System.Collections;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private static readonly int TurnAroundPreparationHash = Animator.StringToHash("TurnAround_Preparation");
    private static readonly int TurnAround1Hash = Animator.StringToHash("TurnAround_1");
    private static readonly int TurnAround2Hash = Animator.StringToHash("TurnAround_2");

    Animator animator;
    public Collider2D hitbox;
    public float flipDuration;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void FlipPlatform(float delayBeforeFlip)
    {
        StartCoroutine(FlipPlatformCoroutine(delayBeforeFlip));
    }

    private IEnumerator FlipPlatformCoroutine(float delayBeforeFlip)
    {
        animator.Play(TurnAroundPreparationHash);
        yield return new WaitForSeconds(delayBeforeFlip);

        animator.Play(TurnAround1Hash);
        hitbox.enabled = true;

        yield return new WaitForSeconds(flipDuration);
        animator.Play(TurnAround2Hash);
        hitbox.enabled = false;
    }
}