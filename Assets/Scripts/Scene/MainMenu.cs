using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        //SceneManager.LoadSceneAsync(1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void Credit()
    {
        //SceneManager.LoadSceneAsync();
    }

    public void GotoScene(string sceneName)
    {
        SceneTransitionManager.Instance.TransitionToScene(sceneName);
    }

    public void LevelSelect()
    {
        SceneTransitionManager.Instance.TransitionToScene("LevelSelect");
    }

    public void InsertCharacterData(CharacterData characterData)
    {
        PlayerManager.Instance.CurrentCharacter = characterData;
        PlayerManager.Instance.ResetStats();
    }

    public void GotoTutorialScene()
    {
        if (PlayerManager.Instance.CurrentCharacter != null)
        {
            string tutorialSceneName = PlayerManager.Instance.CurrentCharacter.tutorialSceneName;
            if (!string.IsNullOrEmpty(tutorialSceneName))
            {
                SceneTransitionManager.Instance.TransitionToScene(tutorialSceneName);
            }
            else
            {
                Debug.LogWarning("Tutorial scene name is not set for the current character.");
            }
        }
        else
        {
            Debug.LogWarning("No character selected. Please select a character before going to the tutorial.");
        }
    }
}
