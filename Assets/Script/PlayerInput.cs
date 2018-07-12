using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    private PlayerData m_Data;
    public UIManager m_UI;

    //this is required to rotate player's view
    private float m_Mouse_X;
    private float m_Mouse_Y;

    //mouseSensitivity
    [SerializeField]
    [Range(0, 30)]
    private float m_mouseSensitivity;

    //to fix bug about button input.
    /*"m_Data.m_Move = Vector3.zero;" in Getkey,
    and "GetButtonMessage()" order problem*/
    public bool init_Getbutton_order_safety { get; set; }

    // Use this for initialization
    void Awake()
    {
        m_Data = GetComponent<PlayerData>(); 
    }


    void Start()
    {
        m_Mouse_X = 0f;
        m_Mouse_Y = 0f;
        m_mouseSensitivity = 50f;
    }

    private void Update()
    {
        ViewControl();
        CheckKey();
    }

    void CheckKey()
    {
        //If m_Move is set with some value at "GetButtonMessage" already,
        //There is no initialization of m_move here.
        if (init_Getbutton_order_safety == true)
        {
            init_Getbutton_order_safety = false;
        }
        else
        {
            m_Data.m_Move = Vector3.zero;
        }

        /**************************테스트용*****************************/
        Vector3 TempVec = m_Data.m_Move;
        TempVec.z = Input.GetAxis("Vertical");
        TempVec.x = Input.GetAxis("Horizontal");

        m_Data.m_Move = TempVec;
        m_Mouse_X = Input.GetAxis("Mouse X");
        m_Mouse_Y -= Input.GetAxis("Mouse Y");

        if (Input.GetKeyDown("f"))   //left shift
        {
            gameObject.SendMessage("Firebullet");
            Debug.Log("F Key Pressed!");
        }
        if (Input.GetKeyDown("r"))   //left shift
        {
            if (!m_Data.m_Reloading)
                gameObject.SendMessage("Reload");
        }
        if (Input.GetKeyDown("t"))   //left shift
        {
            gameObject.SendMessage("SwapWeapon");
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        /**************************pc 키입력****************************/

        //Here is about touch input(View rotation value)
        if (Input.touchCount >= 1)
        {
            Touch touch = Input.GetTouch(0);

            m_Mouse_X = touch.deltaPosition.x;
            m_Mouse_Y -= touch.deltaPosition.y;
        }
    }
    
    void ViewControl()
    {
        transform.Rotate(Vector3.up * m_Mouse_X * Time.deltaTime * m_mouseSensitivity);
        m_Mouse_Y = Mathf.Clamp(m_Mouse_Y, -80f, 50f);
        m_Data.m_Camera.transform.eulerAngles = new Vector3(m_Mouse_Y, m_Data.m_Camera.transform.eulerAngles.y, 0f);
    }

    //Get Message about button from ButtonMgr, and handle that appropriately
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
            gameObject.SendMessage("Firebullet");
        }

        if (_msg == "swap")
        {
            gameObject.SendMessage("SwapWeapon");
        }

        if (_msg == "reload")
        {
            //Relaod only when player is not reloading
            if (!m_Data.m_Reloading)
                gameObject.SendMessage("Reload");
        }
    }
}