using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel; // Reference to the dialogue panel
    public float displayDuration = 10f; // Duration to display the dialogue box

    void Start()
    {
        // Ensure the dialogue panel is initially hidden
        if (dialoguePanel == null)
        {
            Debug.LogError("Dialogue panel is not assigned in the Inspector.");
            return;
        }

        dialoguePanel.SetActive(false);

        // Show the dialogue panel with information about Muzdalifah
        ShowDialogue();

        // Hide the dialogue panel after a specified duration
        Invoke("HideDialogue", displayDuration);
    }

    public void ShowDialogue()
    {
        if (dialoguePanel == null)
        {
            Debug.LogError("Dialogue panel is not assigned.");
            return;
        }

        dialoguePanel.SetActive(true); // Show the dialogue panel
    }

    public void HideDialogue()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false); // Hide the dialogue panel
        }
    }

    public void CloseDialogue()
    {
        HideDialogue();
    }
}
