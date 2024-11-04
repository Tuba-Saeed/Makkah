using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GenderSelection : MonoBehaviour
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

    public void ShowMaleModels()
    {
        showingMaleModels = true;
        maleModels.SetActive(true);
        femaleModels.SetActive(false);
        UpdateCharacterName("Male");
    }

    public void ShowFemaleModels()
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
            CharacterName.text = $" {name}";
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
