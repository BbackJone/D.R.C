using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Handle Image's button sprite swap
/// </summary>
public class ButtonAnimationScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    public Sprite sprNormal;
    public Sprite sprPressed;

    private bool isProperlySetup;
    private Image img;

    void Start() {
        isProperlySetup = !(sprNormal == null || sprPressed == null);
        if (!isProperlySetup) {
            enabled = false;
            return;
        }
        img = gameObject.GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData) {
        img.sprite = sprPressed;
    }

    public void OnPointerUp(PointerEventData eventData) {
        img.sprite = sprNormal;
    }
}
