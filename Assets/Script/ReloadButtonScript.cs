using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReloadButtonScript : MonoBehaviour, IPointerDownHandler {
    private PlayerAction action;

    void Start() {
        action = GameObject.Find("Santa").GetComponent<PlayerAction>();
        if (action == null) enabled = false;
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (Time.timeScale == 0f) return;
        action.Reload();
    }
}
