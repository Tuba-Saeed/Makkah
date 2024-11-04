using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;           // Speed of character movement
    public float turnSmoothTime = 0.1f;    // Time to smooth turning
    private float turnSmoothVelocity;      // Velocity used for turning
    public float runningSpeedMultiplier = 2f; // Multiplier for running

    [Header("Jump Settings")]
    public float jumpForce = 5f;           // Force applied when jumping
    public Transform groundCheck;          // Point to check if grounded
    public LayerMask groundMask;           // Layer mask for the ground
    public LayerMask roofMask;             // Layer mask for the roof
    public float groundDistance = 0.4f;    // Distance to detect ground

    [Header("Camera Settings")]
    public Transform cameraTransform;      // Reference to the main camera
    public float cameraSmoothSpeed = 0.125f; // Smoothness of camera follow
    public float cameraDistance = 15f;     // Distance of the camera from the player
    public float cameraHeight = 10f;       // Height of the camera above the player
    public float mouseSensitivity = 2f;    // Sensitivity of mouse movement

    [Header("Slope and Stairs Settings")]
    public float slopeForce = 10f;         // Force applied on slopes to prevent sliding
    public float slopeForceRayLength = 1.5f; // Ray length for detecting slopes
    public float slopeLimit = 45f;         // Max slope angle the character can walk on
    public LayerMask stairsMask;           // Layer mask for stairs detection

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private Animator animator;

    private float pitch = 0f; // Pitch (up/down rotation) for camera

    private void Start()
    {
        controller = GetComponent<CharacterController>(); // CharacterController reference
        animator = GetComponent<Animator>(); // Animator reference for animations
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor for better camera control

        // Adjust camera FOV for wider view
        Camera.main.fieldOfView = 80f;

        // Ensure feet are aligned with the floor when the character spawns
        AdjustFeetToFloor();
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
    }

    private void LateUpdate()
    {
        FollowPlayerWithCamera(); // Move the camera in LateUpdate to ensure it's smooth
    }

    // Handles player movement
    void HandleMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask | roofMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float horizontal = Input.GetAxisRaw("Horizontal"); // Left and Right input
        float vertical = Input.GetAxisRaw("Vertical");     // Forward and Backward input

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // Smooth turning of the character
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // Check if running (holding Shift key)
            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float speed = isRunning ? moveSpeed * runningSpeedMultiplier : moveSpeed;

            // Move character at the correct speed
            controller.Move(moveDir.normalized * speed * Time.deltaTime);

            // Update animator based on movement speed (0 for idle, 1 for walking, 2 for running)
            animator.SetFloat("Speed", isRunning ? 2f : 1f);
        }
        else
        {
            // Set animator to "Idle" when no input is detected
            animator.SetFloat("Speed", 0f);
        }

        // Apply gravity and handle slope and stairs movement
        HandleSlopeAndStairsMovement();

        // Apply vertical velocity (gravity)
        velocity.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Continuously ensure feet stay on the floor or roof
        AdjustFeetToFloor();
    }

    // Handles player jumping
    void HandleJump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
        }
    }

    // Makes the camera follow the player smoothly and rotate with the mouse
    void FollowPlayerWithCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate the player based on horizontal mouse movement
        transform.Rotate(Vector3.up * mouseX);

        // Adjust pitch for vertical mouse movement, clamping it to avoid over-rotation
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -30f, 60f);

        Vector3 targetPosition = transform.position - transform.forward * cameraDistance + Vector3.up * cameraHeight;

        Vector3 smoothedPosition = Vector3.Lerp(cameraTransform.position, targetPosition, cameraSmoothSpeed);
        cameraTransform.position = smoothedPosition;

        // Camera follows the player and looks at them with a slight pitch
        cameraTransform.rotation = Quaternion.Euler(pitch, transform.eulerAngles.y, 0f);
    }

    // Handles both slope and stairs movement
    private void HandleSlopeAndStairsMovement()
    {
        if (OnStairs())
        {
            // Apply upward movement when on stairs to mimic stepping up
            controller.Move(Vector3.up * Time.deltaTime * moveSpeed);
        }
        else if (OnSlope())
        {
            // If character is on a slope, apply slope force to prevent sliding
            velocity.y = -slopeForce;
        }
    }

    // Check if the character is on a slope and should apply slope force
    private bool OnSlope()
    {
        if (controller.isGrounded)
        {
            RaycastHit hit;
            // Cast a ray downward from the character to detect slopes
            if (Physics.Raycast(transform.position, Vector3.down, out hit, slopeForceRayLength))
            {
                // Check if the slope angle is steep
                float angle = Vector3.Angle(hit.normal, Vector3.up);
                return angle > controller.slopeLimit;
            }
        }
        return false;
    }

    // Check if the character is on stairs by detecting collision with a layer or tagged object
    private bool OnStairs()
    {
        return Physics.CheckSphere(groundCheck.position, groundDistance, stairsMask);
    }

    // Adjust character's feet to touch the ground or roof properly
    void AdjustFeetToFloor()
    {
        RaycastHit hit;
        // Cast a ray downwards from the character to find the ground or roof position
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, groundMask | roofMask))
        {
            Vector3 newPosition = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            transform.position = newPosition;  // Adjust the position so feet touch the ground
        }
    }
}
