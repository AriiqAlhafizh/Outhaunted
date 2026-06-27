using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public void SetBoss(BossData bossData)
    {
        BossStatsManager.Instance.SetBossGameObject(bossData);
    }

    public void GotoBossScene()
    {
        SceneManager.LoadScene("BossTestScene");
    }
}
