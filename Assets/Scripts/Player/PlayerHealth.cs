using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("References")]
    public PlayerMovement movement;

    private void Start()
    {
        movement = GetComponentInParent<PlayerMovement>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!PlayerManager.Instance.inIFrame)
            { 
                PlayerManager.Instance.TakeDamage();
                movement.OnHitKnockback(collision.transform.position);
            }
        }
    }
}