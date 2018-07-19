using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RugbyZombieAI : MonoBehaviour {

    private NavMeshAgent m_Nav;     //for finding route or move zombie
    private ZombieData m_Data;
    private Animator m_Ani;

    private Transform m_target;

    public GameObject obj_body;
    public GameObject obj_head;
    private BoxCollider body_col;
    private BoxCollider head_col;

    private float m_TargetDistance;
    private bool m_Attacking = false;

    private void Awake()
    {
        m_Data = GetComponent<ZombieData>();
        m_Nav = GetComponent<NavMeshAgent>();
        m_Ani = GetComponent<Animator>();

        body_col = obj_body.GetComponent<BoxCollider>();
        head_col = obj_head.GetComponent<BoxCollider>();
    }

    // Use this for initialization
    void Start () {
        m_Nav.speed = m_Data.m_Speed;
    }

    void OnEnable()
    {
        StageMgr.instance.AddSpecialZombieNumber(1);

        m_Nav.enabled = true;

        StartCoroutine("FindTarget");
        StartCoroutine("NavMove");

        m_Data.m_AttackSpeed = 3;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
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
                }
            }
        }
    }

    public Transform GetTarget()
    {
        if (ObjectManager.m_Inst.Objects.m_Playerlist.Count <= 0)
            return null;

        float MinDis = 100000f;
        PlayerInteraction target = null;
        foreach (PlayerInteraction pm in ObjectManager.m_Inst.Objects.m_Playerlist)
        {
            if (pm == null) continue;
            float dis = Vector3.Distance(pm.transform.position, this.transform.position);
            if (MinDis > dis)
            {
                MinDis = dis;
                target = pm;
            }
        }
        return target.transform;
    }

    private IEnumerator FindTarget()
    {
        while (true)
        {
            m_target = GetTarget();
            yield return new WaitForSeconds(5f);
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
                    m_Nav.Stop();

                    float move = m_Nav.desiredVelocity.magnitude;
                    m_Ani.SetFloat("Speed", move);
                }
                else if (m_Attacking == false)
                {
                    m_Nav.Resume();
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

    void AttackFinish()
    {
        m_Attacking = false;
    }
}
