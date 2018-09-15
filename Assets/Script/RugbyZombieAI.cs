using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RugbyZombieAI : MonoBehaviour {

    private NavMeshAgent m_Nav;     //for finding route or move zombie
    private ZombieData m_Data;
    private Animator m_Ani;

    public Transform m_target { get; set; }

    public GameObject obj_body;
    public GameObject obj_head;
    private BoxCollider body_col;
    private BoxCollider head_col;

    private float m_TargetDistance;
    public bool m_Attacking { get; set; }

    private void Awake()
    {
        m_Data = GetComponent<ZombieData>();
        m_Nav = GetComponent<NavMeshAgent>();
        m_Ani = GetComponent<Animator>();

        body_col = obj_body.GetComponent<BoxCollider>();
        head_col = obj_head.GetComponent<BoxCollider>();
        
        m_Attacking = false;
    }

    void OnEnable()
    {
        StageMgr.instance.AddSpecialZombieNumber(1);

        m_Nav.enabled = true;

        m_target = ObjectManager.m_Inst.m_Player.transform;
        StartCoroutine("NavMove");

        m_Data.m_AttackTimer = m_Data.m_AttackSpeed;
    }

    private void Start()
    {
        m_Nav.speed = 2f;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(m_Data.m_Death == false)
            AttackTarget();
        DeathCheck();
    }

    void AttackTarget()
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
                else if(m_Attacking == false)
                {
                    m_Data.m_AttackTimer = 0f;
                    transform.LookAt(m_target);
                    gameObject.SendMessage("WaitAndRush");
                    m_Attacking = true;
                    m_Ani.SetBool("Attacking", m_Attacking);
                }
            }
        }
        else
        {
            m_target = ObjectManager.m_Inst.m_Player.transform;
            AttackTarget();
        }
    }

    IEnumerator NavMove()
    {
        while (true)
        {
            if (m_target)
            {
                if (m_TargetDistance < m_Data.m_AttackRange)
                {
                    m_Nav.isStopped = true;

                    float move = m_Nav.desiredVelocity.magnitude;
                    m_Ani.SetFloat("Speed", move);
                }
                else if (m_Attacking == false)
                {
                    m_Nav.isStopped = false;
                    m_Nav.SetDestination(m_target.position);

                    float move = m_Nav.desiredVelocity.magnitude;
                    m_Ani.SetFloat("Speed", move);
                }
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    void DeathCheck()
    {
        if(m_Data.m_Hp <= 0)
        {
            if (m_Data.m_Death != true)
            {
                m_Data.m_Death = true;
                m_Nav.enabled = false;
                m_Ani.SetBool("DeathBool", m_Data.m_Death);
                m_Ani.SetTrigger("DeathTrigger");
                body_col.enabled = false;
                head_col.enabled = false;
                StopCoroutine("FindTarget");
                StopCoroutine("NavMove");
            }

            if (m_Data.m_DeathTimer < 4f)
                m_Data.m_DeathTimer += Time.deltaTime;
            else
            {
                body_col.enabled = true;
                head_col.enabled = true;
                //Because SetActive(false) with colliders inactive make a kind of bug(collider components are out of order), this is required

                this.gameObject.SetActive(false);
                StageMgr.instance.AddSpecialZombieNumber(-1);
            }
        }
    }
}
