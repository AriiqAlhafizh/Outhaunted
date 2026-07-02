using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RayapBesiController : BossController
{
    private SpriteRenderer sr;
    private Collider2D col;

    public bool ChangingPhase = false;

    [Header("Phase 3 Settings")]
    public GameObject platforms;
    public GameObject spikes;

    protected override void Start()
    {
        base.Start();
        sr = GetComponentInChildren<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        platforms = GameObject.FindGameObjectWithTag("PlatformParent");
        platforms.SetActive(false);

        spikes = GameObject.FindGameObjectWithTag("Spike");
        spikes.SetActive(false);
    }
    public override IEnumerator AttackCycleCoroutine()
    {
        while (true)
        {
            if (ChangingPhase)
            {
                yield return new WaitForSeconds(0.1f);
                continue;
            }

            int randomAttack = UnityEngine.Random.Range(0, startingAttacks + currentPhase);
            Debug.Log("Trying Event No. " + randomAttack);

            yield return StartCoroutine(Attacks[randomAttack].Execute());

            yield return new WaitForSeconds(delayBetweenAttacks);
        }
    }

    public override void OnDamaged()
    {
        if (currentPhase == 0 && BossManager.Instance.CurrentHealth <= BossManager.Instance.MaxHealth * 0.75f)
        {
            IncreasePhase();
        }
        else if (currentPhase == 1 && BossManager.Instance.CurrentHealth <= BossManager.Instance.MaxHealth * 0.60f)
        {
            IncreasePhase();
        }
        else if (currentPhase == 2 && BossManager.Instance.CurrentHealth <= BossManager.Instance.MaxHealth * 0.40f)
        {
            IncreasePhase();
            StopCoroutine(Attacks[3].Execute());
            StartCoroutine(ChangePhaseCoroutine());
        }
    }

    private IEnumerator ChangePhaseCoroutine()
    {
        Attacks[2].IsReady = false;
        Attacks[3].IsReady = false;

        ChangingPhase = true;

        // buat dia ilang
        sr.enabled = false;
        col.enabled = false;

        platforms.SetActive(true);

        // masukin animasi indikator obj akan hilang disini

        transform.position += new Vector3(0, 2.25f, 0);
        yield return MoveCamera(Vector3.up, 1f, 3f);

        spikes.SetActive(true);
        spikes.GetComponent<PhaseThreeSpikes>().ActivateSpikes();

        yield return new WaitForSeconds(3f);

        // buat dia muncul lagi
        sr.enabled = true;
        col.enabled = true;

        ChangingPhase = false;
    }

    IEnumerator MoveCamera(Vector3 direction, float distance, float duration)
    {
        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
        // 1. Record starting conditions
        Vector3 startPosition = cam.transform.position;
        // Calculate exact target position
        Vector3 targetPosition = startPosition + (direction.normalized * distance);


        float elapsedTime = 0f;

        // 2. Loop until the duration has been met
        while (elapsedTime < duration)
        {
            // Track time safely across frames
            elapsedTime += Time.deltaTime;

            // Calculate normalized time (goes from 0.0 to 1.0)
            float percentageComplete = elapsedTime / duration;

            // Smoothly interpolate position
            cam.transform.position = Vector3.Lerp(startPosition, targetPosition, percentageComplete);

            // Yield control back to Unity until the next frame
            yield return null;
        }

        // 3. Snap exactly to the target to avoid rounding errors
        cam.transform.position = targetPosition;
    }
}