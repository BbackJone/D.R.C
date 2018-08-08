using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SantaPositionPreserver : MonoBehaviour {
    Vector3 santaPos;
    Quaternion santaRot;

    public void SaveSantaPos(Vector3 pos, Quaternion rot) {
        santaPos = pos;
        santaRot = rot;
    }

    public void LoadSantaPos(GameObject santa) {
        santa.transform.position = santaPos;
        santa.transform.rotation = santaRot;
    }
}
