using System.Collections;
using UnityEngine;

public class SpikeObject : MonoBehaviour
{
    private static readonly int StopHash = Animator.StringToHash("Stop");
    private static readonly int StartHash = Animator.StringToHash("Spawn");
    Animator animator;
    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        StartCoroutine(DestroyAfter(2f));
    }

    private IEnumerator DestroyAfter(float seconds)
    {
        animator.Play(StartHash);
        yield return new WaitForSeconds(seconds);
        animator.Play(StopHash);
        yield return new WaitForSeconds(20/60f);
        Destroy(gameObject);
    }

}
