using UnityEngine;

public class KuntilanakTutorial : MonoBehaviour
{
    private Camera mainCam;

    [Header("Camera Follow Settings")]
    public Transform target;
    public float minX = -10f;
    public float maxX = 10f;
    public float smoothTime = 0.3f;

    public Vector2 spawnPoint;

    private float velocityX = 0f;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            FollowTargetSmoothly();
        }
        else
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    public void FollowTargetSmoothly()
    {
        if (mainCam == null || target == null) return;

        // Clamp the target's X position within the customizable bounds
        float targetX = Mathf.Clamp(target.position.x, minX, maxX);

        // Smoothly damp the X position
        float newX = Mathf.SmoothDamp(mainCam.transform.position.x, targetX, ref velocityX, smoothTime);

        // Apply the new position. Keeps Y and Z the same.
        mainCam.transform.position = new Vector3(newX, mainCam.transform.position.y, mainCam.transform.position.z);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.position = spawnPoint;
        }
    }
}
