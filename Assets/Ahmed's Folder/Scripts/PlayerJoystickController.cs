using Cinemachine;
using UnityEngine;

public class PlayerJoystickController : MonoBehaviour
{
    public static PlayerJoystickController instance;

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 15f;
    [SerializeField] private float runSpeed = 20f;
    [SerializeField] private float rotationSpeed = 100f; // degrees per second

    [Header("References")]
    [SerializeField] private Joystick joystick;
    [SerializeField] private CinemachineVirtualCamera cinemachineCamera;
    [SerializeField] private Animator anim;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        float verticalInput = joystick.Vertical;

        // Check if there is significant vertical input
        if (Mathf.Abs(verticalInput) >= 0.1f)
        {
            anim.SetFloat("Speed", verticalInput);
            anim.SetFloat("AnimationSpeed", verticalInput);

            // Check if moving backward
            if (verticalInput < 0)
            {
                // anim.SetFloat("Speed", 0f);  // Ensure "walk" and "run" are off
                anim.SetBool("Run", false);

                /*
                // Move backwards relative to the player's current facing direction
                Vector3 moveDirection = transform.forward * verticalInput * walkSpeed * Time.deltaTime;
                transform.Translate(moveDirection, Space.World);
                */
            }
            else
            {
                // If verticalInput is greater than 0.5f, run, otherwise walk
                if (verticalInput > 0.9f)
                {
                    // Set run animation
                    anim.SetBool("Run", true);

                    // Run forward relative to the player's current facing direction
                    Vector3 moveDirection = transform.forward * verticalInput * runSpeed * Time.deltaTime;
                    transform.Translate(moveDirection, Space.World);
                }
                else
                {
                    // Set forward walk animation
                    // anim.SetFloat("Speed", 0.5f);
                    anim.SetBool("Run", false);   // Ensure run is off

                    // Walk forward relative to the player's current facing direction
                    Vector3 moveDirection = transform.forward * verticalInput * walkSpeed * Time.deltaTime;
                    transform.Translate(moveDirection, Space.World);
                }

                
            }
        }
        else
        {
            // No vertical input, stop walking or running
            if (anim.GetFloat("Speed") >= 0)
            {
                anim.SetFloat("Speed", 0);
            }

            if (anim.GetBool("Run"))
            {
                anim.SetBool("Run", false);
            }
        }
    }



    private void HandleRotation()
    {
        float horizontalInput = joystick.Horizontal;

        if (Mathf.Abs(horizontalInput) >= 0.1f)
        {
            // Calculate the new rotation angle around the y-axis
            float rotationAmount = horizontalInput * rotationSpeed * Time.deltaTime;
            Vector3 currentRotation = transform.rotation.eulerAngles;
            float newYRotation = currentRotation.y + rotationAmount;

            // Set the rotation, keeping x and z rotation unchanged
            transform.rotation = Quaternion.Euler(0f, newYRotation, 0f);
        }

        if (cinemachineCamera != null)
        {
            var composer = cinemachineCamera.GetCinemachineComponent<CinemachineComposer>();
            if (composer != null)
            {
                composer.m_TrackedObjectOffset = transform.position;
            }
        }
    }
}
