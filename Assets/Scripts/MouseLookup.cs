using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamp to prevent excessive looking up/down

        // Rotate the entire PlayerParent object (which rotates the character and camera horizontally)
        playerBody.Rotate(Vector3.up * mouseX);

        // Rotate the camera itself up and down
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
