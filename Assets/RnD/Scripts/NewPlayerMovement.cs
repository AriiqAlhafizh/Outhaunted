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

    private Rigidbody2D rb;
    private Collider2D col;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed; // units per second (m/s kalau meter jadi satuan di game)
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
    private bool canJump;
    private float coyoteTimeCounter;
    private bool wasGrounded;
    private float jumpBufferCounter;

    public event Action OnMove;
    public event Action OnJump;
    public event Action OnLand;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();

        inputReader.JumpPressed += JumpPressed;
        inputReader.JumpReleased += JumpReleased;

        canMove = true;
        canJump = true;

        moveSpeed = characterData.moveSpeed;
        jumpForce = characterData.jumpForce;
    }
    private void OnDisable()
    {
        inputReader.JumpPressed -= JumpPressed;
        inputReader.JumpReleased -= JumpReleased;
    }
    private void Update()
    {
        CheckGround();
        JumpHandler();
        Move();
    }

    public void Move()
    {
        if (canMove)
        {
            if (inputReader.IsMoving && IsGrounded())
            {
                //Animation
            }
            else if (IsGrounded())
            {
                //Animation
            }

            // HORIZONTAL MOVEMENT LOGIC
            if (inputReader.MovementVector.x < 0)
            {
                dir = AttackDirection.Left;
                transform.rotation = new Quaternion(0, 180, 0, 0);
            }
            else if (inputReader.MovementVector.x > 0)
            {
                dir = AttackDirection.Right;
                transform.rotation = new Quaternion(0, 0, 0, 0);
            }

            // CHECK WALL
            if ((inputReader.MovementVector.x > 0 && IsTouchingWall(Vector2.right)))
                moveX = Mathf.Min(0f, inputReader.MovementVector.x); // allow left, block right
            else if ((inputReader.MovementVector.x < 0 && IsTouchingWall(Vector2.left)))
                moveX = Mathf.Max(0f, inputReader.MovementVector.x); // allow right, block left
            else
                moveX = inputReader.MovementVector.x;


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
            if (rb.linearVelocity.y < -1f)
            {
                //Animation
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
    public void JumpPressed()
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
        //Animation
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

    public void CheckGround()
    {
        // GROUND CHECK 
        bool grounded = IsGrounded();

        // Detect landing (transition from not grounded to grounded)
        if (grounded && !wasGrounded && rb.linearVelocity.y < 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

            //Animation

            OnLand?.Invoke();
        }

        // Update wasGrounded for next frame
        wasGrounded = grounded;
    }

    public bool IsGrounded()
    {
        float extraHeight = 0.1f;
        float rayLength = .0f + extraHeight;

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
        float rayLength = .3f + extraDistance;
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

    public void OnHitKnockback(Vector2 sourcePos)
    {
        StartCoroutine(KnockbackCoroutine(sourcePos));
    }

    public IEnumerator KnockbackCoroutine(Vector2 sourcePos)
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

    public void OwnAttackKnockback(GameObject enemy)
    {
        StartCoroutine(OwnAttackKnockbackCoroutine(enemy));
    }

    public IEnumerator OwnAttackKnockbackCoroutine(GameObject enemy)
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
