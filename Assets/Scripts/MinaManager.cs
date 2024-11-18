using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SequenceManager : MonoBehaviour
{
    public GameObject minaPanel;
    public GameObject namazTimingPanel;
    public GameObject wazuPanel;
    public GameObject namazPanel;
    public GameObject finalPanel; // The final panel to be displayed after Namaz

    public Button minaCloseButton;
    public Button namazTimingCloseButton;
    public Button wazuCloseButton;
    public Button namazCloseButton;
    public Button restartButton; // Restart button in the final panel
    public Button homeButton;    // Home button in the final panel
    public Button closeButton;   // Close button in the final panel

    public AudioSource azanAudioSource;
    public Transform[] beacons;
    public GameObject indicator;
    public Transform player;
    public float indicatorHeightOffset = 2f;

    private int currentBeaconIndex = 0;
    private bool reachedBeacon = false;

    private bool minaClosed = false;
    private bool namazTimingClosed = false;

    void Start()
    {
        // Initialize sequence
        StartCoroutine(StartSequence());

        // Assign close button events
        minaCloseButton.onClick.AddListener(() => minaClosed = true);
        namazTimingCloseButton.onClick.AddListener(() => namazTimingClosed = true);
        wazuCloseButton.onClick.AddListener(OnWazuClosed);
        namazCloseButton.onClick.AddListener(OnNamazClosed);

        // Assign final panel buttons
        restartButton.onClick.AddListener(RestartSequence);
        homeButton.onClick.AddListener(GoToHome);
        closeButton.onClick.AddListener(CloseScene);
    }

    IEnumerator StartSequence()
    {
        Debug.Log("Starting sequence...");

        // Step 1: Show Mina Panel and wait for it to close
        minaPanel.SetActive(true);
        Debug.Log("Mina panel active, waiting to close...");
        yield return new WaitUntil(() => minaClosed);

        // Step 2: Show Namaz Timing Panel and wait for it to close
        namazTimingPanel.SetActive(true);
        Debug.Log("Namaz Timing panel active, waiting to close...");
        yield return new WaitUntil(() => namazTimingClosed);

        // Step 3: Activate indicator and start azan sound
        indicator.SetActive(true);
        azanAudioSource.Play();
        MoveIndicatorToBeacon();
        Debug.Log("Indicator and azan activated.");

        // Start coroutine to stop azan audio after 22 seconds
        StartCoroutine(StopAzanAfterDelay(22f));
    }

    IEnumerator StopAzanAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        azanAudioSource.Stop();
        Debug.Log("Azan audio stopped after " + delay + " seconds.");
    }

    void Update()
    {
        MoveIndicatorToBeacon();

        if (!reachedBeacon && Vector3.Distance(player.position, beacons[currentBeaconIndex].position) < 1.5f)
        {
            reachedBeacon = true;
            OnReachBeacon();
        }
    }

    void MoveIndicatorToBeacon()
    {
        if (beacons.Length == 0 || player == null || currentBeaconIndex >= beacons.Length)
            return;

        Vector3 abovePlayerPosition = player.position + Vector3.up * indicatorHeightOffset;
        Vector3 directionToBeacon = (beacons[currentBeaconIndex].position - abovePlayerPosition).normalized;

        indicator.transform.position = abovePlayerPosition;
        if (directionToBeacon != Vector3.zero)
        {
            indicator.transform.rotation = Quaternion.LookRotation(directionToBeacon);
        }
    }

    void OnReachBeacon()
    {
        indicator.SetActive(false);

        if (currentBeaconIndex == 0)
        {
            Debug.Log("Reached first beacon, showing Wazu panel.");
            wazuPanel.SetActive(true);
        }
        else if (currentBeaconIndex == 1)
        {
            Debug.Log("Reached second beacon, showing Namaz panel.");
            namazPanel.SetActive(true);
        }
    }

    void OnWazuClosed()
    {
        wazuPanel.SetActive(false);

        // Move to the next beacon and re-enable indicator
        currentBeaconIndex++;
        reachedBeacon = false;
        if (currentBeaconIndex < beacons.Length)
        {
            indicator.SetActive(true);
            MoveIndicatorToBeacon();
        }
    }

    void OnNamazClosed()
    {
        namazPanel.SetActive(false);

        // Show the final panel after Namaz panel is closed
        Debug.Log("Sequence completed.");
        finalPanel.SetActive(true);
    }

    // Final Panel button actions
    void RestartSequence()
    {
        Debug.Log("Restarting sequence...");
        // Reset sequence by hiding the final panel and restarting the flow
        finalPanel.SetActive(false);
        minaClosed = false;
        namazTimingClosed = false;
        StartCoroutine(StartSequence());
    }

    void GoToHome()
    {
        Debug.Log("Returning to home...");
        // Logic for going to home (e.g., load a home scene or reset the game)
        // SceneManager.LoadScene("HomeScene");
    }

    void CloseScene()
    {
        Debug.Log("Closing scene...");
        // Logic for closing the scene or application
        Application.Quit();
    }
}
