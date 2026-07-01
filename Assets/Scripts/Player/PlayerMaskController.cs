using System.Collections;
using UnityEngine;

public class PlayerMaskController : MonoBehaviour
{
    public Material whiteMaterial;
    public Material flashMaterial;

    float whiteDuration;
    float flashDuration;

    SpriteRenderer sr;

    private void Start()
    {
        whiteDuration = PlayerManager.Instance.InputDisabledDuration;
        flashDuration = PlayerManager.Instance.iFrameDuration - PlayerManager.Instance.InputDisabledDuration;

        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
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
        sr.enabled = true;

        sr.material = whiteMaterial;
        yield return new WaitForSeconds(whiteDuration);

        sr.material = flashMaterial;
        yield return new WaitForSeconds(flashDuration);

        sr.enabled = false;
    }
}