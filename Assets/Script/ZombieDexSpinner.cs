using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDexSpinner : MonoBehaviour {
    public Transform spinningObject;
    public Transform camPos;

	void FixedUpdate () {
        spinningObject.Rotate(Vector3.up);
	}
}
