using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUpdater : MonoBehaviour {
    public ResultScoreContainerScript rscs;
    public StageMgr stageMgr;

    private Text text;
    private int score;

	void Start () {
		if (rscs == null || stageMgr == null) {
            Debug.LogError("ScoreUpdater: Required ref is missing! Check Inspector!");
            enabled = false;
            return;
        }

        text = GetComponent<Text>();
	}

    void Update() {
        score = ((stageMgr.m_CurrentWave.Level - 1) * 10) + rscs.kills + (rscs.spkills * 10);
        text.text = score.ToString();
    }
}
