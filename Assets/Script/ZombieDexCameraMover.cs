using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDexCameraMover : MonoBehaviour {
    public Transform target;

	void FixedUpdate () {
		if (target != null)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, 0.25f);
            transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, 0.25f);
        }
	}
}
