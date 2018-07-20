using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwapButtonScript : MonoBehaviour, IPointerDownHandler {
    private PlayerAction action;
    public AudioClip AudioClip;
    public AudioSource AudioSource;
    void Start() {
        AudioSource.clip = AudioClip;
        action = GameObject.Find("Santa").GetComponent<PlayerAction>();
        if (action == null) enabled = false;
    }

    public void OnPointerDown(PointerEventData eventData) {
        AudioSource.PlayOneShot(AudioSource.clip);
        action.SwapWeapon();
    }
}
