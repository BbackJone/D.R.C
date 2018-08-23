using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CoinCounterUpdater : MonoBehaviour {
    LanguageUpdater lu;
    Text text;

	void Start () {
        lu = GameObject.Find("LanguageUpdater").GetComponent<LanguageUpdater>();
        text = GetComponent<Text>();
        if (lu != null && text != null) StartCoroutine(UpdateCoinCount());
	}

    IEnumerator UpdateCoinCount() {
        while (!lu.lm.IsLoaded) {
            yield return new WaitForEndOfFrame();
        }

        while (true) {
            if (enabled) text.text = string.Format(lu.lm.Get("title_store_coin"), PlayerPrefs.GetInt("coins", 0));
            yield return new WaitForEndOfFrame();
        }
    }
}
