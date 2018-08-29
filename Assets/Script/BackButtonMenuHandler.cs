using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButtonMenuHandler : MonoBehaviour {
    public SettingsTitleScript sts;
    public StoreTitleScript storets;

	void Start () {
        if (sts == null || storets == null) enabled = false;
	}
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (Time.timeScale == 1f) {
                Application.Quit();
            } else {
                sts.HideSettingsAndResume();
                storets.HideStore();
            }
        }
    }
}
