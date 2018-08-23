using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearConfirmScreenScript : MonoBehaviour {

    public void ShowClearConfirmScreen() {
        gameObject.SetActive(true);
    }

    public void HideClearConfirmScreen() {
        gameObject.SetActive(false);
    }

    public void ClearSaveAndHideClearConfirmScreen() {
        // continue data will get cleared by TitleButtonScript.
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        HideClearConfirmScreen();
    }
}
