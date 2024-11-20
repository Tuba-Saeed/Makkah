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
    [Range(0f, 60f)] public float indicatorHeightOffset = 5f; // Increase range to 20 for more flexibility

    private int currentBeaconIndex = 0;     // Track which beacon the player is heading to
    private bool azanPlayed = false;        // Ensure Azan is played only once
    private bool reachedBeacon = false;     // Check if the beacon is reached

    void Start()
    {
        currentBeaconIndex = 0;  // Start by targeting the first beacon (Wazu)
        ShowMuzdalifahPanel();   // Show the Muzdalifah panel at the start
        indicator.SetActive(false); // Initially hide the indicator
    }

    void ShowMuzdalifahPanel()
    {
        muzdalifahPanel.SetActive(true);
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

        yield return new WaitForSeconds(22);  // Wait for 22 seconds
        azanAudioSource.Stop();
    }

    void MoveIndicatorToBeacon()
    {
        if (playerHead == null || beacons.Length == 0)
        {
            Debug.LogWarning("Player head transform or beacons array is not assigned.");
            return;
        }

        // Calculate the new position for the indicator above the player's head
        Vector3 abovePlayer = playerHead.position + Vector3.up * indicatorHeightOffset;
        indicator.transform.position = abovePlayer;

        // Ensure the camera can see the indicator (set the Z value appropriately)
        Vector3 directionToBeacon = (beacons[currentBeaconIndex].position - abovePlayer).normalized;
        if (directionToBeacon != Vector3.zero)
        {
            indicator.transform.rotation = Quaternion.LookRotation(directionToBeacon);
        }

        // Ensure indicator is in a visible layer if it's a 3D object
        if (indicator.GetComponent<Renderer>() != null)
        {
            indicator.GetComponent<Renderer>().enabled = true; // Ensure it is rendered
        }
    }

    void Update()
    {
        MoveIndicatorToBeacon();  // Always update the indicator position above the player's head

        if (!reachedBeacon && Vector3.Distance(player.position, beacons[currentBeaconIndex].position) < 1.5f)
        {
            reachedBeacon = true;
            OnReachBeacon();  // Player reached the beacon
        }
    }

    public void OnWazuPanelClosed()
    {
        wazuIntentionPanel.SetActive(false);  // Hide Wazu panel
        duaAudioSource.Stop();  // Stop dua audio

        currentBeaconIndex++; // Move to the next beacon for Namaz
        reachedBeacon = false; // Reset for the next beacon

        MoveIndicatorToBeacon(); // Immediately move indicator to Namaz beacon after Wazu
        indicator.SetActive(true); // Show the indicator again for Namaz
    }

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
    }

    void ShowWazuIntentionPanel()
    {
        wazuIntentionPanel.SetActive(true);  // Show Wazu panel
        duaAudioSource.Play();  // Play dua audio
    }

    void ShowNamazPanel()
    {
        namazPanel.SetActive(true);
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
        SceneManager.LoadScene("Select_Location");  // Load the level menu scene
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

    // Method for Close button in final panel to exit the game
    public void OnFinalPanelCloseButton()
    {
        Debug.Log("Closing the application.");
        Application.Quit();  // This will exit the game in a build
    }
}
