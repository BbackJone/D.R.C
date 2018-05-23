using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMgr : MonoBehaviour {

    public PlayerInput m_PlayerInput;

    public void GetMessage(string msg)
    {
        m_PlayerInput.GetButtonMessage(msg);
    }

}