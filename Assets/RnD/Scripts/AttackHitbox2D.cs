using UnityEngine;

public class AttackHitbox2D : MonoBehaviour
{
    private Collider2D _collider;
    private float _activeTimer;
    private float _damage;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _collider.enabled = false; 
    }
    public void Activate(Vector2 size, Vector2 offset, float damage, float duration, AttackDirection attackDirection)
    {
        _damage = damage;
        _activeTimer = duration;

        if (_collider is BoxCollider2D box)
        {
            if (attackDirection == AttackDirection.Up || attackDirection == AttackDirection.Down)
            {
                box.size = new Vector2(size.y, size.x);
                box.offset = new Vector2(offset.y, offset.x * Mathf.Sign(attackDirection == AttackDirection.Up ? 1 : -1));
            }
            else
            {
                box.size = size;

                //box.offset = new Vector2(offset.x * Mathf.Sign(attackDirection == AttackDirection.Right ? 1 : -1), offset.y);

                box.offset = offset;
            }
        }

        _collider.enabled = true;
    }

    private void Update()
    {
        if (_collider.enabled)
        {
            _activeTimer -= Time.deltaTime;
            if (_activeTimer <= 0f)
            {
                _collider.enabled = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log($"Musuh terkena serangan sebesar: {_damage}");
        }
    }
}
