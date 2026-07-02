using UnityEngine;

public class BossScene : MonoBehaviour
{
    Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
        mainCam.orthographicSize = 3f;
    }
}
