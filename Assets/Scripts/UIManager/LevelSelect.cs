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
        SceneTransitionManager.Instance.TransitionToScene(bossSceneName);
    }
    public void back()
    {
        SceneTransitionManager.Instance.TransitionToScene("MainMenu");
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
