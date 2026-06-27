using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Values")]
    public Vector2 MovementVector;

    [Header("References")]
    [SerializeField] private PlayerInput playerInput;
    //[SerializeField] private PlayerAnimations pAnimation;

    public bool IsMoving => MovementVector.x != 0;
    public bool IsInAir => MovementVector.y != 0;

    public event Action UpPressed;
    public event Action RightPressed;
    public event Action DownPressed;
    public event Action LeftPressed;

    public event Action JumpPressed;
    public event Action JumpReleased;

    public event Action DashPressed;
    public event Action AttackPressed;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.SwitchCurrentActionMap("Player");

        //pAnimation = GetComponent<PlayerAnimations>();

        //JumpPressed += StartJumpAnimation;
        //JumpReleased += StartOnAirAnimation;
        //AttackPressed += StartAttackAnimation;
    }
    private void OnDisable()
    {
        //JumpPressed -= StartJumpAnimation;
        //JumpReleased -= StartOnAirAnimation;
        //AttackPressed -= StartAttackAnimation;
    }

    //private void Update()
    //{
    //    SetAnimationMovementBool();
    //}
    public void OnMove(InputAction.CallbackContext context)
    {
        MovementVector = context.ReadValue<Vector2>();

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

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            JumpPressed?.Invoke();

        if (context.canceled)
            JumpReleased?.Invoke();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
            DashPressed?.Invoke();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
            AttackPressed?.Invoke();
    }

    // ANIMATION
    //public void StartAttackAnimation()
    //{
    //    pAnimation.StartAttack();
    //}

    //public void StartJumpAnimation()
    //{
    //    pAnimation.StartJump();
    //}

    //public void StartOnAirAnimation()
    //{
    //    pAnimation.StartOnAir();
    //}

    //public void SetAnimationMovementBool()
    //{
    //    if (!IsMoving && !IsInAir)
    //        pAnimation.SetBool(pAnimation.IsWalkingHash, false);
    //    else if (IsMoving && !IsInAir)
    //        pAnimation.SetBool(pAnimation.IsWalkingHash, true);
    //}
}