using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsTitleScript : MonoBehaviour {
    public Slider musicVolumeSlider;
    public Slider seVolumeSlider;
    public Slider touchSensitivitySlider;
    public PlayerInputTouch touchInput;

    float musicVol;
    float seVol;
    float sensitivity;

    void Update() {
        if (musicVolumeSlider.value != musicVol) {
            PlayerPrefs.SetFloat("musicvol", musicVolumeSlider.value);
            musicVol = musicVolumeSlider.value;
        }
        if (seVolumeSlider.value != seVol) {
            PlayerPrefs.SetFloat("sevol", seVolumeSlider.value);
            seVol = seVolumeSlider.value;
        }
        if (touchSensitivitySlider.value != sensitivity) {
            PlayerPrefs.SetFloat("sensitivity", touchSensitivitySlider.value);
            sensitivity = touchSensitivitySlider.value;
            if (touchInput != null) {
                touchInput.rotateUserSensitivity = (sensitivity * 1.8f) + 0.2f;
            }
        }
    }

    private void ShowSettings() {
        musicVol = PlayerPrefs.GetFloat("musicvol", 1f);
        seVol = PlayerPrefs.GetFloat("sevol", 1f);
        sensitivity = PlayerPrefs.GetFloat("sensitivity", 0.5f);
        musicVolumeSlider.value = musicVol;
        seVolumeSlider.value = seVol;
        touchSensitivitySlider.value = sensitivity;
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
        HideSettings();
        Time.timeScale = 1f;
    }
}
