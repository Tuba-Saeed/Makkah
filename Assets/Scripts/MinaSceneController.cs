using UnityEngine;
using System.Collections;

public class MinaSceneController : MonoBehaviour
{
    public GameObject minaInfoPanel;      // Mina Info Panel
    public GameObject namazTimePanel;     // Namaz Time Panel
    public GameObject wuduPanel;          // Wudu Info Panel
    public GameObject namazPanel;         // Namaz Info Panel
    public GameObject indicator;          // Indicator (above the player's head)
    public GameObject wuduTarget;         // Wudu Target
    public GameObject namazTarget;        // Namaz Target
    public GameObject beaconWudu;         // Beacon for Wudu target
    public GameObject beaconNamaz;        // Beacon for Namaz target
    public Transform playerModel;         // Player model to place the indicator above
    public float moveSpeed = 5f;          // Character move speed

    private bool isMoving = false;        // Is character moving?
    private GameObject currentTarget;     // Current target for character to move to

    void Start()
    {
        StartCoroutine(StartSequence());
    }

    void Update()
    {
        // Update the indicator position to stay above the player's head
        if (indicator.activeInHierarchy && playerModel != null)
        {
            Vector3 offset = new Vector3(0, 2f, 0); // Adjust offset to control how high the indicator is above the player
            indicator.transform.position = playerModel.position + offset;
        }

        // Move towards the target if isMoving is true
        if (isMoving && currentTarget != null)
        {
            MoveTowardsTarget();
        }
    }

    IEnumerator StartSequence()
    {
        // Step 1: Show Mina Info Panel
        minaInfoPanel.SetActive(true);

        // Wait for Mina Info Panel to close
        yield return new WaitUntil(() => !minaInfoPanel.activeInHierarchy);

        // Step 2: Show Namaz Time Panel
        namazTimePanel.SetActive(true);

        // Wait for Namaz Time Panel to close
        yield return new WaitUntil(() => !namazTimePanel.activeInHierarchy);

        // Step 3: Show Wudu Indicator and Move to Wudu Target
        indicator.SetActive(true);
        beaconWudu.SetActive(true); // Show beacon for Wudu
        currentTarget = wuduTarget; // Set current target to Wudu
        isMoving = true; // Start moving

        // Wait for player to reach Wudu target
        yield return new WaitUntil(() => !isMoving);

        // Step 4: Show Wudu Panel and close after 5 seconds
        ShowWuduPanel();

        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);

        // Close Wudu Panel
        CloseWuduPanel();

        // Step 5: Move to Namaz Target
        indicator.SetActive(true); // Show indicator again
        beaconNamaz.SetActive(true); // Show Namaz beacon
        currentTarget = namazTarget; // Set target to Namaz
        isMoving = true; // Start moving again

        // Wait for player to reach Namaz target
        yield return new WaitUntil(() => !isMoving);

        // Step 6: Show Namaz Panel
        namazPanel.SetActive(true);

        // Wait for Namaz Panel to close
        yield return new WaitUntil(() => !namazPanel.activeInHierarchy);

        // Hide the Namaz Beacon
        beaconNamaz.SetActive(false);

        // You can continue adding more steps for other Namaz or other sequences as needed
    }

    void MoveTowardsTarget()
    {
        // Move character towards the current target position
        float step = moveSpeed * Time.deltaTime;
        playerModel.position = Vector3.MoveTowards(playerModel.position, currentTarget.transform.position, step);

        // Check if the player has reached the target
        if (Vector3.Distance(playerModel.position, currentTarget.transform.position) < 0.1f)
        {
            isMoving = false; // Stop moving
            indicator.SetActive(false); // Hide the indicator when target is reached
        }
    }

    void ShowWuduPanel()
    {
        Debug.Log("Showing Wudu Panel");
        // Hide the first beacon, then show the Wudu panel
        beaconWudu.SetActive(false);
        wuduPanel.SetActive(true);

        // Start coroutine to automatically close the Wudu panel after 5 seconds
        StartCoroutine(CloseWuduPanelAfterDelay());
    }

    IEnumerator CloseWuduPanelAfterDelay()
    {
        Debug.Log("Closing Wudu Panel after 5 seconds");
        yield return new WaitForSeconds(5f); // Wait for 5 seconds
        CloseWuduPanel(); // Close the Wudu panel
    }

    public void CloseWuduPanel()
    {
        Debug.Log("Close Wudu Panel called"); // Debug log
        wuduPanel.SetActive(false); // Make sure the Wudu panel is deactivated
        beaconNamaz.SetActive(true); // Show Namaz beacon
        currentTarget = namazTarget;
        isMoving = true; // Start moving again
    }

    public void CloseMinaInfoPanel()
    {
        Debug.Log("Close Mina Info Panel called"); // Debug log
        minaInfoPanel.SetActive(false); // Close Mina info panel
    }
}
