using UnityEngine;

public class MapEditorCameraController : PlayerController
{
    public Camera cam;
    public float movementSpeed = 10.0f;
    public float mouseSensitivity = 100.0f;
    public float clampAngle = 80.0f;

    private float verticalRotation = 0.0f;
    private float horizontalRotation = 0.0f;

    public override void OnInit()
    {
        base.OnInit();

        Vector3 rot = transform.localRotation.eulerAngles;
        verticalRotation = rot.x;
        horizontalRotation = rot.y;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        // Movement
        float horizontal = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
        transform.Translate(horizontal, 0, vertical);

        // Rotation
        if (Input.GetMouseButton(1)) // Right mouse button
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Removed Time.deltaTime for more direct mouse movement to rotation mapping
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            verticalRotation -= mouseY;
            // Consider re-enabling the clamp if you encounter issues with the camera flipping over
            verticalRotation = Mathf.Clamp(verticalRotation, -clampAngle, clampAngle);

            horizontalRotation += mouseX;

            Quaternion localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0.0f);
            Camera.main.transform.rotation = localRotation;
        }
        else 
        {
            if(!Cursor.visible) 
            {
                Cursor.lockState = CursorLockMode.None;        
                Cursor.visible = true;
            }
        }
    }

    void Start()
    {
        OnInit();
    }

    void Update()
    {
        OnUpdate();
    }
}
