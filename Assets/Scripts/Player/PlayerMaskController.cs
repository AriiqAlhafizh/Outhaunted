using System.Collections;
using UnityEngine;

public class PlayerMaskController : MonoBehaviour
{
    public Material whiteMaterial;
    public Material flashMaterial;
    Material originalMaterial;

    float whiteDuration;
    float flashDuration;

    SpriteRenderer sr;

    private void Start()
    {
        whiteDuration = PlayerManager.Instance.InputDisabledDuration;
        flashDuration = PlayerManager.Instance.iFrameDuration - PlayerManager.Instance.InputDisabledDuration;

        sr = GetComponent<SpriteRenderer>();
        originalMaterial = sr.material;
        PlayerManager.Instance.OnDamaged += StartDamageFlash;
    }

    private void OnDisable()
    {
        PlayerManager.Instance.OnDamaged -= StartDamageFlash;
    }

    private void StartDamageFlash()
    {
        StartCoroutine(StartDamageFlashCoroutine());
    }

    private IEnumerator StartDamageFlashCoroutine()
    {
        sr.material = whiteMaterial;
        yield return new WaitForSeconds(whiteDuration);

        sr.material = flashMaterial;
        yield return new WaitForSeconds(flashDuration);

        sr.material = originalMaterial;
    }
}