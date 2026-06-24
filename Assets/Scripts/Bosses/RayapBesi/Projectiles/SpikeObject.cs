using System.Collections;
using UnityEngine;

public class SpikeObject : MonoBehaviour
{
    
    void Start()
    {
        StartCoroutine(DestroyAfter(2f));
    }

    private IEnumerator DestroyAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
