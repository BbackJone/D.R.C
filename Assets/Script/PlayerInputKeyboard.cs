using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handle player input by keyboard
/// </summary>
public class PlayerInputKeyboard : MonoBehaviour {
    private PlayerData data;

    private float moveSensitivity = 1.0f;
    private float dx = 0f;
    private float dy = 0f;

    private float lookSensitivity = 2f;

    private bool mouseLock = false;

    private float lookY;

    void Start () {
        data = GetComponent<PlayerData>();
        lookY = data.m_Camera.transform.eulerAngles.x;
    }

    void OnEnable() {
        Debug.Log("Keyboard Input enabled");
        Debug.Log("<b>Press Z to toggle mouse look</b>");
    }
	
	void Update () {
        dx = Input.GetAxis("Horizontal");
        dy = Input.GetAxis("Vertical");

        Vector3 vec = new Vector3(dx * moveSensitivity, 0, dy * moveSensitivity);
        data.m_Move = vec;
        
        // mouse look toggle by z
        if (Input.GetKeyDown(KeyCode.Z)) {
            mouseLock = !mouseLock;
            Cursor.lockState = (mouseLock) ? CursorLockMode.Locked : CursorLockMode.None;
        }

        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");
        
        if (Input.GetButton("Fire3"))
        {
            gameObject.SendMessage("Firebullet");
        }

        if (mouseLock) {
            transform.Rotate(Vector3.up * mx * lookSensitivity * (Screen.width / 1280f));
            lookY -= my * lookSensitivity * (Screen.height / 720f);
            lookY = Mathf.Clamp(lookY, -80f, 50f);
            data.m_Camera.transform.eulerAngles = new Vector3(lookY, data.m_Camera.transform.eulerAngles.y, 0f);
        }
    }
}
