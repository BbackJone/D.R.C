using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBodyScript : MonoBehaviour {

    public void SetPositionAndEnable(Transform santaPos)
    {
        transform.position = santaPos.position + (santaPos.forward * 3f);
        gameObject.SetActive(true);
    }
}
