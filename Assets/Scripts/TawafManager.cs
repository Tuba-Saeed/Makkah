using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class TawafManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject tawafInstructionPanel;
    public GameObject completionPanel;
    public GameObject finalOptionsPanel;
    public float beaconRadius = 2.0f;

    [Header("Objects")]
    public GameObject indicator;
    public GameObject beacon;
    public GameObject character;

    [Header("Settings")]
    public Vector3 indicatorOffset = new Vector3(0, 2f, 0); // Offset for the indicator
    public int totalRounds = 7; // Total number of rounds for Tawaf

    private int completedRounds = 0; // Number of completed rounds
    private bool isMoving = false;  // Is the character moving?
    private HashSet<Vector3> checkpoints = new HashSet<Vector3>(); // To store unique beacon entries

    void Start()
    {
        // Initialize panels and objects
        tawafInstructionPanel.SetActive(true);
        completionPanel.SetActive(false);
        finalOptionsPanel.SetActive(false);

        if (indicator != null)
            indicator.SetActive(false); // Hide the indicator initially

        if (beacon != null)
            beacon.SetActive(true); // Show the beacon at the start

        Debug.Log("TawafManager Initialized. Waiting to start Tawaf.");
    }

    public void StartTawaf()
    {
        // Start the Tawaf process
        tawafInstructionPanel.SetActive(false);
        isMoving = true;

        if (indicator != null)
            indicator.SetActive(true); // Show the indicator

        Debug.Log("Tawaf Started. Move your character to begin rounds.");
    }

    void Update()
    {
        if (isMoving)
        {
            // Check if the character is inside the beacon
            if (beacon != null && IsCharacterInsideBeacon())
            {
                completedRounds++;

                // Log the number of completed rounds to the console
                Debug.Log("Completed Round: " + completedRounds);

                // Reset character's position to start the next round
                character.transform.position = beacon.transform.position;

                // Check if Tawaf is complete
                if (completedRounds >= totalRounds)
                {
                    Debug.Log("Tawaf Complete!");
                    isMoving = false;

                    if (beacon != null)
                        beacon.SetActive(false); // Hide the beacon

                    if (indicator != null)
                        indicator.SetActive(false); // Hide the indicator

                    completionPanel.SetActive(true); // Show the completion panel
                }
            }
        }

        // Position the indicator above the character's head
        if (indicator != null && character != null)
        {
            Vector3 headPosition = character.transform.position + indicatorOffset;
            indicator.transform.position = headPosition;

            // Ensure the indicator is active
            if (!indicator.activeSelf)
            {
                indicator.SetActive(true);
            }
        }
    }

    bool IsCharacterInsideBeacon()
    {
        // Check if the character is within the beacon's radius
        if (beacon != null && character != null)
        {
            float distance = Vector3.Distance(character.transform.position, beacon.transform.position);
            Debug.Log("Distance to Beacon: " + distance);
            return distance <= beaconRadius;
        }
        return false;
    }

    public void CloseCompletionPanel()
    {
        // Close the completion panel and show the final options panel
        completionPanel.SetActive(false);
        finalOptionsPanel.SetActive(true);
    }

    // Button functions for final options
    public void GoHome()
    {
        SceneManager.LoadScene("HomeScene");
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CloseScene()
    {
        Application.Quit();
    }
}
