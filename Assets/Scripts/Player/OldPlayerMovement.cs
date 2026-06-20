//using System;
//using UnityEngine;
//using UnityEngine.InputSystem;

//public class OldPlayerMovement : MonoBehaviour
//{
//    [Header("References")]
//    [SerializeField] private Rigidbody2D rb;
//    [SerializeField] private Collider2D col;
//    [SerializeField] private PlayerInput playerInput;

//    [Header("Movement Settings")]
//    public float movSpeed = 5f; // units per second (m/s kalau meter jadi satuan di game)
//    public AttackDirection dir = AttackDirection.Left;
//    [SerializeField] private float moveX;
//    [SerializeField] private float inputX;

//    [Header("Jump Settings")]
//    public float jumpForce = 7.5f;
//    public float fallMultiplier = 2.5f; // Increases gravity when falling
//    public float lowJumpMultiplier = 5f; // Increases gravity when jump is released early
//    public float coyoteTime = 0.1f;
//    public float jumpBufferTime = 0.15f;
    
//    [SerializeField] private bool isJumping;
//    [SerializeField] private float coyoteTimeCounter;
//    [SerializeField] private bool wasGrounded;
//    [SerializeField] private int maxJumps;
//    [SerializeField] private int jumpsLeft;
//    [SerializeField] private float jumpBufferCounter;

//    [Header("Dash Settings")]
//    public float dashSpeed = 15f;
//    public float dashDuration = 0.2f;
//    public float dashCooldown = 1f; // Long cooldown after all dashes used
//    public float dashChainCooldown = 0.2f; // Short cooldown between consecutive dashes

//    [SerializeField] private int maxConsecutiveDashes;
//    [SerializeField] private int dashesLeft;
//    [SerializeField] private float dashDirection;
//    [SerializeField] private bool isDashing;
//    [SerializeField] private float dashTimeLeft;
//    [SerializeField] private float dashCooldownTimer;
//    [SerializeField] private float dashChainCooldownTimer;
//    private void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        col = GetComponent<BoxCollider2D>();
//        playerInput = GetComponent<PlayerInput>();
//        playerInput.SwitchCurrentActionMap("Player");
//        maxJumps = PlayerStatsManager.Instance.maxJumps;
//        maxConsecutiveDashes = PlayerStatsManager.Instance.maxDash;
//        jumpsLeft = maxJumps;
//        dashesLeft = maxConsecutiveDashes;
//    }
//    private void Update()
//    {
//        // DASH LOGIC
//        HandleDash();
//        if (isDashing)
//        {
//            // Skip the rest of Update while dashing
//            wasGrounded = IsGrounded();
//            return;
//        }

//        // HORIZONTAL MOVEMENT LOGIC
//        if (!IsGrounded() && (inputX > 0 && IsTouchingWall(Vector2.right)) || (inputX < 0 && IsTouchingWall(Vector2.left)))
//        {
//            moveX = 0f;
//        }
//        else
//        {
//            moveX = inputX;
//        }

//        // GROUND CHECK 
//        bool grounded = IsGrounded();

//        // Detect landing (transition from not grounded to grounded)
//        if (grounded && !wasGrounded && rb.linearVelocity.y < 0)
//        {
//            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
//        }

//        rb.linearVelocity = new Vector2(moveX * movSpeed, rb.linearVelocity.y);

//        // JUMP LOGIC
//        if (jumpBufferCounter > 0)
//        {
//            StartJump();
//        }
//        jumpBufferCounter -= Time.deltaTime;

//        // GROUNDED LOGIC 
//        /*
//         * Reset Jump
//         * Reset Dash
//         * CoyoteTime
//         */
//        if (IsGrounded())
//        {
//            coyoteTimeCounter = coyoteTime;
//            jumpsLeft = maxJumps;
//            dashesLeft = maxConsecutiveDashes; // Reset dashes on landing
//            //dashChainCooldownTimer = 0f;
//        }
//        else
//        {
//            coyoteTimeCounter -= Time.deltaTime;
//        }

//        // Variable jump height logic
//        if (rb.linearVelocity.y < 0)
//        {
//            // Falling: apply extra gravity
//            rb.linearVelocity += (fallMultiplier - 1) * Physics2D.gravity.y * Time.deltaTime * Vector2.up;
//        }
//        else if (rb.linearVelocity.y > 0 && !isJumping)
//        {
//            // Jump released early: apply extra gravity
//            rb.linearVelocity += (lowJumpMultiplier - 1) * Physics2D.gravity.y * Time.deltaTime * Vector2.up;
//        }

//        // Update wasGrounded for next frame
//        wasGrounded = grounded;
//    }

//    public void Move(InputAction.CallbackContext context)
//    {
//        inputX = context.ReadValue<Vector2>().x;
//        if (inputX < 0)
//            dir = AttackDirection.Left;
//        else if (inputX > 0)
//            dir = AttackDirection.Right;
//    }

//    public void Jump(InputAction.CallbackContext context)
//    {
//        if (context.started)
//        {
//            jumpBufferCounter = jumpBufferTime;
//        }
//        else if (context.canceled)
//        {
//            isJumping = false;
//        }
//    }

//    private void StartJump()
//    {
//        if ((coyoteTimeCounter > 0 || jumpsLeft > 1))
//        {
//            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
//            isJumping = true;
//            if (!IsGrounded())
//                jumpsLeft--;
//            jumpBufferCounter = 0; // Consume buffer
//        }
//    }
//    public void Dash(InputAction.CallbackContext context)
//    {
//        if (context.started && !isDashing)
//        {
//            // Can dash if: have dashes left, and chain cooldown is over, and not in long cooldown
//            if (dashesLeft > 0 && dashChainCooldownTimer <= 0f && dashCooldownTimer <= 0f)
//            {
//                StartDash();
//            }
//        }
//    }
//    private void StartDash()
//    {
//        isDashing = true;
//        dashTimeLeft = dashDuration;
//        //dashDirection = Mathf.Abs(inputX) > 0.01f ? Mathf.Sign(inputX) : Mathf.Sign(transform.localScale.x);
//        dashDirection = (dir == AttackDirection.Left) ? -1 : 1;
//        rb.linearVelocity = new Vector2(dashDirection * dashSpeed, 0f);

//        dashesLeft--;

//        if (dashesLeft > 0)
//        {
//            dashChainCooldownTimer = dashChainCooldown; // Short cooldown before next dash
//        }
//        else
//        {
//            dashCooldownTimer = dashCooldown; // Long cooldown after all dashes used
//        }
//    }
//    private void HandleDash()
//    {
//        if (isDashing)
//        {
//            dashTimeLeft -= Time.deltaTime;
//            rb.linearVelocity = new Vector2(dashDirection * dashSpeed, 0f);
//            moveX = 0f;

//            if (dashTimeLeft <= 0)
//            {
//                isDashing = false;
//            }
//        }
//        else
//        {
//            if (dashChainCooldownTimer > 0)
//                dashChainCooldownTimer -= Time.deltaTime;
//            if (dashCooldownTimer > 0)
//                dashCooldownTimer -= Time.deltaTime;
//        }
//    }
//    private bool IsGrounded()
//    {
//        float extraHeight = 0.1f;
//        float rayLength = .2f + extraHeight;

//        // Cast from left and right bottom corners
//        Vector2 leftOrigin = new(col.bounds.min.x + 0.01f, col.bounds.min.y);
//        Vector2 rightOrigin = new(col.bounds.max.x - 0.01f, col.bounds.min.y);

//        RaycastHit2D hitLeft = Physics2D.Raycast(leftOrigin, Vector2.down, rayLength, LayerMask.GetMask("Ground"));
//        RaycastHit2D hitRight = Physics2D.Raycast(rightOrigin, Vector2.down, rayLength, LayerMask.GetMask("Ground"));

//        Debug.DrawRay(leftOrigin, Vector2.down * rayLength, hitLeft.collider ? Color.red : Color.green);
//        Debug.DrawRay(rightOrigin, Vector2.down * rayLength, hitRight.collider ? Color.red : Color.green);

//        return hitLeft.collider != null || hitRight.collider != null;
//    }

//    private bool IsTouchingWall(Vector2 direction)
//    {
//        float extraDistance = 0.1f;
//        float rayLength = .6f + extraDistance;
//        Vector2 top = new(rb.position.x, col.bounds.max.y - 0.01f);
//        Vector2 bottom = new(rb.position.x, col.bounds.min.y + 0.01f);

//        RaycastHit2D hitTop = Physics2D.Raycast(top, direction, rayLength, LayerMask.GetMask("Ground"));
//        RaycastHit2D hitBottom = Physics2D.Raycast(bottom, direction, rayLength, LayerMask.GetMask("Ground"));

//        Debug.DrawRay(top, direction * (rayLength), hitTop.collider ? Color.red : Color.green);
//        Debug.DrawRay(bottom, direction * (rayLength), hitBottom.collider ? Color.red : Color.green);

//        return hitTop.collider != null || hitBottom.collider != null;
//    }
//}
