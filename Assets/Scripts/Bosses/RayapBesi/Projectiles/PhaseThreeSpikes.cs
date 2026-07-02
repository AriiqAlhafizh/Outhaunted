using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PhaseThreeSpikes : MonoBehaviour
{
    private static readonly int SpawnHash = Animator.StringToHash("Spawn");
    List<Animator> animators;

    private void Start()
    {
        animators = GetComponentsInChildren<Animator>().ToList();
    }

    public void ActivateSpikes()
    {
        foreach (var animator in animators)
        {
            animator.SetTrigger(SpawnHash);
        }
    }
}
