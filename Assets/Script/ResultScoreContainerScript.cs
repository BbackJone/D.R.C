using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultScoreContainerScript : MonoBehaviour {
    public int kills { get; set; }
    public int spkills { get; set; }
    public int waves { get; set; }
    public int score { get; set; }
    public bool isGameClear { get; set; }

    void Start () {
        DontDestroyOnLoad(gameObject);

        var sdm = GameObject.Find("SaveDataManager").GetComponent<SaveDataManager>();
        if (sdm != null) {
            kills = sdm.currentSaveData.kills;
            spkills = sdm.currentSaveData.spkills;
            waves = sdm.currentSaveData.currentWave;
            score = 0;      // to be calculated later
            StageMgr.instance.m_GameTime = sdm.currentSaveData.elapsedTime;
        } else {
            kills = 0;
            spkills = 0;
            waves = 0;
            score = 0;
            StageMgr.instance.m_GameTime = 0f;
        }
        isGameClear = false;
    }

    public void SetResultsAndStopTime(int kills, int spkills, int waves, int score, bool isGameClear) {
        this.kills = kills;
        this.spkills = spkills;
        this.waves = waves;
        this.score = score;
        this.isGameClear = isGameClear;
    }
}
