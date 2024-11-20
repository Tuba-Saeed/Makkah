using UnityEngine;
using UnityEngine.SceneManagement;


public class ArafatController : MonoBehaviour
{
    public GameObject arafatInfoPanel; // Panel for Arafat information
    public GameObject choicePanel;     // Panel for choices (prayer, dua, recite Quran)
    public GameObject finalPanel;      // Final panel with home, close, and restart buttons
    public GameObject indicator;       // Indicator that moves towards the beacon
    public GameObject beacon;          // Target beacon for the model to reach
    public GameObject model;           // The model that will move towards the beacon
    public Vector3 indicatorOffset = new Vector3(0, 2f, 0);  // Offset for indicator position above the model

    private bool isMovingToBeacon = false; // Flag to control model movement to the beacon

    void Start()
    {
        // Show only the Arafat info panel at the start
        ShowPanel(arafatInfoPanel);
        choicePanel.SetActive(false);
        finalPanel.SetActive(false);
        indicator.SetActive(false);
        beacon.SetActive(false);
    }

    public void OnCloseArafatInfoPanel()
    {
        // Close Arafat info panel, show the indicator, and start moving the model towards the beacon
        arafatInfoPanel.SetActive(false);
        indicator.SetActive(true);
        beacon.SetActive(true);
        isMovingToBeacon = true;
    }

    private void Update()
    {
        if (isMovingToBeacon)
        {
            // Position the indicator above the model's head
            indicator.transform.position = model.transform.position + indicatorOffset;

            // Move the model towards the beacon position
            model.transform.position = Vector3.MoveTowards(model.transform.position, beacon.transform.position, Time.deltaTime * 2f);

            // Check if the model has reached the beacon
            if (Vector3.Distance(model.transform.position, beacon.transform.position) < 0.5f)
            {
                // Stop the model's movement and hide the indicator and beacon
                isMovingToBeacon = false;
                indicator.SetActive(false);
                beacon.SetActive(false);

                // Show the choice panel
                ShowPanel(choicePanel);
            }
        }
    }

    public void OnCloseChoicePanel()
    {
        // Close choice panel and show the final panel
        choicePanel.SetActive(false);
        ShowPanel(finalPanel);
    }

    private void ShowPanel(GameObject panel)
    {
        // Display the specified panel
        panel.SetActive(true);
    }

    private void HidePanel(GameObject panel)
    {
        // Hide the specified panel
        panel.SetActive(false);
    }

    public void OnFinalPanelHomeButton()
    {
        SceneManager.LoadScene("Select_Location");  // Load the level menu scene (replace with actual scene name)
    }

    public void OnFinalPanelRestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Reload the current scene
    }

    public void OnFinalPanelCancelButton()
    {
        Application.Quit();
    }
}
