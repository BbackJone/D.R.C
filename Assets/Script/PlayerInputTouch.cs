using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handle player input by touch
/// </summary>
public class PlayerInputTouch : MonoBehaviour {
    public ControlStickScript controlStick;
    private PlayerData data;
    
    private float stickSensitivity = 2f;
    private float rotateXSensitivity = 0.24f;
    private float rotateYSensitivity = 0.16f;

    private Vector3 touchPos;
    private float lookY;

    private float mouseX;
    private float mouseY;
    
	void Start () {
        if (controlStick == null) enabled = false;
        data = GetComponent<PlayerData>();
        lookY = data.m_Camera.transform.eulerAngles.y;

        mouseX = Input.mousePosition.x;
        mouseY = Input.mousePosition.y;
    }

    void Update() {
        // Move character
        if (controlStick != null) { 
            Vector3 vec = new Vector3(controlStick.xAxis * stickSensitivity, 0, controlStick.yAxis * stickSensitivity);
            data.m_Move = vec;
        }

        // Handle camera movement
        if (Input.touchCount > 0) {
            // for every active touch pointer...
            for (int i = 0; i < Input.touchCount; i++) {
                // if this touch pointer is not using control stick...
                if (i != controlStick.draggingPointer) {
                    // assume this touch is trying to rotate camera
                    Touch touch = Input.GetTouch(i);

                    //Temporary...
                    if(touch.phase == TouchPhase.Began)
                    {
                        gameObject.SendMessage("fire");
                    }
                    ////

                    if (touch.phase != TouchPhase.Began) {
                        // apply rotation
                        transform.Rotate(Vector3.up * touch.deltaPosition.x * rotateXSensitivity);
                        lookY -= touch.deltaPosition.y * rotateYSensitivity;
                        lookY = Mathf.Clamp(lookY, -80f, 50f);
                        data.m_Camera.transform.eulerAngles = new Vector3(lookY, data.m_Camera.transform.eulerAngles.y, 0f);
                    }

                    break;
                }
            }
        }

#if false
        // Mouse substitution
        float dx = Input.mousePosition.x - mouseX;
        float dy = Input.mousePosition.y - mouseY;

        if (Input.GetMouseButton(0) && !controlStick.dragging) {
            transform.Rotate(Vector3.up * dx * rotateXSensitivity * (Screen.width / 1280f));
            lookY -= dy * rotateYSensitivity * (Screen.height / 720f);
            data.m_Camera.transform.eulerAngles = new Vector3(Mathf.Clamp(lookY, -80f, 50f), data.m_Camera.transform.eulerAngles.y, 0f);
        }

        mouseX = Input.mousePosition.x;
        mouseY = Input.mousePosition.y;
#endif
    }
}
