using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponButtonEnabler : MonoBehaviour {
    public string weaponName;
	
	void Start () {
        if (string.IsNullOrEmpty(weaponName)) {
            gameObject.SetActive(false);
            return;
        }
        
        bool isWeaponPurchased = (PlayerPrefs.GetInt("WeaponPurchased_" + weaponName, 0) == 1);
        
        if (!isWeaponPurchased) {
            gameObject.SetActive(false);
            return;
        }
    }
}
