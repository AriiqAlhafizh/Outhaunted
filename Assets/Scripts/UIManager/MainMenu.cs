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

    public void back()
    {
        //SceneManager.LoadSceneAsync(0);
    }

    public void LevelSelect()
    {
        SceneManager.LoadSceneAsync("LevelSelect");
    }
}
