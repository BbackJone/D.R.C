using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwapScreenScript : MonoBehaviour {

    public void ShowWeaponSwapScreen() {
        if (Time.timeScale == 0f) return;
        Time.timeScale = 0.15f;
        gameObject.SetActive(true);
    }

    public void HideWeaponSwapScreen() {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
}
