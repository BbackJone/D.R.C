using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButtonUpdateScript : MonoBehaviour {
	void Start () {
        StartCoroutine(UpdateText());
	}

    IEnumerator UpdateText()
    {
        var lu = GameObject.Find("LanguageUpdater").GetComponent<LanguageUpdater>();
        while (!lu.lm.IsLoaded)
        {
            yield return new WaitForEndOfFrame();
        }

        if (!SaveData.SaveExists(0))
        {
            transform.Find("Text").GetComponent<Text>().text = lu.lm.Get("title_continue_nf");
        }
        else
        {
            transform.Find("Text").GetComponent<Text>().text = string.Format(lu.lm.Get("title_continue"), SaveData.Load(0).currentWave);
        }
    }
}
