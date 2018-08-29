using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButtonResultHandler : MonoBehaviour {
    	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            GetComponent<ResultUpdateScript>().ReturnToTitle();
        }
    }
}
