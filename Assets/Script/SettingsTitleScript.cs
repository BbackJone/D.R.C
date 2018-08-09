using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsTitleScript : MonoBehaviour {
    public Slider musicVolumeSlider;
    public Slider seVolumeSlider;
    
    private void ShowSettings() {
        // TODO: Update settings here...
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
