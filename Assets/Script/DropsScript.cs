using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropsScript : MonoBehaviour {
    public enum DropsType {
        Heart,
        Ammo
    }

    public DropsType type;

	void Update() {
        transform.Rotate(Vector3.down * 1.75f);
	}

    void OnTriggerEnter(Collider other) {
        var m_Data = other.gameObject.GetComponent<PlayerData>();

        if (m_Data != null) {
            try {
                if (type == DropsType.Heart) {
                    m_Data.m_Hp = Math.Min(m_Data.m_Hp + 5, m_Data.m_MaxHp);
                } else if (type == DropsType.Ammo) {
                    m_Data.m_WeaponInhand.m_AmmoBulletNum = Math.Min(m_Data.m_WeaponInhand.m_AmmoBulletNum + 5, m_Data.m_WeaponInhand.m_MaxBulletNum);
                }
            } catch (Exception) { }
            
            DestroyObject(gameObject);
        }
    }
}
