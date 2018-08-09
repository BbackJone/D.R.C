using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeCurtainEffectScript : MonoBehaviour {
    private float alpha = 1f;
    private Image image;

    void Start() {
        image = GetComponent<Image>();
        if (image == null) Destroy(gameObject);
    }
    	
	void Update () {
        alpha -= Time.deltaTime * 4f;
        if (alpha < 0) Destroy(gameObject);
        image.color = new Color(0f, 0f, 0f, alpha);
	}
}
