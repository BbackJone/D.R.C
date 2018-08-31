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
        transform.Rotate(Vector3.down * 50f * Time.deltaTime);
	}

    void OnTriggerEnter(Collider other) {
        var m_Data = other.gameObject.GetComponent<PlayerData>();

        if (m_Data != null) {
            try {
                if (type == DropsType.Heart) {
                    m_Data.m_Hp = Math.Min(m_Data.m_Hp + 20, m_Data.m_MaxHp);
                }
            } catch (Exception) { }

            gameObject.SetActive(false);
        }
    }
}
