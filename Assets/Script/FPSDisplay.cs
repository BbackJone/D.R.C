using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSDisplay : MonoBehaviour {

    float deltaTime = 0;
    GUIStyle style = new GUIStyle();
    int w = Screen.width;
    int h = Screen.height;
    Rect rect;

    private void Start()
    {
        rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color(255, 255, 255, 1);
    }

    // Update is called once per frame
    void Update () {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
	}

    private void OnGUI()
    {
        float msec = deltaTime * 1000;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }
}
