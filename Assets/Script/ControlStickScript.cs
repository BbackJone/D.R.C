using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlStickScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {
    Vector3 startPos;
    const float range = 90;
    
    /// <summary>
    /// Range between -1 to 1
    /// </summary>
    [HideInInspector]
    public float xAxis;

    /// <summary>
    /// Range between -1 to 1
    /// </summary>
    [HideInInspector]
    public float yAxis;

    private float xAxisRaw;
    private float yAxisRaw;

    [HideInInspector]
    public int draggingPointer = -1;
    [HideInInspector]
    public bool dragging = false;

    void Start () {
        startPos = transform.position;
	}

    void FixedUpdate() {
        if (!dragging) {
            xAxisRaw = xAxisRaw * 0.67f;
            yAxisRaw = yAxisRaw * 0.67f;
            xAxis = xAxisRaw / range;
            yAxis = yAxisRaw / range;
            transform.position = new Vector3(startPos.x + xAxisRaw, startPos.y + yAxisRaw, startPos.z);
        }
    }
    	
    public void OnDrag(PointerEventData eventData) {
        draggingPointer = eventData.pointerId;
        Vector2 stickpos = new Vector2(eventData.position.x - startPos.x, eventData.position.y - startPos.y);
        xAxisRaw = stickpos.x;
        yAxisRaw = stickpos.y;
        xAxis = xAxisRaw / range;
        yAxis = yAxisRaw / range;
        stickpos = Vector2.ClampMagnitude(stickpos, range);
        transform.position = new Vector3(startPos.x + stickpos.x, startPos.y + stickpos.y, startPos.z);
    }

    public void OnPointerDown(PointerEventData eventData) {
        draggingPointer = eventData.pointerId;
        dragging = true;
    }

    public void OnPointerUp(PointerEventData eventData) {
        /*
        transform.position = startPos;
        xAxisRaw = 0;
        yAxisRaw = 0;
        xAxis = 0;
        yAxis = 0;
        */
        draggingPointer = -1;
        dragging = false;
    }
}
