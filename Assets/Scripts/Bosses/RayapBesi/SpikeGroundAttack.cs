using System.Collections;
using UnityEngine;

public class SpikeGroundAttack : BossAttack
{
    [Header("Attack Settings")]
    public float startPosY = -1.5f;
    public float warningDelay = 0.5f;
    public float attackDelay = 1f;
    public int numberOfAttacks = 1;

    [Header("Spike Settings")]
    public GameObject spikePrefab;
    public GameObject warningPrefab;
    public float distanceBetweenSpikes = 2f;

    SpriteRenderer sr;
    Collider2D col;

    public override void Start()
    {
        base.Start();
        Duration = (warningDelay + attackDelay) * numberOfAttacks;
        ActionEvent += ExecuteAttack;

        sr = GetComponentInChildren<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }
    private void OnDisable()
    {
        ActionEvent -= ExecuteAttack;
    }

    private void ExecuteAttack()
    {
        
        StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        sr.enabled = false;
        col.enabled = false;

        for (int iteration = 0; iteration < numberOfAttacks; iteration++)
        {
            float playerX = PlayerManager.Instance.PlayerPosition.x;
            for (int i = 0; i < 10; i++)
            {
                Instantiate(warningPrefab, new Vector3(playerX + (i * distanceBetweenSpikes), startPosY, 0f), Quaternion.identity);
            }
            for (int i = -1; i > -10; i--)
            {
                Instantiate(warningPrefab, new Vector3(playerX + (i * distanceBetweenSpikes), startPosY, 0f), Quaternion.identity);
            }

            yield return new WaitForSeconds(warningDelay);

            for (int i = 0; i < 10; i++)
            {
                Instantiate(spikePrefab, new Vector3(playerX + (i * distanceBetweenSpikes), startPosY, 0f), Quaternion.identity);
            }
            for (int i = -1; i > -10; i--)
            {
                Instantiate(spikePrefab, new Vector3(playerX + (i * distanceBetweenSpikes), startPosY, 0f), Quaternion.identity);
            }

            yield return new WaitForSeconds(attackDelay);
        }

        sr.enabled = true;
        col.enabled = true;
    }
}
