using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.SceneManagement;

public class SettingMenuManager : MonoBehaviour
{
    public static bool isVibrate;
    public static float cameraSensitivity;

    public TMP_Dropdown graphicsDropdown;
    public Slider masterVol, musicVol, sfxVol, cameraSensitivitySlider;
    public AudioMixer mainAudioMixer;
    public Toggle vibrateToggle;

    private int defaultGraphicsQuality;
    private float defaultMasterVolume;
    private float defaultMusicVolume;
    private float defaultSFXVolume;
    private float defaultCameraSensitivity;
    private bool defaultVibrate;

    private void Start()
    {
        // Save the default settings
        defaultGraphicsQuality = QualitySettings.GetQualityLevel();
        mainAudioMixer.GetFloat("MasterVolume", out defaultMasterVolume);
        mainAudioMixer.GetFloat("MyExposedParam 1", out defaultMusicVolume);
        mainAudioMixer.GetFloat("MyExposedParam", out defaultSFXVolume);
        defaultVibrate = true;
        defaultCameraSensitivity = 1.0f; // Default camera sensitivity value

        isVibrate = defaultVibrate;
        cameraSensitivity = defaultCameraSensitivity;

        // Optionally, set the UI elements to these defaults
        graphicsDropdown.value = defaultGraphicsQuality;
        masterVol.value = defaultMasterVolume;
        musicVol.value = defaultMusicVolume;
        sfxVol.value = defaultSFXVolume;
        vibrateToggle.isOn = defaultVibrate;
        cameraSensitivitySlider.value = defaultCameraSensitivity;
    }

    public void ChangeGraphicsQuality()
    {
        QualitySettings.SetQualityLevel(graphicsDropdown.value);
    }

    public void ChangeMasterVolume()
    {
        mainAudioMixer.SetFloat("MasterVolume", masterVol.value);
    }

    public void ChangeMusicVolume()
    {
        mainAudioMixer.SetFloat("MyExposedParam 1", musicVol.value);
    }

    public void ChangeSFXVolume()
    {
        mainAudioMixer.SetFloat("MyExposedParam", sfxVol.value);
    }

    public void ChangeVibrate()
    {
        isVibrate = vibrateToggle.isOn;
    }

    public void ChangeCameraSensitivity()
    {
        cameraSensitivity = cameraSensitivitySlider.value;
        // Apply the camera sensitivity to your camera controller here if needed
    }

    public void ResetToDefaults()
    {
        // Reset to default settings
        graphicsDropdown.value = defaultGraphicsQuality;
        masterVol.value = defaultMasterVolume;
        musicVol.value = defaultMusicVolume;
        sfxVol.value = defaultSFXVolume;
        vibrateToggle.isOn = defaultVibrate;
        cameraSensitivitySlider.value = defaultCameraSensitivity;

        ApplySettings();
    }

    public void ApplySettings()
    {
        // Directly apply the settings
        ChangeGraphicsQuality();
        ChangeMasterVolume();
        ChangeMusicVolume();
        ChangeSFXVolume();
        ChangeVibrate();
        ChangeCameraSensitivity();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Replace "MainMenu" with the actual name of your main menu scene
    }

    // Update is called once per frame
    private void Update()
    {

    }
}
