using System.Collections;
using UnityEngine;

public class NormalProjectile : MonoBehaviour
{ 
    [Header("Projectile Settings")]
    public Vector2 mapBounds;
    public float speed;
    public Vector3 direction;

    public void Start()
    {
        if (transform.position.x > 0)
        {
            direction = Vector3.left;
        }
        else
        {
            direction = Vector3.right;
        }
    }
    public void Update()
    {
        transform.Translate(speed * Time.deltaTime * direction);

        if (transform.position.x < mapBounds.x || transform.position.x > mapBounds.y)
        {
            Destroy(gameObject);
        }
    }
}