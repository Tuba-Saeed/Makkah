using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SequenceManager : MonoBehaviour
{
    public GameObject minaPanel;
    public GameObject namazTimingPanel;
    public GameObject wazuPanel;
    public GameObject namazPanel;

    public Button minaCloseButton;
    public Button namazTimingCloseButton;
    public Button wazuCloseButton;
    public Button namazCloseButton;

    public AudioSource azanAudioSource;
    public Transform[] beacons;
    public GameObject indicator;
    public Transform player;
    public float indicatorHeightOffset = 2f;

    private int currentBeaconIndex = 0;
    private bool reachedBeacon = false;

    void Start()
    {
        StartCoroutine(StartSequence());

        // Assign close button functionality for each panel
        minaCloseButton.onClick.AddListener(CloseMinaPanel);
        namazTimingCloseButton.onClick.AddListener(CloseNamazTimingPanel);
        wazuCloseButton.onClick.AddListener(CloseWazuPanel);
        namazCloseButton.onClick.AddListener(CloseNamazPanel);
    }

    IEnumerator StartSequence()
    {
        // Step 1: Show Mina Panel, then hide it after 3 seconds
        minaPanel.SetActive(true);
        yield return new WaitForSeconds(3);
        minaPanel.SetActive(false);

        // Step 2: Show Namaz Timing Panel, then hide it after 3 seconds
        namazTimingPanel.SetActive(true);
        yield return new WaitForSeconds(3);
        namazTimingPanel.SetActive(false);

        // Step 3: Activate indicator and start azan sound
        indicator.SetActive(true);
        azanAudioSource.Play();
        MoveIndicatorToBeacon();

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
        // Keep moving the indicator to the current beacon's position
        MoveIndicatorToBeacon();

        // Check if the player has reached the current beacon
        if (!reachedBeacon && Vector3.Distance(player.position, beacons[currentBeaconIndex].position) < 1.5f)
        {
            reachedBeacon = true;
            OnReachBeacon();
        }
    }

    void MoveIndicatorToBeacon()
    {
        if (beacons.Length == 0 || player == null)
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
            StartCoroutine(ShowPanelFor3Seconds(wazuPanel));
        }
        else if (currentBeaconIndex == 1)
        {
            Debug.Log("Reached second beacon, showing Namaz panel.");
            StartCoroutine(ShowPanelFor3Seconds(namazPanel));
        }
    }

    IEnumerator ShowPanelFor3Seconds(GameObject panel)
    {
        panel.SetActive(true);
        Debug.Log(panel.name + " is now active.");
        yield return new WaitForSeconds(3);

        panel.SetActive(false);
        Debug.Log(panel.name + " is now inactive.");

        // Move to the next beacon if available
        beacons[currentBeaconIndex].gameObject.SetActive(false);
        currentBeaconIndex++;
        reachedBeacon = false;

        // Reactivate indicator if there are more beacons to navigate
        if (currentBeaconIndex < beacons.Length)
        {
            indicator.SetActive(true);
            MoveIndicatorToBeacon();
        }
    }

    // Separate close functions for each panel
    void CloseMinaPanel()
    {
        minaPanel.SetActive(false);
    }

    void CloseNamazTimingPanel()
    {
        namazTimingPanel.SetActive(false);
    }

    void CloseWazuPanel()
    {
        wazuPanel.SetActive(false);
    }

    void CloseNamazPanel()
    {
        namazPanel.SetActive(false);
    }
}
