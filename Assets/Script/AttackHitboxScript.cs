using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitboxScript : MonoBehaviour {
    /// <summary>
    /// Damage dealt to player when the hitbox is triggered.
    /// </summary>
    public int damage = 1;

    private PlayerData data;

    private void Start()
    {
        data = ObjectManager.m_Inst.Objects.m_Playerlist[0].GetComponent<PlayerData>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("Santa"))
        {
            data.m_Hp = Math.Max(data.m_Hp - damage, 0);
        }
    }
}
