using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunInit : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.GetComponent<Light>().intensity = PlayerPrefs.GetFloat("brightness", 0.5f);
    }
}
