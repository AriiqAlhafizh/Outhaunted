using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public void SetBoss(BossData bossData)
    {
        BossManager.Instance.SetBossGameObject(bossData);
    }

    public void GotoBossScene(string bossSceneName)
    {
        SceneManager.LoadScene(bossSceneName);
    }
    public void back()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void SelectT3()
    {
         AudioManager.Instance.PlayMusic(AudioManager.Instance.T3);
    }
    public void SelectRayap()
    {
        AudioManager.Instance.PlayMusic(AudioManager.Instance.Rayap);
    }
}
