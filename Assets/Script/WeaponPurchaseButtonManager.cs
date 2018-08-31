using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPurchaseButtonManager : MonoBehaviour {
    public string weaponName;
    public int weaponPrice;

    public Button weaponPurchaseButton;
    public Image weaponPurchaseButtonImage;
    public Text weaponDescText;
    public Text weaponPriceText;

    public Text coinCounterText;

    public GameObject otherObject;

    LanguageUpdater lu;

    public float fadeColor = 0.2f;

    void Start () {
        // sanity check
        if (string.IsNullOrEmpty(weaponName)) {
            Debug.LogError("WeaponPurchaseButtonManager: Weapon Name is not set!");
            enabled = false;
            return;
        }

        if (weaponPrice < 0) {
            Debug.LogError("WeaponPurchaseButtonManager: Weapon Price must be positive!");
            enabled = false;
            return;
        }

        if (weaponPurchaseButtonImage == null || weaponDescText == null || weaponPriceText == null) {
            Debug.LogError("WeaponPurchaseButtonManager: Missing required objects, check inspector!");
            enabled = false;
            return;
        }

        lu = GameObject.Find("LanguageUpdater").GetComponent<LanguageUpdater>();
        if (lu != null) StartCoroutine(UpdateWeaponButton());
        else Debug.LogError("WeaponPurchaseButtonManager: Cannot find LanguageUpdater!");
    }

    IEnumerator UpdateWeaponButton() {
        while (lu.lm == null || !lu.lm.IsLoaded) {
            yield return new WaitForEndOfFrame();
        }

        while (true) {
            if (enabled) {
                weaponDescText.text = lu.lm.Get("title_store_" + weaponName + "_desc");
                weaponPriceText.text = weaponPrice + "";
                if (PlayerPrefs.GetInt("WeaponPurchased_" + weaponName, 0) == 0) {
                    weaponPurchaseButton.interactable = true;
                    weaponPurchaseButtonImage.color = Color.white;
                } else {
                    weaponPurchaseButton.interactable = false;
                    weaponPurchaseButtonImage.color = Color.green;
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public void PurchaseWeapon() {
        int coin = PlayerPrefs.GetInt("coins", 0);
        if (coin >= weaponPrice) {
            otherObject.SendMessage("uisound5");
            PlayerPrefs.SetInt("coins", coin - weaponPrice);
            PlayerPrefs.SetInt("WeaponPurchased_" + weaponName, 1);
            PlayerPrefs.Save();
        } else {
            otherObject.SendMessage("uisound4");
            StopCoroutine(SetRedThenFadeRevertCoinCounterColor());
            StartCoroutine(SetRedThenFadeRevertCoinCounterColor());
        }
    }

    IEnumerator SetRedThenFadeRevertCoinCounterColor() {
        coinCounterText.color = new Color(1f, 0f, 0f);

        while (true) {
            if (fadeColor < 1f) {
                fadeColor += 0.075f;
                coinCounterText.color = new Color(1f, fadeColor, fadeColor);
            } else {
                fadeColor = 0.2f;
                coinCounterText.color = new Color(1f, 1f, 1f);
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
