using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireButtonScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    private PlayerData playerData;

    void Start() {
        playerData = ObjectManager.m_Inst.Objects.m_Playerlist[0].GetComponent<PlayerData>();
        if (playerData == null)
            enabled = false;
    }

    public void OnPointerDown(PointerEventData eventData) {
        playerData.m_isShooting = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        playerData.m_isShooting = false;
    }
}
