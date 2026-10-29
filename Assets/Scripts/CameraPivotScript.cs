using UnityEngine;
using UnityEngine.InputSystem;

public class CameraPivotScript : MonoBehaviour
{
    //camera movement stuffz
    public float sens = 0.1f;
    public float minY = -70f;
    public float maxY = 70f;
    private float camYLimit;
    private float camXLimit;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        camXLimit = transform.localEulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 theMouse = Mouse.current.delta.ReadValue();
        float mouseX = theMouse.x * sens;
        float mouseY = theMouse.y * sens;
        camXLimit += mouseX;
        camYLimit -= mouseY;
        camYLimit = Mathf.Clamp(camYLimit, minY, maxY);
        transform.localRotation = Quaternion.Euler(camYLimit, camXLimit, 0f);
        
    }
}
