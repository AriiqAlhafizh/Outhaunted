using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class NewPlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private CharacterData characterData; //subject to change
    [SerializeField] private CharacterDataAnchorSO runtimeCharacterData;

    private Rigidbody2D rb;
    private EnvironmentSensor2D environmentSensor;

    [Header("Movement Settings")]
    private Vector2 movementVector;
    private float moveSpeed; // units per second (m/s kalau meter jadi satuan di game)
    private AttackDirection dir = AttackDirection.Right;
    private float moveX;
    private bool canMove;
    private float ownAttackKnockbackForce = 1f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float fallMultiplier = 2.5f; // Increases gravity when falling
    [SerializeField] private float lowJumpMultiplier = 5f; // Increases gravity when jump is released early
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float jumpBufferTime = 0.15f;

    private bool isJumping;
    public bool canJump;
    private float coyoteTimeCounter;
    private bool wasGrounded;
    private float jumpBufferCounter;

    public event Action<Vector2> OnMove;
    public event Action OnJump;
    public event Action OnOnAir;
    public event Action OnLand;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        environmentSensor = GetComponent<EnvironmentSensor2D>();

        inputReader.MovementChanged += Move;
        inputReader.JumpPressed += JumpPressed;
        inputReader.JumpReleased += JumpReleased;

        runtimeCharacterData.OnStatChanged += StatsChange;

        canMove = true;
        canJump = true;

        moveSpeed = runtimeCharacterData.GetStat(StatType.MoveSpeed);
        jumpForce = runtimeCharacterData.GetStat(StatType.JumpForce);
    }
    private void OnDisable()
    {
        inputReader.MovementChanged -= Move;
        inputReader.JumpPressed -= JumpPressed;
        inputReader.JumpReleased -= JumpReleased;

        runtimeCharacterData.OnStatChanged -= StatsChange;
    }
    private void Update()
    {
        CheckGround();
        JumpHandler();
        MoveHandler();
    }

    private void Move(Vector2 _movementVector)
    {
        movementVector = _movementVector;
    }

    private void StatsChange(StatType type, float newValue)
    {
        switch (type)
        {
            case StatType.MoveSpeed:
                moveSpeed = newValue;
                break;
            case StatType.JumpForce:
                jumpForce = newValue;
                break;
            default:
                break;
        }
    }

    private void MoveHandler()
    {
        if (canMove)
        {
            if (environmentSensor.isGrounded)
            {
                OnMove?.Invoke(movementVector);
            }

            // HORIZONTAL MOVEMENT LOGIC
            if (movementVector.x < 0)
            {
                dir = AttackDirection.Left;
                transform.rotation = new Quaternion(0, 180, 0, 0);
            }
            else if (movementVector.x > 0)
            {
                dir = AttackDirection.Right;
                transform.rotation = new Quaternion(0, 0, 0, 0);
            }

            // CHECK WALL
            if ((movementVector.x > 0 && environmentSensor.isTouchingRightWall))
                moveX = Mathf.Min(0f, movementVector.x); // allow left, block right
            else if ((movementVector.x < 0 && environmentSensor.isTouchingLeftWall))
                moveX = Mathf.Max(0f, movementVector.x); // allow right, block left
            else
                moveX = movementVector.x;


            // CHECK ROOF
            if (!environmentSensor.isGrounded && environmentSensor.isTouchingRoof)
            {
                JumpReleased();
                if (rb.linearVelocity.y > 0)
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            }

            rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y); 
        }
    }

    private void JumpHandler()
    {
        // Handle jump buffer: if buffer is active and coyote time is available, jump
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            PerformJump(jumpForce);
            jumpBufferCounter = 0f; // Consume buffer
        }
        jumpBufferCounter -= Time.deltaTime;

        // Update coyote time
        if (environmentSensor.isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Variable jump height logic
        if (rb.linearVelocity.y < 0)
        {
            // Falling: apply extra gravity
            rb.linearVelocity += (fallMultiplier - 1) * Physics2D.gravity.y * Time.deltaTime * Vector2.up;
            if (rb.linearVelocity.y < -1f)
            {
                //Animation
                OnOnAir?.Invoke();
            }
        }
        else if (rb.linearVelocity.y > 0 && !isJumping)
        {
            // Jump released early: apply extra gravity
            rb.linearVelocity += (lowJumpMultiplier - 1) * Physics2D.gravity.y * Time.deltaTime * Vector2.up;
        }

        //// Falling
        //if (rb.linearVelocity.y < 0)
        //{
        //    pAnimation.SetBool(pAnimation.IsFallingHash, true);
        //    pAnimation.StartOnAir();
        //}
    }
    private void JumpPressed()
    {
        if (!canJump)
            return;
        // Only allow jump if within coyote time (recently grounded)
        if (coyoteTimeCounter > 0f)
        {
            PerformJump(jumpForce);
            jumpBufferCounter = 0f; // Consume buffer
        }
        else
        {
            // If not grounded, start jump buffer
            jumpBufferCounter = jumpBufferTime;
        }
    }
    private void ForceJump(float extJumpForce)
    {
        // Only allow jump if within coyote time (recently grounded)
        if (coyoteTimeCounter > 0f)
        {
            PerformJump(extJumpForce);
            jumpBufferCounter = 0f; // Consume buffer
        }
        else
        {
            // If not grounded, start jump buffer
            jumpBufferCounter = jumpBufferTime;
        }
    }

    private void PerformJump(float jumpForce)
    {
        OnJump?.Invoke();
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        isJumping = true;
    }

    public void JumpReleased()
    {
        isJumping = false;
    }
    public void ResetCoyoteTime()
    {
        coyoteTimeCounter = coyoteTime;
    }

    public void CheckGround()
    {
        // GROUND CHECK 
        bool grounded = environmentSensor.isGrounded;

        // Detect landing (transition from not grounded to grounded)
        if (grounded && !wasGrounded && rb.linearVelocity.y < 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

            OnLand?.Invoke();
        }

        // Update wasGrounded for next frame
        wasGrounded = grounded;
    }

    private void OnHitKnockback(Vector2 sourcePos)
    {
        StartCoroutine(KnockbackCoroutine(sourcePos));
    }

    private IEnumerator KnockbackCoroutine(Vector2 sourcePos)
    {
        PlayerManager.Instance.inIFrame = true;
        canMove = false;
        canJump = false;

        if (sourcePos.x < transform.position.x)
        {
            rb.linearVelocity = new Vector2(5f, 5f);
        }
        else
        {
            rb.linearVelocity = new Vector2(-5f, 5f);
        }
        //Animation

        yield return new WaitForSeconds(PlayerManager.Instance.InputDisabledDuration);

        canMove = true;
        canJump = true;

        yield return new WaitForSeconds(PlayerManager.Instance.iFrameDuration - PlayerManager.Instance.InputDisabledDuration);
        PlayerManager.Instance.inIFrame = false;
    }

    private void OwnAttackKnockback(GameObject enemy)
    {
        StartCoroutine(OwnAttackKnockbackCoroutine(enemy));
    }

    private IEnumerator OwnAttackKnockbackCoroutine(GameObject enemy)
    {
        canMove = false;
        canJump = false;
        rb.linearVelocityX = enemy.transform.position.x < transform.position.x
            ? ownAttackKnockbackForce
            : -ownAttackKnockbackForce;
        yield return new WaitForSeconds(.1f);
        canMove = true;
        canJump = true;
    }
}
