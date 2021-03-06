﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsIngameScript : MonoBehaviour {
    private StageMgr stageMgr;
    private SaveDataManager saveMgr;
    private ResultScoreContainerScript rscs;
    private PlayerData pdata;
    private bool saveFeatureAvailable;
    public Light sun;

    public Slider musicVolumeSlider;
    public Slider seVolumeSlider;
    public Slider touchSensitivitySlider;
    public Slider brightnessSlider;
    public PlayerInputTouch touchInput;

    float musicVol;
    float seVol;
    float sensitivity;
    float brightness;

    // Possible problem: Attack hitbox collision may continue to occur after the game paused
    // └Workaround: other scripts can use (Time.timeScale == 1f) to determine if the game is paused or not

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

    void OnEnable() {
        stageMgr = GameObject.Find("StageMgr").GetComponent<StageMgr>();
        saveMgr = GameObject.Find("SaveDataManager").GetComponent<SaveDataManager>();
        rscs = GameObject.Find("ResultScoreContainer").GetComponent<ResultScoreContainerScript>();
        pdata = GameObject.Find("Santa").GetComponent<PlayerData>();
        saveFeatureAvailable = (ObjectManager.m_Inst != null && stageMgr != null && saveMgr != null && rscs != null && pdata != null);
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
        // Fix for the problem where player was able to pause when died
        // Note: pdata is not initiated if settings screen has not been shown
        if (GameObject.Find("Santa").GetComponent<PlayerData>().m_Hp == 0) return;

        Time.timeScale = 0f;
        ShowSettings();
    }

    public void HideSettingsAndResume() {
        HideSettings();
        Time.timeScale = 1f;
    }

    public void SaveAndExit() {
        if (saveFeatureAvailable) {
            /*
            SaveData sd = saveMgr.currentSaveData;
            sd.currentWave = stageMgr.m_CurrentWave.Level;
            sd.kills = rscs.kills;
            sd.elapsedTime = (int)StageMgr.instance.m_GameTime;
            sd.health = pdata.m_Hp;
            sd.spkills = rscs.spkills;
            SaveData.Write(sd, 0);
            */
            Destroy(GameObject.Find("ResultScoreContainer"));
            Time.timeScale = 1f;
            ObjectManager.m_Inst.NextScene("Menu");
        } else {
            Debug.LogError("SettingsIngameScript: Feature not available! Please restart from Menu scene");
        }
    }
}
