using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TawafManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject kabainfoPanel;
    public GameObject completionPanel;
    public GameObject finalPanel;
    public Text roundText;

    [Header("Beacon and Character")]
    public GameObject beacon;
    public GameObject character;

    [Header("Indicator")]
    public GameObject indicator;
    public Vector3 indicatorOffset = new Vector3(0, 2f, 0);

    [Header("Settings")]
    public float beaconRadius = 2.0f;
    public int totalRounds = 7;

    private int currentRound = 0;
    private bool isMoving = false;
    private bool tawafCompleted = false;
    private bool wasInsideBeacon = false;

    void Start()
    {
        // Initialize UI and objects
        kabainfoPanel.SetActive(true);
        completionPanel.SetActive(false);
        finalPanel.SetActive(false);

        if (indicator != null)
        {
            indicator.SetActive(false);
        }
        else
        {
            Debug.LogError("Indicator is not assigned in the Inspector.");
        }

        if (roundText != null)
        {
            roundText.text = "Rounds: 0 / " + totalRounds;
        }
        else
        {
            Debug.LogError("RoundText is not assigned in the Inspector.");
        }
    }

    public void CloseInfoPanel()
    {
        kabainfoPanel.SetActive(false);
        isMoving = true;

        if (indicator != null)
        {
            indicator.SetActive(true);
            Debug.Log("Indicator activated.");
        }
        else
        {
            Debug.LogError("Indicator is not assigned.");
        }
    }

    void Update()
    {
        if (isMoving && !tawafCompleted)
        {
            if (character != null && beacon != null)
            {
                CheckBeaconProximity();
                UpdateIndicatorPosition();
            }
            else
            {
                Debug.LogError("Character or Beacon is not assigned.");
            }
        }
    }

    void CheckBeaconProximity()
    {
        float distance = Vector3.Distance(character.transform.position, beacon.transform.position);
        Debug.Log("Character-Beacon Distance: " + distance);

        if (distance <= beaconRadius)
        {
            if (!wasInsideBeacon)
            {
                wasInsideBeacon = true;
                Debug.Log("Character entered beacon area.");
            }
        }
        else
        {
            if (wasInsideBeacon)
            {
                wasInsideBeacon = false;
                currentRound++;
                Debug.Log("Completed Round: " + currentRound);
                UpdateRoundText();

                if (currentRound >= totalRounds)
                {
                    TawafComplete();
                }
            }
        }
    }

    void UpdateIndicatorPosition()
    {
        if (indicator != null && character != null)
        {
            indicator.transform.position = character.transform.position + indicatorOffset;
        }
    }

    void UpdateRoundText()
    {
        if (roundText != null)
        {
            roundText.text = "Rounds: " + currentRound + " / " + totalRounds;
            Debug.Log("Round text updated: " + roundText.text);
        }
    }

    void TawafComplete()
    {
        Debug.Log("Tawaf Completed!");
        tawafCompleted = true;
        isMoving = false;

        if (indicator != null) indicator.SetActive(false);
        if (completionPanel != null) completionPanel.SetActive(false); // Ensure it's hidden
        if (finalPanel != null) finalPanel.SetActive(true);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoHome()
    {
        SceneManager.LoadScene("HomeScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}