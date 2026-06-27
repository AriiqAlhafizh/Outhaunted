using UnityEngine;

public class BossInstantiator : MonoBehaviour
{
    private void Start()
    {
        SpawnBoss();
    }
    public void SpawnBoss()
    {
        GameObject Boss = Instantiate(BossStatsManager.Instance.CurrentBoss.bossPrefab, BossStatsManager.Instance.CurrentBoss.spawnPos, Quaternion.identity);
        
        BossStatsManager.Instance.ResetBossStats();

        Boss.GetComponent<BossController>().InitializeStats(BossStatsManager.Instance.CurrentBoss);

        Destroy(gameObject);
    }
}
