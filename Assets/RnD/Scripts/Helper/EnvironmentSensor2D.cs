using UnityEngine;

public class EnvironmentSensor2D : MonoBehaviour
{
    private Collider2D col;
    private Rigidbody2D rb;

    public bool isGrounded => IsGrounded();
    public bool isTouchingRightWall => IsTouchingWall(Vector2.right) ;

    public bool isTouchingLeftWall => IsTouchingWall(Vector2.left);
    public bool isTouchingRoof => IsTouchingRoof();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
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
}
