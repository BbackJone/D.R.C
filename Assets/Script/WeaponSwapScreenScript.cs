using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwapScreenScript : MonoBehaviour {

	void Start() {

    }

    public void ShowWeaponSwapScreen() {
        Time.timeScale = 0.1f;
        gameObject.SetActive(true);
    }

    public void HideWeaponSwapScreen() {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
}
