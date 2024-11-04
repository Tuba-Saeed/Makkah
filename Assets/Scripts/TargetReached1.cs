using UnityEngine;

public class TargetReached : MonoBehaviour
{
    public GameObject wazuMessage;  // Assign the Wazu message UI in Inspector
    public GameObject indicator;     // Assign the indicator in Inspector
    public GameObject beacon;        // Assign the beacon in Inspector

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Assuming your player has the "Player" tag
        {
            // Show the Wazu message
            wazuMessage.SetActive(true);

            // Hide the indicator and beacon
            if (indicator != null)
            {
                indicator.SetActive(false);
            }
            if (beacon != null)
            {
                beacon.SetActive(false);
            }
        }
    }
}
