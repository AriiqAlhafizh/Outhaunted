using System.Collections;
using UnityEngine;

public class CameraZoomController : MonoBehaviour
{
    public Camera cam;

    public float zoomSize = 3f;
    public float duration = 1f;

    private Vector3 originalPosition;
    private float originalSize;

    private Transform currentTarget;
    private Coroutine currentRoutine;

    private void Start()
    {
        originalPosition = transform.position;
        originalSize = cam.orthographicSize;
    }

    public void ToggleZoom(Transform target)
    {
        // Jika karakter yang sama ditekan lagi
        if (currentTarget == target)
        {
            if (currentRoutine != null)
                StopCoroutine(currentRoutine);

            currentRoutine = StartCoroutine(ZoomOutRoutine());
            currentTarget = null;
        }
        else
        {
            if (currentRoutine != null)
                StopCoroutine(currentRoutine);

            currentRoutine = StartCoroutine(ZoomInRoutine(target));
            currentTarget = target;
        }
    }

    IEnumerator ZoomInRoutine(Transform target)
    {
        Vector3 startPos = transform.position;
        float startSize = cam.orthographicSize;

        Vector3 targetPos = new Vector3(
            target.position.x,
            target.position.y,
            originalPosition.z
        );

        float timer = 0;

        while (timer < duration)
        {
            float t = timer / duration;

            transform.position =
                Vector3.Lerp(startPos, targetPos, t);

            cam.orthographicSize =
                Mathf.Lerp(startSize, zoomSize, t);

            timer += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator ZoomOutRoutine()
    {
        Vector3 startPos = transform.position;
        float startSize = cam.orthographicSize;

        float timer = 0;

        while (timer < duration)
        {
            float t = timer / duration;

            transform.position =
                Vector3.Lerp(startPos, originalPosition, t);

            cam.orthographicSize =
                Mathf.Lerp(startSize, originalSize, t);

            timer += Time.deltaTime;
            yield return null;
        }
    }
}
