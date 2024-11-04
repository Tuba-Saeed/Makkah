using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private FixedJoystick _joystick;
    [SerializeField] private Animator _animator;

    [Header("Player Movement")]
    public float movementSpeed = 1.1f;
    public float runningMultiplier = 2f;

    [Header("Jumping & Gravity")]
    public float jumpForce = 5f;
    public float gravityScale = 1f;
    public Transform surfaceCheck;
    public float surfaceDistance = 0.4f;
    public LayerMask surfaceMask;

    private bool isOnSurface;
    private bool isSprinting;
    private Vector3 moveDirection;

    private void Start()
    {
        _rigidbody.freezeRotation = true; // Ensure Rigidbody settings for consistent physics behavior
    }

    private void Update()
    {
        // Handle Input
        HandleMovement();

        // Update Animator
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        // Apply Movement and Gravity
        MovePlayer();
        ApplyGravity();
    }

    private void HandleMovement()
    {
        float verticalInput = _joystick.Vertical;

        // Check if there is significant vertical input
        if (Mathf.Abs(verticalInput) >= 0.1f)
        {
            if (verticalInput < 0)
            {
                // Set backward animation
                _animator.SetBool("back", true);
                _animator.SetBool("walk", false);
                _animator.SetBool("run", false);

                moveDirection = transform.forward * verticalInput;
            }
            else
            {
                if (verticalInput > 0.75f)
                {
                    // Set run animation
                    _animator.SetBool("run", true);
                }
                else
                {
                    // Set walk animation
                    _animator.SetBool("walk", true);
                    _animator.SetBool("run", false);
                }

                moveDirection = transform.forward * verticalInput;
                _animator.SetBool("back", false);
            }
        }
        else
        {
            // No vertical input, stop walking or running
            _animator.SetBool("walk", false);
            _animator.SetBool("run", false);
            _animator.SetBool("back", false);

            moveDirection = Vector3.zero;
        }
    }

    private void MovePlayer()
    {
        float speed = isSprinting ? movementSpeed * runningMultiplier : movementSpeed;

        Vector3 movement = moveDirection * speed;
        movement.y = _rigidbody.velocity.y;  // Preserve vertical velocity
        _rigidbody.velocity = movement;

        if (moveDirection.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    private void ApplyGravity()
    {
        isOnSurface = Physics.CheckSphere(surfaceCheck.position, surfaceDistance, surfaceMask);
        if (!isOnSurface)
        {
            _rigidbody.AddForce(Vector3.down * gravityScale, ForceMode.Acceleration);
        }
    }

    private void Jump()
    {
        if (isOnSurface)
        {
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, jumpForce, _rigidbody.velocity.z);
        }
    }

    private void UpdateAnimator()
    {
        float speed = moveDirection.magnitude * (isSprinting ? 2f : 1f);
        _animator.SetFloat("Speed", speed > 0.1f ? (isSprinting ? 2f : 1f) : 0f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(surfaceCheck.position, surfaceDistance);
    }
}
