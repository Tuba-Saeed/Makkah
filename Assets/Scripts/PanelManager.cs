using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public GameObject mainPanel;  // Assign the MainPanel in Inspector
    public GameObject finalPanel; // Assign the FinalPanel in Inspector

    // Method to handle cross button click
    public void OnCrossButtonClick()
    {
        mainPanel.SetActive(false); // Hide the main panel
        finalPanel.SetActive(true); // Show the final panel
    }
}
