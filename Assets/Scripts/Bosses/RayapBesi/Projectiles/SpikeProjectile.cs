using System.Collections;
using UnityEngine;

public class SpikeProjectile : MonoBehaviour
{ 
    [Header("Spike Settings")]
    public GameObject spikePrefab;
    public Vector3 spikeOffset = new(0, -2.5f);

    private float spikeSpriteSizeX;

    [Header("Projectile Settings")]
    public Vector2 mapBounds;
    public float speed;
    public Vector3 direction;

    private float distanceTraveled;

    public void Start()
    {
        spikeSpriteSizeX = spikePrefab.GetComponentInChildren<SpriteRenderer>().bounds.size.x;
        if (transform.position.x > 0)
        {
            direction = Vector3.left;
        }
        else
        {
            direction = Vector3.right;
        }
        spikeOffset.x = spikeOffset.x * direction.x;
    }
    public void Update()
    {
        distanceTraveled += speed * Time.deltaTime;
        transform.position += speed * Time.deltaTime * direction;

        if (transform.position.x < mapBounds.x || transform.position.x > mapBounds.y)
        {
            Destroy(gameObject);
        }

        if (distanceTraveled > spikeSpriteSizeX)
        {
            distanceTraveled -= spikeSpriteSizeX;
            Vector3 spikePosition = transform.position + spikeOffset;
            Debug.Log("Spawning spike at: " + spikePosition + ", spike offset: " + spikeOffset);
            Instantiate(spikePrefab, spikePosition, Quaternion.identity);
        }
    }
}