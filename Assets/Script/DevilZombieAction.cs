﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilZombieAction : MonoBehaviour {

    private Animator m_Ani;
    private DevilZombieAI m_AI;
    private ZombieData m_Data;

    // Use this for initialization
    void Awake() {
        m_Ani = GetComponent<Animator>();
        m_AI = GetComponent<DevilZombieAI>();
        m_Data = GetComponent<ZombieData>();
    }

    void ShootFlame()
    {
        m_Ani.SetTrigger("Attack");
        //Make a shoot direction vector
        Vector3 ShootDirection;
        if (m_AI.m_target)
            ShootDirection = m_AI.m_target.transform.position - transform.position;
        else
            ShootDirection = transform.forward;

        GameObject Flame = ObjectPoolMgr.instance.CreatePooledObject("FlameParticle", transform.position,
            Quaternion.LookRotation(ShootDirection.normalized));
        Flame.SendMessage("SetDamage", m_Data.m_AttackDamage);
    }
}
