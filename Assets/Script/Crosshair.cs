using UnityEngine;

public class Crosshair : MonoBehaviour
{
    private void Update()
    {
        if (Cursor.visible)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}