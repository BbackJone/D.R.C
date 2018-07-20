using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitboxScript : MonoBehaviour {

    private PlayerData data;

    private void Start()
    {
        data = GameObject.Find("Santa").GetComponent<PlayerData>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("Santa"))
        {
            data.m_Hp = Math.Max(data.m_Hp - 1, 0);
        }
    }
}
