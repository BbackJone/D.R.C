using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultScoreContainerScript : MonoBehaviour {
    public int kills { get; set; }
    public int waves { get; set; }
    public int score { get; set; }
    public float timeInSec { get; set; }

    bool doCalcTime;

    void Start () {
        DontDestroyOnLoad(gameObject);
        doCalcTime = true;
        timeInSec = 0f;
	}

    void Update() {
        if (doCalcTime && Time.timeScale != 0f) {
            timeInSec += Time.deltaTime;
        }
    }

    public void SetResultsAndStopTime(int kills, int waves, int score) {
        doCalcTime = false;

        this.kills = kills;
        this.waves = waves;
        this.score = score;
    }
}
