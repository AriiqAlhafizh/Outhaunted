using UnityEngine;

public class SpikeGroundAttackFollow : BossAttack
{
    [Header("Attack Settings")]
    public float startPosY = -1.5f;
    public float attackDelay = 1f;

    [Header("Spike Settings")]
    public GameObject spikePrefab;
    public float distanceBetweenSpikes = 2f;

    public override void Start()
    {
        base.Start();
        ActionEvent += ExecuteAttack;
    }
    private void OnDisable()
    {
        ActionEvent -= ExecuteAttack;
    }

    private void ExecuteAttack()
    {
        float playerX = PlayerStatsManager.Instance.PlayerPosition.x;
        Instantiate(spikePrefab, new Vector3(playerX, startPosY, 0f), Quaternion.identity);
    }
}
