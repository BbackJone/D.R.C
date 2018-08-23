using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultUpdateScript : MonoBehaviour {
    public Text varTitle;
    public Text varKill;
    public Text varWave;
    public Text varScore;
    public Text varTime;
    public Text varCoin;

    public Text textReturn;
    private bool isTitleLoading = false;

    void Start () {
        var rsc = GameObject.Find("ResultScoreContainer");

        if (rsc != null) {
            var rscs = rsc.GetComponent<ResultScoreContainerScript>();
            varKill.text = rscs.kills + "";
            varWave.text = rscs.waves + "";
            varScore.text = rscs.score + "";
            var ts = new TimeSpan(0, 0, (int)rscs.timeInSec);
            varTime.text = string.Format("{0:00}:{1:00}", (int)ts.TotalMinutes, ts.Seconds);
        } else {
            varKill.text = "123";
            varWave.text = "456";
            varScore.text = "789";
            varTime.text = "00:00";
        }

        StartCoroutine(UpdateText());
	}

    public void ReturnToTitle() {
        if (isTitleLoading) return;
        isTitleLoading = true;
        var lu = GameObject.Find("LanguageUpdater").GetComponent<LanguageUpdater>();
        textReturn.text = (lu.lm.IsLoaded ? lu.lm.Get("result_loading") : "Please wait...");
        SceneManager.LoadSceneAsync("Menu");
    }

    IEnumerator UpdateText()
    {
        var lu = GameObject.Find("LanguageUpdater").GetComponent<LanguageUpdater>();
        while (!lu.lm.IsLoaded) { yield return new WaitForEndOfFrame(); }

        var rsc = GameObject.Find("ResultScoreContainer");
        if (rsc != null)
        {
            var rscs = rsc.GetComponent<ResultScoreContainerScript>();
            varTitle.text = (rscs.isGameClear ? lu.lm.Get("result_title_clear") : lu.lm.Get("result_title"));

            int curCoin = PlayerPrefs.GetInt("coins", 0);
            int newCoin = rscs.score / 10;
            PlayerPrefs.SetInt("coins", curCoin + newCoin);
            PlayerPrefs.Save();
            varCoin.text = string.Format(lu.lm.Get("result_coins"), newCoin);

            Destroy(rsc);
        }
    }
}
