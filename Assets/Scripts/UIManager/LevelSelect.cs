using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public void SetBoss(BossData bossData)
    {
        BossManager.Instance.SetBossGameObject(bossData);
    }

    public void GotoBossScene()
    {
        SceneManager.LoadScene("BossTestScene");
    }
    public void back()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
