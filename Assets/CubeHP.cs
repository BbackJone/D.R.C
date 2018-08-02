using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeHP : MonoBehaviour {

    public int m_HP = 1000;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GetDamage(int _damage)
    {
        m_HP -= _damage;
    }
}
