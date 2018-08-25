using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CoinCounterUpdater : MonoBehaviour {
    LanguageUpdater lu;
    Text text;

    private bool updateOk = false;

	void Start () {
        lu = GameObject.Find("LanguageUpdater").GetComponent<LanguageUpdater>();
        text = GetComponent<Text>();
        if (lu != null && text != null) StartCoroutine(UpdateCoinCount());
	}

    void Update()
    {
        if (updateOk) text.text = string.Format(lu.lm.Get("title_store_coin"), PlayerPrefs.GetInt("coins", 0));
    }

    IEnumerator UpdateCoinCount() {
        while (!lu.lm.IsLoaded) {
            yield return new WaitForEndOfFrame();
        }

        updateOk = true;
        Update();
    }
}
