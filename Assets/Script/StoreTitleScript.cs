using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreTitleScript : MonoBehaviour {

    public void ShowStore()
    {
        Time.timeScale = 0f;
        gameObject.SetActive(true);
    }

    public void HideStore()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    public void MoneyCheat()
    {
        PlayerPrefs.SetInt("coins", 65535);
        PlayerPrefs.Save();
    }
}
