using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SantaPositionFailsafe : MonoBehaviour {
    public Transform santaPos;

	void Start () {
        if (santaPos == null)
        {
            gameObject.SetActive(false);
            return;
        }
        StartCoroutine(CheckSantaPos());
	}
	
	IEnumerator CheckSantaPos() {
        while (true)
        {
            if (santaPos.position.y < -30) santaPos.position = transform.position;
            yield return new WaitForSeconds(5f);
        }
	}
}
