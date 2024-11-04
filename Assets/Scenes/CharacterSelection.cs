using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Add this line to include the UI namespace
using TMPro; // Add this line to include the TextMeshPro namespace

public class NewBehaviourScript : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI CharacterName;
    public GameObject maleModels;
    public GameObject femaleModels;
    public Button nextButton;
    public Button previousButton;

    private bool showingMaleModels = true;
    private List<GameObject> selectedModels = new List<GameObject>();

    void Start()
    {
        nextButton.onClick.AddListener(ShowMaleModels);
        previousButton.onClick.AddListener(ShowFemaleModels);

        ShowMaleModels(); // Initialize by showing male models
    }

    void ShowMaleModels()
    {
        showingMaleModels = true;
        maleModels.SetActive(true);
        femaleModels.SetActive(false);
        UpdateCharacterName("Male");
    }

    void ShowFemaleModels()
    {
        showingMaleModels = false;
        maleModels.SetActive(false);
        femaleModels.SetActive(true);
        UpdateCharacterName("Female");
    }

    void UpdateCharacterName(string name)
    {
        if (CharacterName != null)
        {
            CharacterName.text = $"Selected Model: {name}";
        }
    }

    public void SelectModel(GameObject model)
    {
        if (!selectedModels.Contains(model))
        {
            selectedModels.Add(model);
            // Implement logic to indicate model selection, e.g., highlight the model
        }
        else
        {
            selectedModels.Remove(model);
            // Implement logic to indicate model deselection
        }
    }
}
