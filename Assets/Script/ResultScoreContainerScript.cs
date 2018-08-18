using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultScoreContainerScript : MonoBehaviour {
    public int kills { get; set; }
    public int waves { get; set; }
    public int score { get; set; }
    public float timeInSec { get; set; }
    public bool isGameClear { get; set; }

    bool doCalcTime;

    void Start () {
        DontDestroyOnLoad(gameObject);
        kills = 0;
        waves = 0;
        score = 0;
        doCalcTime = true;
        timeInSec = 0f;
        isGameClear = false;
	}

    void Update() {
        if (doCalcTime && Time.timeScale != 0f) {
            timeInSec += Time.deltaTime;
        }
    }

    public void SetResultsAndStopTime(int kills, int waves, int score, bool isGameClear) {
        doCalcTime = false;

        this.kills = kills;
        this.waves = waves;
        this.score = score;
        this.isGameClear = isGameClear;
    }
}
