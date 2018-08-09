using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsIngameScript : MonoBehaviour {
    private StageMgr stageMgr;
    private SaveDataManager saveMgr;
    private bool saveFeatureAvailable;

    public Slider musicVolumeSlider;
    public Slider seVolumeSlider;
    
    // Possible problem: Attack hitbox collision may continue to occur after the game paused
    // └Workaround: other scripts can use (Time.timeScale == 1f) to determine if the game is paused or not

    void OnEnable() {
        stageMgr = GameObject.Find("StageMgr").GetComponent<StageMgr>();
        saveMgr = GameObject.Find("SaveDataManager").GetComponent<SaveDataManager>();
        saveFeatureAvailable = (ObjectManager.m_Inst != null && stageMgr != null && saveMgr != null);
    }

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

    public void SaveAndExit() {
        if (saveFeatureAvailable) {
            SaveData sd = saveMgr.currentSaveData;
            sd.currentWave = stageMgr.m_CurrentWave.Level;
            SaveData.Write(sd, 0);
            Time.timeScale = 1f;
            ObjectManager.m_Inst.NextScene("Menu");
        } else {
            Debug.LogError("SettingsIngameScript: Feature not available! Please restart from Menu scene");
        }
    }
}
