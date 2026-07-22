using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTutorial : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GoToMainMenu();
        }
    }

    public void GoToMainMenu()
    {
        SceneTransitionManager.Instance.TransitionToScene("MainMenu");
    }
}
