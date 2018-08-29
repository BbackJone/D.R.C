using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButtonInGameHandler : MonoBehaviour {
    public SettingsIngameScript sis;
    public WeaponSwapScreenScript wsss;
	
	void Start () {
        if (sis == null || wsss == null) enabled = false;
	}
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (Time.timeScale == 1f) {
                sis.PauseAndShowSettings();
            } else if (Time.timeScale == 0f) {
                sis.HideSettingsAndResume();
            } else {
                wsss.HideWeaponSwapScreen();
            }
        }
    }
}
