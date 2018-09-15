using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsTitleScript : MonoBehaviour {
    public GameObject helpScreen;
    public Slider musicVolumeSlider;
    public Slider seVolumeSlider;
    public Slider touchSensitivitySlider;
    public Slider brightnessSlider;
    public PlayerInputTouch touchInput;
    public Light sun;

    float musicVol;
    float seVol;
    float sensitivity;
    float brightness;

    void Update() {
        if (musicVolumeSlider.value != musicVol) {
            PlayerPrefs.SetFloat("musicvol", musicVolumeSlider.value);
            musicVol = musicVolumeSlider.value;
            VolumeHolderScript.instance.musicVol = musicVol;
            PlayerPrefs.Save();
        }
        if (seVolumeSlider.value != seVol) {
            PlayerPrefs.SetFloat("sevol", seVolumeSlider.value);
            seVol = seVolumeSlider.value;
            PlayerPrefs.Save();
        }
        if (touchSensitivitySlider.value != sensitivity) {
            PlayerPrefs.SetFloat("sensitivity", touchSensitivitySlider.value);
            sensitivity = touchSensitivitySlider.value;
            if (touchInput != null) {
                touchInput.rotateUserSensitivity = (sensitivity * 1.8f) + 0.2f;
            }
            PlayerPrefs.Save();
        }
        if (brightnessSlider.value != brightness)
        {
            PlayerPrefs.SetFloat("brightness", brightnessSlider.value);
            brightness = brightnessSlider.value;
            sun.intensity = brightness;
            PlayerPrefs.Save();
        }
    }

    private void ShowSettings() {
        musicVol = PlayerPrefs.GetFloat("musicvol", 1f);
        seVol = PlayerPrefs.GetFloat("sevol", 1f);
        sensitivity = PlayerPrefs.GetFloat("sensitivity", 0.5f);
        brightness = PlayerPrefs.GetFloat("brightness", 0.5f);
        musicVolumeSlider.value = musicVol;
        seVolumeSlider.value = seVol;
        touchSensitivitySlider.value = sensitivity;
        brightnessSlider.value = brightness;
        gameObject.SetActive(true);
    }

    private void HideSettings() {
        gameObject.SetActive(false);
    }

    public void PauseAndShowSettings() {
        Time.timeScale = 0f;
        ShowSettings();
    }

    public void HideSettingsAndResume() {
        if (helpScreen.activeSelf) helpScreen.SetActive(false);
        HideSettings();
        Time.timeScale = 1f;
    }

    public void ShowHelpScreen() {
        helpScreen.SetActive(true);
    }
}
