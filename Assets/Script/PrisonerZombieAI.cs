﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;   //this for NavMeshAgent

public class PrisonerZombieAI : MonoBehaviour {

    private ZombieData m_Data;
    private Animator m_Ani;
    private NavMeshAgent m_Nav;     //for finding route or move zombie

    public GameObject obj_body;
    public GameObject obj_head;
    public BoxCollider body_col;
    public BoxCollider head_col;

    public Transform m_target { get; set; }
    public float m_TargetDistance { get; set; }

    void Awake() {
        m_Nav = GetComponent<NavMeshAgent>();
        m_Data = GetComponent<ZombieData>();
        m_Ani = GetComponent<Animator>();

        body_col = obj_body.GetComponent<BoxCollider>();
        head_col = obj_head.GetComponent<BoxCollider>();
    }

    void OnEnable() {
        StageMgr.instance.AddSpecialZombieNumber(1);

        m_Nav.enabled = true;

        m_target = ObjectManager.m_Inst.m_Player.transform;

        StartCoroutine("TargetAttack");
        StartCoroutine("NavMove");
        StartCoroutine("DeathCheck");
    }

    // Use this for initialization
    void Start() {
        m_Nav.speed = m_Data.m_Speed;
    }


    IEnumerator TargetAttack() {
        while (true) {
            if (m_target) {
                m_TargetDistance = Vector3.Distance(this.transform.position, m_target.position);

                if (m_TargetDistance < m_Data.m_AttackRange) {
                    if (m_Data.m_AttackTimer < m_Data.m_AttackSpeed) {
                        m_Data.m_AttackTimer += Time.deltaTime;
                    } else {
                        m_Data.m_AttackTimer = 0f;
                        transform.LookAt(m_target);
                        gameObject.SendMessage("AttackofTwohand");
                    }
                }
            }

            yield return null;
        }
    }

    //Move toward target
    IEnumerator NavMove() {
        while (true) {
            if (m_target) {
                if (m_TargetDistance < m_Data.m_AttackRange * 0.75) {
                    m_Nav.Stop();
                } else {
                    m_Nav.Resume();
                    m_Nav.SetDestination(m_target.position);
                }
            }

            float move = m_Nav.desiredVelocity.magnitude;
            m_Ani.SetFloat("Speed", move);

            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator DeathCheck() {
        while (true) {
            if (m_Data.m_Hp <= 0) {
                if (m_Data.m_Death != true) {
                    m_Data.m_Death = true;
                    m_Nav.enabled = false;
                    m_Ani.SetBool("DeathBool", m_Data.m_Death);
                    m_Ani.SetTrigger("DeathTrigger");
                    body_col.enabled = false;
                    head_col.enabled = false;
                    StopCoroutine("FindTarget");
                    StopCoroutine("TargetAttack");
                    StopCoroutine("NavMove");
                }

                if (m_Data.m_DeathTimer < 4f)
                    m_Data.m_DeathTimer += Time.deltaTime;
                else {
                    body_col.enabled = true;
                    head_col.enabled = true;
                    //Because SetActive(false) with colliders inactive make a kind of bug(collider components are out of order), this is required
                    this.gameObject.SetActive(false);
                    StageMgr.instance.AddSpecialZombieNumber(-1);
                }
            }

            yield return null;
        }
    }
}
