using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDexSpinner : MonoBehaviour {
    public Transform camPos;

	void FixedUpdate () {
        transform.Rotate(Vector3.up * 0.5f);
	}
}
