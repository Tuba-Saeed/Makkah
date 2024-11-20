using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointScript : MonoBehaviour
{
    public Transform[] targets;  // Array of target transforms (Safa and Marwa)
    public float rotationSpeed;
    public float movementSpeed = 6f;  // Speed of movement
    public float arrivalThreshold = 1.5f; // Distance to consider the target reached
    public GameObject indicator; // Indicator to point to target

    private int currentTargetIndex = 0; // Track the current target

    void Start()
    {
        if (targets.Length > 0)
        {
            transform.position = targets[0].position;
        }
    }

    void Update()
    {
        if (targets.Length == 0 || indicator == null) return;

        RotateTowardsTarget();
        MoveTowardsTarget();

        // Check if the target is reached
        if (Vector3.Distance(transform.position, targets[currentTargetIndex].position) < arrivalThreshold)
        {
            currentTargetIndex++;
            if (currentTargetIndex >= targets.Length)
            {
                currentTargetIndex = 0; // Loop back if all targets are visited
            }
        }
    }

    void RotateTowardsTarget()
    {
        Vector3 direction = (targets[currentTargetIndex].position - indicator.transform.position).normalized;

        // If close to the target, set the indicator to point downwards
        if (Vector3.Distance(indicator.transform.position, targets[currentTargetIndex].position) < arrivalThreshold)
        {
            indicator.transform.rotation = Quaternion.Euler(90f, 0f, 0f); // Downward rotation
        }
        else
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            indicator.transform.rotation = Quaternion.Slerp(indicator.transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void MoveTowardsTarget()
    {
        Vector3 direction = (targets[currentTargetIndex].position - transform.position).normalized;
        transform.position += direction * movementSpeed * Time.deltaTime;
    }
}
