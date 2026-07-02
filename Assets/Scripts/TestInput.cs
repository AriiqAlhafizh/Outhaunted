using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestInput : MonoBehaviour
{
    public PlayerInput playerInput;
    public GameObject settingPrefab;

    private bool isSettingsActive = false;
    private void Start()
    {
        StartCoroutine(GetPlayerInputCoroutine());
    }

    private void OnDisable()
    {
        playerInput.actions["Pause"].performed -= ToggleSettings;
    }

    public void ToggleSettings(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isSettingsActive = !isSettingsActive;
            settingPrefab.SetActive(isSettingsActive);

            if (isSettingsActive)
            {
                Time.timeScale = 0f; // Uncomment to pause the game
            }
            else
            {
                Time.timeScale = 1f; // Uncomment to unpause the game
            }
        }
    }

    public IEnumerator GetPlayerInputCoroutine()
    {
        while (playerInput == null)
        {
            try
            {
                playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("PlayerInput not found. Retrying...");
            }
            yield return new WaitForSeconds(0.1f);
        }
        playerInput.actions["Pause"].performed += ToggleSettings;
    }
}
