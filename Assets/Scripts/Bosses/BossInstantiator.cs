using UnityEngine;

public class BossInstantiator : MonoBehaviour
{
    private void Start()
    {
        SpawnBoss();
    }
    public void SpawnBoss()
    {
        GameObject Boss = Instantiate(BossManager.Instance.CurrentBoss.bossPrefab, BossManager.Instance.CurrentBoss.spawnPos, Quaternion.identity);
        
        BossManager.Instance.ResetBossStats();

        Boss.GetComponent<BossController>().InitializeStats(BossManager.Instance.CurrentBoss);

        Destroy(gameObject);
    }
}
