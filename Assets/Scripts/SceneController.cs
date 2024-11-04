using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController : MonoBehaviour
{
    public GameObject muzdalifahPanel;      // Panel for Muzdalifah
    public GameObject wazuIntentionPanel;   // Panel for Wazu Intention
    public GameObject namazPanel;           // Panel for Namaz instructions
    public GameObject finalPanel;           // Final panel for pause or next scene
    public AudioSource azanAudioSource;     // Azan audio source
    public AudioSource duaAudioSource;      // Dua audio source
    public Transform[] beacons;             // Array of beacon positions (Wazu, Namaz)
    public GameObject indicator;            // Indicator that points to the beacon
    public Transform player;                // Reference to the player GameObject
    public Transform playerHead;            // Reference to the player's head transform
    public float indicatorHeightOffset = 2f; // Height above player's head

    private int currentBeaconIndex = 0;     // Track which beacon the player is heading to
    private bool azanPlayed = false;        // Ensure Azan is played only once
    private bool reachedBeacon = false;     // Check if the beacon is reached

    void Start()
    {
        currentBeaconIndex = 0;  // Start by targeting the first beacon (Wazu)
        ShowMuzdalifahPanel();   // Show the Muzdalifah panel at the start
        indicator.SetActive(false); // Initially hide the indicator
        Debug.Log("Indicator initially hidden.");
    }

    void ShowMuzdalifahPanel()
    {
        muzdalifahPanel.SetActive(true);
        Debug.Log("Muzdalifah panel is now active.");
    }

    public void OnMuzdalifahPanelClosed()
    {
        muzdalifahPanel.SetActive(false);   // Close the panel
        StartCoroutine(PlayAzanFor22Seconds()); // Start Azan for 22 seconds
    }

    IEnumerator PlayAzanFor22Seconds()
    {
        azanAudioSource.Play();
        indicator.SetActive(true); // Show the indicator as soon as Azan starts
        MoveIndicatorToBeacon(); // Move the indicator to the first beacon (Wazu)
        Debug.Log("Azan started, and indicator is shown pointing to the first beacon.");

        yield return new WaitForSeconds(22);  // Wait for 22 seconds
        azanAudioSource.Stop();
        Debug.Log("Azan stopped after 22 seconds.");
        // After the Azan stops, the indicator should remain active
    }

    void MoveIndicatorToBeacon()
    {
        if (playerHead == null || beacons.Length == 0)
        {
            Debug.LogWarning("Player head transform or beacons array is not assigned.");
            return;
        }

        // Get position above the player's head
        Vector3 abovePlayer = playerHead.position + Vector3.up * indicatorHeightOffset;

        // Move indicator to player's position above the head first
        indicator.transform.position = abovePlayer;

        // Determine direction to the current beacon (either Wazu or Namaz based on index)
        Vector3 directionToBeacon = (beacons[currentBeaconIndex].position - abovePlayer).normalized;

        if (directionToBeacon != Vector3.zero)
        {
            indicator.transform.rotation = Quaternion.LookRotation(directionToBeacon);
            Debug.Log($"Indicator pointed to beacon {currentBeaconIndex} at position {beacons[currentBeaconIndex].position}");
        }
    }

    void Update()
    {
        // Always update the indicator position
        MoveIndicatorToBeacon();

        // If the Azan is playing, update the indicator position
        if (azanAudioSource.isPlaying)
        {
            // This can be commented out if you want to always point to the current beacon
            // MoveIndicatorToBeacon(); // Uncomment if you want the indicator to update even when azan is playing
        }

        // If the player hasn't reached the current beacon yet
        if (!reachedBeacon && Vector3.Distance(player.position, beacons[currentBeaconIndex].position) < 1.5f)
        {
            reachedBeacon = true;
            OnReachBeacon();  // Player reached the beacon
        }

        // Optional: Debug log to see player and beacon positions
        Debug.Log($"Player Position: {player.position}, Beacon Position: {beacons[currentBeaconIndex].position}, Current Beacon Index: {currentBeaconIndex}");
    }

    public void OnWazuPanelClosed()
    {
        wazuIntentionPanel.SetActive(false);  // Hide Wazu panel
        duaAudioSource.Stop();  // Stop dua audio

        // Move to the next beacon for Namaz (now set index to 1 for Namaz)
        currentBeaconIndex++;
        reachedBeacon = false;  // Reset for the next beacon

        // Immediately move indicator to Namaz beacon after Wazu
        MoveIndicatorToBeacon();
        indicator.SetActive(true);  // Show the indicator again for Namaz
        Debug.Log("Wazu task completed. Moving indicator to Namaz beacon. Current Beacon Index: " + currentBeaconIndex);
    }

    // Other methods remain unchanged...

    void OnReachBeacon()
    {
        indicator.SetActive(false);  // Hide the indicator temporarily

        if (currentBeaconIndex == 0)  // First beacon is for Wazu
        {
            ShowWazuIntentionPanel();  // Show the Wazu instruction panel
        }
        else if (currentBeaconIndex == 1)  // Second beacon is for Namaz
        {
            ShowNamazPanel();  // Show Namaz instructions
        }
        // Add more checks if you have additional beacons
    }


    void ShowWazuIntentionPanel()
    {
        wazuIntentionPanel.SetActive(true);  // Show Wazu panel
        duaAudioSource.Play();  // Play dua audio
        Debug.Log("Wazu intention panel is shown and dua audio is playing.");
    }

    void ShowNamazPanel()
    {
        namazPanel.SetActive(true);
        Debug.Log("Namaz panel is now shown.");
    }

    public void OnNamazPanelClosed()
    {
        namazPanel.SetActive(false);  // Hide Namaz panel
        ShowFinalPanel();  // Display final panel
    }

    void ShowFinalPanel()
    {
        finalPanel.SetActive(true);  // Show final panel
        indicator.SetActive(false);  // Hide the indicator
    }

    // Method for Home button to go to level menu
    public void OnFinalPanelHomeButton()
    {
        SceneManager.LoadScene("Select_Location");  // Load the level menu scene (replace with actual scene name)
    }

    // Method for Restart button to reload the current scene
    public void OnFinalPanelRestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Reload the current scene
    }

    // Method for Cancel button to hide the final panel
    public void OnFinalPanelCancelButton()
    {
        finalPanel.SetActive(false);  // Hide final panel
    }
}