using UnityEngine;

public class JoystickCharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed of the character
    public Joystick joystick;      // Reference to the joystick
    private Rigidbody playerRigidbody; // The Rigidbody component
    private Animator animator;     // Animator for character animations

    void Start()
    {
        // Get required components
        playerRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
       
            Debug.Log("Joystick Horizontal: " + joystick.Horizontal + " | Vertical: " + joystick.Vertical);
            Move();
            HandleAnimation();
        

    }

    void Move()
    {
        // Get joystick input
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        // If joystick input is above a threshold
        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            // Calculate movement direction
            Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

            // Move character
            Vector3 movement = direction * moveSpeed * Time.deltaTime;
            playerRigidbody.MovePosition(playerRigidbody.position + movement);

            // Rotate character to face movement direction
            if (direction != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
                playerRigidbody.rotation = Quaternion.RotateTowards(playerRigidbody.rotation, toRotation, 500 * Time.deltaTime);
            }
        }
    }

    void HandleAnimation()
    {
        // Set walking animation based on joystick input
        bool isWalking = Mathf.Abs(joystick.Horizontal) > 0.1f || Mathf.Abs(joystick.Vertical) > 0.1f;
        animator.SetBool("isWalking", isWalking); // Ensure this parameter exists in your Animator
    }
}
