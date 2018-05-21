using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour {

    private ZombieData m_Data;
    private ZombieInteraction m_Interaction;
    private Animator m_Ani;
    private Collider m_CapsuleCol;
    private NavMeshAgent m_Nav;

    public Transform m_target { get; set; }
    public float m_TargetDistance { get; set; }


    void Awake()
    {
        m_Nav = GetComponent<NavMeshAgent>();
        m_Data = GetComponent<ZombieData>();
        m_Interaction = GetComponent<ZombieInteraction>();
        m_Ani = GetComponent<Animator>();
        m_CapsuleCol = GetComponent<CapsuleCollider>();
    }

    void OnEnable()
    {
        m_Nav.enabled = true;

        StartCoroutine("FindTarget");
        StartCoroutine("TargetAttack");
        StartCoroutine("NavMove");
        StartCoroutine("DeathCheck");
    }
	// Use this for initialization
	void Start () {
        m_Nav.speed = m_Data.m_Speed;
	}


    IEnumerator TargetAttack()
    {
        while (true)
        {
            if (m_target)
            {
                m_TargetDistance = Vector3.Distance(this.transform.position, m_target.position);

                if (m_TargetDistance < m_Data.m_AttackRange)
                {
                    if (m_Data.m_AttackTimer < m_Data.m_AttackSpeed)
                    {
                        m_Data.m_AttackTimer += Time.deltaTime;
                    }
                    else
                    {
                        m_Data.m_AttackTimer = 0f;
                        transform.LookAt(m_target);
                        gameObject.SendMessage("AttackofTwohand");
                    }
                }
            }

            yield return null;
        }
    }

    IEnumerator NavMove()
    {
        while (true)
        {
            if (m_target)
            {
                if (m_TargetDistance < m_Data.m_AttackRange * 0.75)
                {
                    m_Nav.Stop();
                }
                else
                {
                    m_Nav.Resume();
                    m_Nav.SetDestination(m_target.position);
                }
            }

            float move = m_Nav.desiredVelocity.magnitude;
            m_Ani.SetFloat("Speed", move);

            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator FindTarget()
    {
        while (true)
        {
            m_target = m_Interaction.GetTarget(this.transform);
            yield return new WaitForSeconds(5f);
        }
    }


    IEnumerator DeathCheck()
    {
        while (true)
        {
            if (m_Data.m_Hp <= 0)
            {
                if (m_Data.m_Death != true)
                {
                    m_Data.m_Death = true;
                    m_CapsuleCol.enabled = false;
                    m_Nav.enabled = false;
                    m_Ani.SetBool("DeathBool", m_Data.m_Death);
                    m_Ani.SetTrigger("DeathTrigger");
                    /*m_ObjMgr.GiveMoney(0, m_Price);*/
                }
                if (m_Data.m_DeathTimer < 4f)
                    m_Data.m_DeathTimer += Time.deltaTime;
                else
                    this.gameObject.SetActive(false);

                StopCoroutine("FindTarget");
                StopCoroutine("TargetAttack");
                StopCoroutine("NavMove");
            }

            yield return null;
        }
    }
}
