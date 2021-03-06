﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwapButtonScript : MonoBehaviour, IPointerDownHandler {
    //public GameObject weaponSwapScreen;
    private PlayerAction action;

    void Start() {
        action = GameObject.Find("Santa").GetComponent<PlayerAction>();

        if (action == null) enabled = false;
    }

    public void OnPointerDown(PointerEventData eventData) {
        //action.SwapWeapon();
    }

    public void SwapWeaponTo(int index) {
        action.SwapWeaponTo(index);
    }
}
