using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstralMove : MonoBehaviour {

    public Transform m_LookPos;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        transform.RotateAround(Vector3.zero, new Vector3(0,1,1), 18f * Time.deltaTime);
        transform.LookAt(m_LookPos.position);
    }
}
