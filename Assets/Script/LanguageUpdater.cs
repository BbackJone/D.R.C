using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageUpdater : MonoBehaviour {
    public LanguageManager lm { get; set; }

    [Serializable]
    public struct TranslateMap
    {
        public UnityEngine.UI.Text text;
        public string key;
    }

    public List<TranslateMap> textsToTranslate;

	// Use this for initialization
	void Start () {
        lm = LanguageManager.GetInstance();
        if (!lm.IsLoaded) StartCoroutine(lm.LoadLanguage());

        StartCoroutine(UpdateText());
	}

    IEnumerator UpdateText()
    {
        while (!lm.IsLoaded)
        {
            yield return new WaitForEndOfFrame();
        }

        foreach (TranslateMap map in textsToTranslate)
        {
            map.text.text = lm.Get(map.key);
        }
    }
}
