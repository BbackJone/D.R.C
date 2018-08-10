using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SantaPositionPreserver : MonoBehaviour {
    Vector3 santaPos;
    Quaternion santaRot;
    //Quaternion camRot;
    float mouseY;

    public void SaveSantaPos(Vector3 pos, Quaternion rot, float mouseY) {
        santaPos = pos;
        santaRot = rot;
        this.mouseY = mouseY;
    }

    public void LoadSantaPos(GameObject santa) {
        santa.transform.position = santaPos;
        santa.transform.rotation = santaRot;
        var pi = santa.GetComponent<PlayerInput>();
        if (pi != null) pi.m_Mouse_Y = mouseY;
        var pit = santa.GetComponent<PlayerInputTouch>();
        if (pit != null) pit.lookY = mouseY;
    }
}
