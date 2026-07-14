using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "ScriptableObjects/InputReader")]
public class InputReader : ScriptableObject, InputSystem_Actions.IPlayerActions
{
    [Header("Input Values")]
    public Vector2 MovementVector;

    private InputSystem_Actions gameInput;
    public bool IsMoving => MovementVector.x != 0;
    public bool IsInAir => MovementVector.y != 0;

    public event Action UpPressed;
    public event Action RightPressed;
    public event Action DownPressed;
    public event Action LeftPressed;

    public event Action<Vector2> MovementChanged;

    public event Action JumpPressed;
    public event Action JumpReleased;

    public event Action DashPressed;
    public event Action AttackPressed;

    void OnEnable()
    {
        if (gameInput == null)
        {
            gameInput = new InputSystem_Actions();
            gameInput.Player.SetCallbacks(this);
        }
        setPlayer();
    }
    public void setUI()
    {
        gameInput.Player.Disable();
        gameInput.UI.Enable();
    }

    public void setPlayer()
    {
        gameInput.Player.Enable();
        gameInput.UI.Disable();
    }

    void OnDisable()
    {
        gameInput.Player.Disable();
        gameInput.UI.Disable();
        gameInput.Dispose();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
            AttackPressed?.Invoke();
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            JumpPressed?.Invoke();

        if (context.canceled)
            JumpReleased?.Invoke();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementVector = context.ReadValue<Vector2>();

        MovementChanged?.Invoke(MovementVector);

        if (context.performed)
        {
            if (MovementVector.y > 0)
                UpPressed?.Invoke();
            else if (MovementVector.y < 0)
                DownPressed?.Invoke();
            if (MovementVector.x > 0)
                RightPressed?.Invoke();
            else if (MovementVector.x < 0)
                LeftPressed?.Invoke();
        }
    }

    public void OnNext(InputAction.CallbackContext context)
    {
        
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        setUI();
    }

    public void OnPrevious(InputAction.CallbackContext context)
    {
        
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        
    }
}
