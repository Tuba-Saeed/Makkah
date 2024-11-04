using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    [Header("Volume Setting")]
    [SerializeField] private TMP_Text VolumeTextValue = null;
    [SerializeField] private Slider VolumeSlider = null;
    [SerializeField] private float defaultVolume = 1.0f;
    [SerializeField] private GameObject volumeSettingsPanel = null; // Reference to the volume settings panel

    [Header("Confirmation")]
    [SerializeField] private GameObject confirmationPrompt = null;

    [Header("Levels To Load")]
    public string _newGameLevel;
    private string levelToLoad;

    void Start()
    {
        // Ensure the volume settings panel is initially inactive
        if (volumeSettingsPanel != null)
        {
            volumeSettingsPanel.SetActive(false);
        }

        // Load saved volume value
        if (PlayerPrefs.HasKey("masterVolume"))
        {
            float savedVolume = PlayerPrefs.GetFloat("masterVolume");
            AudioListener.volume = savedVolume;
            VolumeSlider.value = savedVolume;
            VolumeTextValue.text = savedVolume.ToString("0.0");
        }
    }

    public void NewGameDialogYes()
    {
        SceneManager.LoadScene(_newGameLevel);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        VolumeTextValue.text = volume.ToString("0.0");
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        StartCoroutine(ConfirmationBox());
    }

    public void ResetButton(string MenuType)
    {
        if (MenuType == "Audio")
        {
            AudioListener.volume = defaultVolume;
            VolumeSlider.value = defaultVolume;
            VolumeTextValue.text = defaultVolume.ToString("0.0");
            VolumeApply();
        }
    }

    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }

    // Method to show the volume settings panel
    public void ShowVolumeSettings()
    {
        if (volumeSettingsPanel != null)
        {
            volumeSettingsPanel.SetActive(true);
        }
    }

    // Method to hide the volume settings panel
    public void HideVolumeSettings()
    {
        if (volumeSettingsPanel != null)
        {
            volumeSettingsPanel.SetActive(false);
        }
    }
}
