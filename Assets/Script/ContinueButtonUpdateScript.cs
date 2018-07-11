using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButtonUpdateScript : MonoBehaviour {
	void Start () {
        if (!SaveData.SaveExists(0)) {
            transform.Find("Text").GetComponent<Text>().text = "Continue from\nNew Game";
        } else {
            transform.Find("Text").GetComponent<Text>().text = "Continue from\nWave " + SaveData.Load(0).currentWave;
        }
	}
}
