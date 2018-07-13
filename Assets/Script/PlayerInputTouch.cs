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
    
    private float stickSensitivity = 2f;
    private float rotateXSensitivity = 0.24f;
    private float rotateYSensitivity = 0.16f;

    private Vector3 touchPos;
    private float lookY;

    private float mouseX;
    private float mouseY;

    private int screenDraggingPointer = -1;
    // scale threshold according to the screen resolution - base value is 50 at 720p
    private float touchRotateThreshold = 50 * (Screen.width / 1280);
    private const float touchFireTimeConst = 0.33f;
    private float touchFireTime = touchFireTimeConst;
    private Dictionary<int, Vector2> touchDownPos;
    
	void Start () {
        if (controlStick == null) enabled = false;
        data = GetComponent<PlayerData>();
        action = GetComponent<PlayerAction>();
        lookY = data.m_Camera.transform.eulerAngles.y;

        mouseX = Input.mousePosition.x;
        mouseY = Input.mousePosition.y;

        touchDownPos = new Dictionary<int, Vector2>();
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
                Touch touch = Input.GetTouch(i);
                // if this touch pointer is not using control stick...
                if (touch.fingerId != controlStick.draggingPointer) {
                    // if screenDraggingPointer is not set or this touch pointer is screenDraggingPointer...
                    if (screenDraggingPointer == -1 || touch.fingerId == screenDraggingPointer) { 
                        if (touch.phase == TouchPhase.Began) {
                            // save touch start position
                            screenDraggingPointer = touch.fingerId;
                            touchDownPos.Add(touch.fingerId, touch.position);
                        } else if (touch.phase == TouchPhase.Ended) {
                            // fire once if touchFireTime is not expired (if short touch has happened)
                            if (touchFireTime > 0f) {
                                action.Firebullet();
                            }
                            // reset touch state
                            screenDraggingPointer = -1;
                            touchDownPos.Remove(touch.fingerId);
                            touchFireTime = touchFireTimeConst;
                        } else if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) {
                            if (touchDownPos[touch.fingerId] != null) {
                                // check if touch point has moved beyond threshold
                                if (Vector2.Distance(touchDownPos[touch.fingerId], touch.position) > 30) {
                                    // user is dragging, set touchDownPos[touch.fingerId] to null to indicate this finger is rotating screen
                                    touchDownPos.Remove(touch.fingerId);
                                } else {
                                    // touch point is staying inside threshold - check touchFireTime
                                    if (touchFireTime < 0f) {
                                        // if touchFireTime is under 0, fire the weapon
                                        // since this call is in update, firing will called continuously
                                        action.Firebullet();
                                    } else {
                                        // decrease touchFireTime by deltaTime
                                        touchFireTime -= Time.deltaTime;
                                    }
                                }
                            } else {
                                // user is clearly trying to rotate screen
                                transform.Rotate(Vector3.up * touch.deltaPosition.x * rotateXSensitivity);
                                lookY -= touch.deltaPosition.y * rotateYSensitivity;
                                lookY = Mathf.Clamp(lookY, -80f, 50f);
                                data.m_Camera.transform.eulerAngles = new Vector3(lookY, data.m_Camera.transform.eulerAngles.y, 0f);
                            }
                        }

                        break;
                    }
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
