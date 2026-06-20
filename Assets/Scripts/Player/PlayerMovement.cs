using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D col;
    [SerializeField] private PlayerInputHandler input;

    [Header("Movement Settings")]
    public float moveSpeed; // units per second (m/s kalau meter jadi satuan di game)
    public AttackDirection dir = AttackDirection.Left;
    public float moveX;

    [Header("Jump Settings")]
    public float jumpForce;
    public float fallMultiplier = 2.5f; // Increases gravity when falling
    public float lowJumpMultiplier = 5f; // Increases gravity when jump is released early
    public float coyoteTime = 0.1f;
    public float jumpBufferTime = 0.15f;
    public bool isJumping;

    [SerializeField] private float coyoteTimeCounter;
    [SerializeField] private bool wasGrounded;
    [SerializeField] private float jumpBufferCounter;

    // Ability Events
    public delegate void MoveEvent();
    public event MoveEvent OnMove;

    public delegate void JumpEvent();
    public event JumpEvent OnJump;

    public delegate void LandEvent();
    public event LandEvent OnLand;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        input = GetComponent<PlayerInputHandler>();

        input.JumpPressed += JumpPressed;
        input.JumpReleased += JumpReleased;

        moveSpeed = PlayerStatsManager.Instance.CurrentCharacter.moveSpeed;
        jumpForce = PlayerStatsManager.Instance.CurrentCharacter.jumpForce;
    }

    private void OnDisable()
    {
        input.JumpPressed -= JumpPressed;
        input.JumpReleased -= JumpReleased;
    }
    private void Update()
    {
        Move();
        CheckGround();
        JumpHandler();
    }
    public void Move()
    {
        // HORIZONTAL MOVEMENT LOGIC
        if (input.MovementVector.x < 0)
            dir = AttackDirection.Left;
        else if (input.MovementVector.x > 0)
            dir = AttackDirection.Right;

        // CHECK WALL
        if ((input.MovementVector.x > 0 && IsTouchingWall(Vector2.right)))
            moveX = Mathf.Min(0f, input.MovementVector.x); // allow left, block right
        else if ((input.MovementVector.x < 0 && IsTouchingWall(Vector2.left)))
            moveX = Mathf.Max(0f, input.MovementVector.x); // allow right, block left
        else
            moveX = input.MovementVector.x;
       

        // CHECK ROOF
        if (!IsGrounded() && IsTouchingRoof())
        {
            JumpReleased();
            if (rb.linearVelocity.y > 0)
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        }

        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);

        OnMove?.Invoke();
    }
    public void CheckGround()
    {
        // GROUND CHECK 
        bool grounded = IsGrounded();

        // Detect landing (transition from not grounded to grounded)
        if (grounded && !wasGrounded && rb.linearVelocity.y < 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

            OnLand?.Invoke();
        }

        // Update wasGrounded for next frame
        wasGrounded = grounded;
    }
    public void JumpHandler()
    {
        // Handle jump buffer: if buffer is active and coyote time is available, jump
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            PerformJump(jumpForce);
            jumpBufferCounter = 0f; // Consume buffer
        }
        jumpBufferCounter -= Time.deltaTime;

        // Update coyote time
        if (IsGrounded())
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
        }
        else if (rb.linearVelocity.y > 0 && !isJumping)
        {
            // Jump released early: apply extra gravity
            rb.linearVelocity += (lowJumpMultiplier - 1) * Physics2D.gravity.y * Time.deltaTime * Vector2.up;
        }
    }

    public void JumpPressed()
    {
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
    public void ForceJump(float extJumpForce)
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
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        isJumping = true;
        OnJump?.Invoke();
    }

    public void JumpReleased()
    {
        isJumping = false;
    }
    public void ResetCoyoteTime()
    {
        coyoteTimeCounter = coyoteTime;
    }
    public bool IsGrounded()
    {
        float extraHeight = 0.1f;
        float rayLength = .2f + extraHeight;

        // Cast from left and right bottom corners
        Vector2 leftOrigin = new(col.bounds.min.x + 0.01f, col.bounds.min.y);
        Vector2 rightOrigin = new(col.bounds.max.x - 0.01f, col.bounds.min.y);

        RaycastHit2D hitLeft = Physics2D.Raycast(leftOrigin, Vector2.down, rayLength, LayerMask.GetMask("Ground"));
        RaycastHit2D hitRight = Physics2D.Raycast(rightOrigin, Vector2.down, rayLength, LayerMask.GetMask("Ground"));

        Debug.DrawRay(leftOrigin, Vector2.down * rayLength, hitLeft.collider ? Color.red : Color.green);
        Debug.DrawRay(rightOrigin, Vector2.down * rayLength, hitRight.collider ? Color.red : Color.green);

        return hitLeft.collider != null || hitRight.collider != null;
    }

    private bool IsTouchingWall(Vector2 direction)
    {
        float extraDistance = 0.1f;
        float rayLength = .6f + extraDistance;
        Vector2 top = new(rb.position.x, col.bounds.max.y - 0.01f);
        Vector2 bottom = new(rb.position.x, col.bounds.min.y + 0.01f);

        RaycastHit2D hitTop = Physics2D.Raycast(top, direction, rayLength, LayerMask.GetMask("Ground"));
        RaycastHit2D hitBottom = Physics2D.Raycast(bottom, direction, rayLength, LayerMask.GetMask("Ground"));

        Debug.DrawRay(top, direction * (rayLength), hitTop.collider ? Color.red : Color.green);
        Debug.DrawRay(bottom, direction * (rayLength), hitBottom.collider ? Color.red : Color.green);

        return hitTop.collider != null || hitBottom.collider != null;
    }

    private bool IsTouchingRoof()
    {
        float extraDistance = 0.1f;
        float rayLength = .2f + extraDistance;
        Vector2 left = new(rb.position.x - col.bounds.extents.x + 0.01f, col.bounds.max.y);
        Vector2 right = new(rb.position.x + col.bounds.extents.x - 0.01f, col.bounds.max.y);

        RaycastHit2D hitLeft = Physics2D.Raycast(left, Vector2.up, rayLength, LayerMask.GetMask("Ground"));
        RaycastHit2D hitRight = Physics2D.Raycast(right, Vector2.up, rayLength, LayerMask.GetMask("Ground"));

        Debug.DrawRay(left, Vector2.up * (rayLength), hitLeft.collider ? Color.red : Color.green);
        Debug.DrawRay(right, Vector2.up * (rayLength), hitRight.collider ? Color.red : Color.green);

        return hitLeft.collider != null || hitRight.collider != null;
    }
}
