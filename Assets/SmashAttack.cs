using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashAttack : MonoBehaviour {

    private ZombieData m_Data;

    private void Awake()
    {
        m_Data = gameObject.GetComponentInParent<ZombieData>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !m_Data.m_Death)
        {
            Debug.Log("충돌");
            other.GetComponent<Rigidbody>().AddRelativeForce(Vector3.back * 20, ForceMode.Impulse);
            other.gameObject.SendMessage("GetDamage", 1);
        }
    }
}