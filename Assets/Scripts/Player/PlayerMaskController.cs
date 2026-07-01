using System.Collections;
using UnityEngine;

public class PlayerMaskController : MonoBehaviour
{
    public Material maskMaterial;

    public float flashSpeed = 1.0f;
    public float damageFlashDuration = 1.3f; // Duration of the flash effect in seconds

    SpriteRenderer sr;
    Material defaultMask;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        defaultMask = sr.material;
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
        sr.material = maskMaterial;
        float elapsedTime = 0f;
        float opacity = 1f;

        while (elapsedTime < damageFlashDuration)
        {
            opacity = Mathf.PingPong(elapsedTime * flashSpeed, 1f);
            sr.material.color = new Color(255f, 255f, 255f, opacity);
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        sr.material = defaultMask;
    }
}