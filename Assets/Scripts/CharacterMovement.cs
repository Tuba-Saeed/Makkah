using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator _animator;  // Reference to the Animator

    [Header("Player Movement")]
    public float movementSpeed = 1.1f;  // Speed of player movement
    public float runningMultiplier = 2f; // Multiplier for running

    [Header("Player Animator & Gravity")]
    public CharacterController cC;
    public float gravity = -5f;

    [Header("Player Script Camera")]
    public Transform PlayerCamera; // Reference to the main camera

    [Header("Player Jumping & Velocity")]
    public float jumpRange = 3f;
    public float turnCalmTime = 0.1f;
    float turnCalmVelocity;
    Vector3 velocity;
    public Transform surfaceCheck;
    bool onSurface;
    public float surfaceDistance = 0.4f;
    public LayerMask surfaceMask;

    // New Variable to Control Movement
    private bool isMovementEnabled = true;  // Flag to control movement (true allows movement, false disables it)

    private void Update()
    {
        if (isMovementEnabled)
        {
            // Use keyboard input for velocity calculation
            Vector3 moveInput = new Vector3(
                Input.GetAxis("Horizontal"), // Keyboard input
                0,
                Input.GetAxis("Vertical")).normalized; // Keyboard input

            float speedModifier = Input.GetButton("Sprint") ? runningMultiplier : 1f; // Check if sprinting

            _rigidbody.velocity = moveInput * movementSpeed * speedModifier;

            // Rotate player in the direction of movement
            if (moveInput.magnitude > 0)
            {
                transform.rotation = Quaternion.LookRotation(_rigidbody.velocity);
            }

            // Check if the player is on the ground
            onSurface = Physics.CheckSphere(surfaceCheck.position, surfaceDistance, surfaceMask);

            if (onSurface && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            // Apply gravity
            velocity.y += gravity * Time.deltaTime;
            cC.Move(velocity * Time.deltaTime);

            // Handle jumping
            Jump();

            // Update animator
            UpdateAnimator(moveInput);
        }
    }

    void Jump()
    {
        if (onSurface && Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jumping");
            velocity.y = Mathf.Sqrt(jumpRange * -2f * gravity);
        }
    }

    // Update the animator based on player speed
    void UpdateAnimator(Vector3 moveInput)
    {
        float speed = moveInput.magnitude;

        // Set "Speed" parameter for animations based on movement speed
        if (Input.GetButton("Sprint"))
        {
            _animator.SetFloat("Speed", 2f);  // Run animation
        }
        else if (speed > 0.1f)
        {
            _animator.SetFloat("Speed", 1f);  // Walk animation
        }
        else
        {
            _animator.SetFloat("Speed", 0f);  // Idle animation
        }
    }

    // Method to disable movement (called when player reaches the Marwa beacon)
    public void DisableMovement()
    {
        isMovementEnabled = false;
        Debug.Log("Movement Disabled");
    }

    // Method to enable movement (called when player reaches the Safa beacon)
    public void EnableMovement()
    {
        isMovementEnabled = true;
        Debug.Log("Movement Enabled");
    }
}
