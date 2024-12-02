using UnityEngine;
using UnityEngine.UI;

public class BeaconRoundManager : MonoBehaviour
{
    public Transform player; // Player object
    public Transform blackBox; // Black box at the center
    public GameObject beacon; // Beacon (sky blue circle)
    public GameObject finalPanel; // Panel to show when Tawaf is completed
    public Text roundCountText; // UI Text for round count
    public float detectionRadius = 5f; // Beacon area radius

    private int roundsCompleted = 0; // Rounds completed
    private bool isPlayerInBeacon = false; // Whether the player is within the beacon area
    private float lastAngle = 0f; // Last recorded angle of the player
    private float totalAngleTravelled = 0f; // Total angle traveled by the player

    void Update()
    {
        // Check if the player is within the beacon area
        isPlayerInBeacon = Vector3.Distance(player.position, blackBox.position) <= detectionRadius;

        if (isPlayerInBeacon)
        {
            TrackPlayerMovement();
        }
    }

    private void TrackPlayerMovement()
    {
        // Calculate the angle of the player relative to the black box
        Vector3 directionToPlayer = player.position - blackBox.position;
        float currentAngle = Mathf.Atan2(directionToPlayer.z, directionToPlayer.x) * Mathf.Rad2Deg;

        // Detect movement and track the angular difference
        if (Vector3.Distance(player.position, blackBox.position) > 0.1f)  // Ensure movement is significant
        {
            float angleDifference = Mathf.DeltaAngle(lastAngle, currentAngle);

            // Accumulate the total angle traveled
            totalAngleTravelled += Mathf.Abs(angleDifference);

            // Check if a full round (360°) has been completed
            if (totalAngleTravelled >= 360f)
            {
                roundsCompleted++;
                totalAngleTravelled = 0f; // Reset the total angle for the next round

                UpdateRoundText();

                // Check if 7 rounds are completed
                if (roundsCompleted >= 7)
                {
                    CompleteTawaf();
                }
            }

            // Update the last recorded angle
            lastAngle = currentAngle;
        }
    }

    private void UpdateRoundText()
    {
        if (roundCountText != null)
        {
            roundCountText.text = "Rounds Completed: " + roundsCompleted;
        }
    }

    private void CompleteTawaf()
    {
        // Hide the beacon after 7 rounds
        if (beacon != null)
        {
            beacon.SetActive(false); // Hide the beacon
        }

        // Show the final panel when Tawaf is complete
        if (finalPanel != null)
        {
            finalPanel.SetActive(true); // Show the final "Tawaf Completed" panel
        }
    }
}