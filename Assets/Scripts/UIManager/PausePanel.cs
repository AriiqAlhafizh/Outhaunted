using UnityEngine;
using UnityEngine.InputSystem;

public class PausePanel : MonoBehaviour
{
    [SerializeField]
    private GameObject PausePrefab;
    private PlayerInput playerInput;

    private void Start()
    {
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();

        playerInput.actions["Pause"].performed += ShowPause;
        playerInput.actions["Pause"].performed += HidePause;
    }

    private void OnDisable()
    {
        playerInput.actions["Pause"].performed -= ShowPause;
        playerInput.actions["Pause"].performed -= HidePause;
    }

    public void ShowPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PausePrefab.SetActive(true);

        }
    }

    public void HidePause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PausePrefab.SetActive(true);

        }
    }
}
