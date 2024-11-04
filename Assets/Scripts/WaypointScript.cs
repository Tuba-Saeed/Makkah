using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointScript : MonoBehaviour
{
    public Transform[] targets;  // Array of target transforms (for two targets)
    public float rotationSpeed;
    public float movementSpeed = 5f;  // Speed of movement
    public float arrivalThreshold = 1.5f; // Distance to consider the target reached

    private int currentTargetIndex = 0; // Track the current target

    void Start()
    {
        // Optional: Set the initial position if you want to start at the first target
        if (targets.Length > 0)
        {
            transform.position = targets[0].position;
        }
    }

    void Update()
    {
        if (targets.Length == 0) return; // Ensure that there are targets assigned

        // Rotate towards the current target
        RotateTowardsTarget();

        // Move towards the current target
        MoveTowardsTarget();

        // Check if the object has reached the target
        if (Vector3.Distance(transform.position, targets[currentTargetIndex].position) < arrivalThreshold)
        {
            // Switch to the next target
            currentTargetIndex++;

            // If all targets are reached, reset to the first target (loop)
            if (currentTargetIndex >= targets.Length)
            {
                currentTargetIndex = 0; // Loop back to the first target
            }
        }
    }

    void RotateTowardsTarget()
    {
        // Rotate towards the current target
        Vector3 direction = (targets[currentTargetIndex].position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    void MoveTowardsTarget()
    {
        // Move towards the target
        Vector3 direction = (targets[currentTargetIndex].position - transform.position).normalized;
        transform.position += direction * movementSpeed * Time.deltaTime;
    }
}
