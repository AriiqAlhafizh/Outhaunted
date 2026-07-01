using System.Collections;
using UnityEngine;

public class BossMaskController : MonoBehaviour
{
    public Material maskMaterial;
    public float damageFlashDuration = 0.2f; // Duration of the flash effect in seconds

    SpriteRenderer sr;
    Material defaultMask;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        defaultMask = sr.material;
        BossManager.Instance.OnDamaged += StartDamageFlash;
    }

    private void OnDisable()
    {
        BossManager.Instance.OnDamaged -= StartDamageFlash;
    }

    private void StartDamageFlash()
    {
        StartCoroutine(StartDamageFlashCoroutine());
    }

    private IEnumerator StartDamageFlashCoroutine()
    {
        sr.material = maskMaterial;
        yield return new WaitForSeconds(damageFlashDuration);
        sr.material = defaultMask;
    }
}