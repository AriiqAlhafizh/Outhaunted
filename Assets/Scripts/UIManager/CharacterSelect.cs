using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    public CameraZoomController cameraController;

    public Transform pocong;
    public Transform kuntilanak;

    public void SelectPocong()
    {
        cameraController.ToggleZoom(pocong);
    }

    public void SelectKuntilanak()
    {
        cameraController.ToggleZoom(kuntilanak);
    }

}