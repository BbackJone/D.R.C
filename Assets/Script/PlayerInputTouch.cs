using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handle player input by touch
/// </summary>
public class PlayerInputTouch : MonoBehaviour {
    public ControlStickScript controlStick;
    private PlayerData data;
    private PlayerAction action;
    
    private float stickSensitivity = 0.75f;
    private float rotateXSensitivity = 0.20f;
    private float rotateYSensitivity = 0.12f;

    public float rotateUserSensitivity;

    private Vector3 touchPos;
    public float lookY { get; set; }

    private float mouseX;
    private float mouseY;

    //private int screenDraggingPointer = -1;
    private float touchRotateThreshold = 12 * (Screen.width / 1280f);
    private const float touchFireTimeConst = 0.2f;
    private float touchFireTime = touchFireTimeConst;

    //private Vector2[] touchDragDeltaA;
    private float touchDragDist;
    
	void Start () {
        if (controlStick == null) enabled = false;
        data = GetComponent<PlayerData>();
        action = GetComponent<PlayerAction>();
        //lookY = data.m_Camera.transform.eulerAngles.y;

        data.m_Camera.transform.eulerAngles = new Vector3(lookY, data.m_Camera.transform.eulerAngles.y, 0f);

        mouseX = Input.mousePosition.x;
        mouseY = Input.mousePosition.y;
        /*
        touchDragDeltaA = new Vector2[20];
        for (int i = 0; i < 20; i++) {
            touchDragDeltaA[i] = Vector2.zero;
        }
        */

        // rotateUserSensitivity range: [0.2 - 2.0] (default: 1.1)
        rotateUserSensitivity = (PlayerPrefs.GetFloat("sensitivity") * 1.8f) + 0.2f;
    }

    void Update() {
        if (Time.timeScale == 0f) return;

        // Move character
        if (controlStick != null) { 
            Vector3 vec = new Vector3(controlStick.xAxis * stickSensitivity, 0, controlStick.yAxis * stickSensitivity);
            data.m_Move = vec;
        }

        #region Look-around only implementation
        // Handle camera movement
        if (Input.touchCount > 0) {
            // for every active touch pointer...
            for (int i = 0; i < Input.touchCount; i++) {
                Touch touch = Input.GetTouch(i);

                // top-left singleshot
                if (touch.phase == TouchPhase.Began && touch.position.x < Screen.width / 2 && touch.position.y > Screen.height / 2)
                {
                    StartCoroutine(SingleShot());
                }

                // if this touch pointer is not using control stick...
                if (touch.fingerId != controlStick.draggingPointer) {
                    // assume this touch is trying to rotate camera
                    if (touch.phase != TouchPhase.Began &&
                        touch.phase != TouchPhase.Ended &&
                        touch.phase != TouchPhase.Canceled) {
                        if (touchDragDist < touchRotateThreshold) {
                            touchDragDist += touch.deltaPosition.magnitude;
                        } else {
                            // apply rotation
                            transform.Rotate(Vector3.up * touch.deltaPosition.x * rotateXSensitivity * rotateUserSensitivity);
                            lookY -= touch.deltaPosition.y * rotateYSensitivity * rotateUserSensitivity;
                            lookY = Mathf.Clamp(lookY, -80f, 35f);
                            data.m_Camera.transform.eulerAngles = new Vector3(lookY, data.m_Camera.transform.eulerAngles.y, 0f);
                        }
                    } else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
                        touchDragDist = 0;
                    }

                    //break;
                }
            }
        }
        #endregion

        #region Short touch fire implementation (not used)
#if false
        // Handle camera movement
        if (Input.touchCount > 0) {
            // for every active touch pointer...
            for (int i = 0; i < Input.touchCount; i++) {
                Touch touch = Input.GetTouch(i);
                
                if (touch.fingerId == controlStick.draggingPointer || touch.fingerId < 0 || touch.fingerId > 19) continue;
                
                if (touch.phase == TouchPhase.Began) {
                    touchDragDeltaA[touch.fingerId] = touch.position;
                } else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
                    if (!touchDragDeltaA[touch.fingerId].Equals(Vector2.zero)) {
                        action.Firebullet();
                    }
                    touchFireTime = touchFireTimeConst;
                } else {
                    if (touchDragDeltaA[touch.fingerId] != Vector2.zero) {
                        if (Vector2.Distance(touchDragDeltaA[touch.fingerId], touch.position) > touchRotateThreshold) {
                            touchDragDeltaA[touch.fingerId] = Vector2.zero;
                        } else {
                            touchFireTime -= Time.deltaTime;
                            if (touchFireTime < 0) action.Firebullet();
                        }
                    } else {
                        transform.Rotate(Vector3.up * touch.deltaPosition.x * rotateXSensitivity);
                        lookY -= touch.deltaPosition.y * rotateYSensitivity;
                        lookY = Mathf.Clamp(lookY, -80f, 50f);
                        data.m_Camera.transform.eulerAngles = new Vector3(lookY, data.m_Camera.transform.eulerAngles.y, 0f);
                    }
                }
            }
        }
#endif
        #endregion

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

    IEnumerator SingleShot()
    {
        data.m_isShooting = true;
        yield return new WaitForEndOfFrame();
        data.m_isShooting = false;
    }
}
