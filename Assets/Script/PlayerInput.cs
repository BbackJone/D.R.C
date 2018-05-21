using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Flags]
public enum KeyDef
{
    KEY_FIRE            = 0x1,
    KEY_RELOAD          = 0x2,
    KEY_SWAP            = 0x4,
    KEY_FORWARD         = 0x8,
    KEY_BACKWARD        = 0x10,
    KEY_ATTACK          = 0x20,
}

public class PlayerInput : MonoBehaviour
{
    private PlayerData m_Data;
    public UIManager m_UI;

    private float m_Mouse_X;
    private float m_Mouse_Y;
    [SerializeField]
    [Range(0, 30)]
    private float m_mouseSensitivity;

    public bool init_Getbutton_order_safety { get; set; }

    private KeyCode[] KeyCodes = {
                                         KeyCode.Alpha0,
                                         KeyCode.Alpha1,
                                         KeyCode.Alpha2,
                                         KeyCode.Alpha3,
                                     };

    // Use this for initialization
    void Awake()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        m_Data = GetComponent<PlayerData>();
    }

    void OnEnable()
    {
        StartCoroutine("Getkey");
        StartCoroutine("ViewControl");
    }

    void Start()
    {
        m_Mouse_X = 0f;
        m_Mouse_Y = 0f;
        m_mouseSensitivity = 50f;

        StartCoroutine("Getkey");
        StartCoroutine("ViewControl");
    }


    //터치스크린의 슬라이드에 따라 카메라의 시점을 움직여주는 함수이다.
    IEnumerator ViewControl()
    {
        while (true)
        {
            transform.Rotate(Vector3.up * m_Mouse_X * Time.deltaTime * m_mouseSensitivity);
            m_Mouse_Y = Mathf.Clamp(m_Mouse_Y, -80f, 50f);
            m_Data.m_Camera.transform.eulerAngles = new Vector3(m_Mouse_Y, m_Data.m_Camera.transform.eulerAngles.y, 0f);

            yield return null;
        }
    }

    //입력장치로부터 입력을 받아 Key값을 변경시키는 함수이다.
    IEnumerator Getkey()
    {
        while (true)
        {
            if (init_Getbutton_order_safety == true)
            {
                init_Getbutton_order_safety = false;
            }
            else
            {
                m_Data.m_KeyInput = 0;
                m_Data.m_Move = Vector3.zero;
            }

            /**************************테스트용*****************************/
            /*Vector3 TempVec = m_Data.m_Move;
            TempVec.z = Input.GetAxis("Vertical");
            TempVec.x = Input.GetAxis("Horizontal");
            m_Data.m_Move = TempVec;*/
            m_Mouse_X = Input.GetAxis("Mouse X");
            m_Mouse_Y -= Input.GetAxis("Mouse Y");
            /**************************pc 키입력****************************/

            if (Input.touchCount >= 1)
            {
                Touch touch = Input.GetTouch(0);

                m_Mouse_X = touch.deltaPosition.x;
                m_Mouse_Y -= touch.deltaPosition.y;
            }

            yield return null;
        }

    }


    public void GetButtonMessage(string _msg)
    {
        init_Getbutton_order_safety = true;

        if (_msg == "forward")
        {
            Vector3 TempVec = m_Data.m_Move;
            TempVec.z = 1;
            m_Data.m_Move = TempVec;
        }

        if (_msg == "backward")
        {
            Vector3 TempVec = m_Data.m_Move;
            TempVec.z = -1;
            m_Data.m_Move = TempVec;
        }

        if (_msg == "fire")
        {
            //m_Data.m_KeyInput |= KeyDef.KEY_FIRE;
            gameObject.SendMessage("Firebullet");
        }

        if (_msg == "swap")
        {
            //m_Data.m_KeyInput |= KeyDef.KEY_SWAP;
            gameObject.SendMessage("SwapWeapon");
        }

        if (_msg == "reload")
        {
            //m_Data.m_KeyInput |= KeyDef.KEY_RELOAD;
            if (!m_Data.m_Reloading)
                gameObject.SendMessage("Reload");
        }
    }
}
