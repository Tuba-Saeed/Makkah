using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class QurbaniSceneController : MonoBehaviour
{
    public GameObject qurbaniInfoPanel;
    public GameObject qurbaniDistributionPanel;
    public GameObject finalPanel;
    public GameObject indicator;
    public Transform targetPoint;
    public GameObject model;
    public Vector3 indicatorOffset = new Vector3(0, 2f, 0);  // Adjust the Y value for indicator height

    private bool isMovingToTarget = false;
    private bool hasReachedTarget = false; // Added to track if the model has already reached the target

    void Start()
    {
        // Show Qurbani info panel first and hide others
        qurbaniInfoPanel.SetActive(true);
        qurbaniDistributionPanel.SetActive(false);
        finalPanel.SetActive(false);
        indicator.SetActive(false);  // Initially hide the indicator

        // Start the process of showing the indicator after a short delay
        StartCoroutine(ShowIndicatorAfterDelay());
    }

    private IEnumerator ShowIndicatorAfterDelay()
    {
        yield return new WaitForSeconds(0.1f); // Wait for a short time before showing the indicator
        indicator.SetActive(true);
        Debug.Log("Indicator should now be visible: " + indicator.activeSelf);

        // Start moving the indicator and model towards the target beacon
        isMovingToTarget = true;
    }

    public void CloseQurbaniInfoPanel()
    {
        // Hide the Qurbani Info Panel and show the indicator above the model
        qurbaniInfoPanel.SetActive(false);
        indicator.SetActive(true);
        Debug.Log("Indicator activated after Qurbani Info Panel close: " + indicator.activeSelf);

        // Start moving the indicator and model towards the target
        isMovingToTarget = true;
    }

    private void Update()
    {
        if (isMovingToTarget)
        {
            // Position the indicator above the model's head
            indicator.transform.position = model.transform.position + indicatorOffset;
            Debug.Log("Indicator position: " + indicator.transform.position);

            // Move the model towards the target point (beacon) without changing its Y-axis position
            Vector3 targetPosition = new Vector3(targetPoint.position.x, model.transform.position.y, targetPoint.position.z);
            model.transform.position = Vector3.MoveTowards(model.transform.position, targetPosition, Time.deltaTime * 2f);

            // Check if the model has reached the target (beacon)
            if (Vector3.Distance(model.transform.position, targetPosition) < 0.5f && !hasReachedTarget)
            {
                // Stop moving the model and hide the indicator
                isMovingToTarget = false;
                indicator.SetActive(false); // Hide the indicator after reaching the target

                // Mark that the model has reached the target
                hasReachedTarget = true;

                // Show the Qurbani Distribution Panel
                qurbaniDistributionPanel.SetActive(true);
            }
        }
    }

    public void CloseQurbaniDistributionPanel()
    {
        Debug.Log("Closing Qurbani Distribution Panel and opening Final Panel");

        // Hide the Qurbani Distribution Panel and show the Final Panel
        qurbaniDistributionPanel.SetActive(false);
        finalPanel.SetActive(true);
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
