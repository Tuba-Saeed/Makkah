using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AzanScript : MonoBehaviour
{
    public GameObject minaInfoPanel; // Assign the Mina Info Panel in the Inspector
    public AudioClip azanClip; // Assign the Azan audio clip in the Inspector
    public GameObject target; // Assign your target (Target 1) in the Inspector
    public Button closeButton; // Assign the close button for the panel
    public float moveSpeed = 5f; // Speed of character movement

    private AudioSource audioSource; // To play audio
    private bool isPanelClosed = false; // Track if panel is closed
    private bool isMoving = false;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // Add an AudioSource if not already present
        closeButton.onClick.AddListener(CloseMinaInfoPanel); // Add listener to the close button
        StartCoroutine(StartSequence());
    }

    IEnumerator StartSequence()
    {
        // Show Mina info panel
        minaInfoPanel.SetActive(true);

        // Wait for the close button to be pressed
        yield return new WaitUntil(() => isPanelClosed);

        // Play Azan sound
        audioSource.clip = azanClip;
        audioSource.Play();

        // Wait for 22 seconds while Azan is playing
        yield return new WaitForSeconds(22f);

        // Start moving towards target
        isMoving = true;

        // Optionally, if you want to hide the Azan indicator, do it here
        // HideIndicator();

        // Start moving towards the target
        while (isMoving)
        {
            MoveTowardsTarget();
            yield return null; // Wait for the next frame
        }
    }

    public void CloseMinaInfoPanel()
    {
        minaInfoPanel.SetActive(false); // Hide the Mina info panel
        isPanelClosed = true; // Mark the panel as closed
    }

    void MoveTowardsTarget()
    {
        // Move character towards target position
        float step = moveSpeed * Time.deltaTime; // Calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);

        // Check if we have reached the target
        if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
        {
            isMoving = false; // Stop moving once we reach the target
            // Optionally, you can call a method to indicate arrival
            // OnArrival();
        }
    }
}
