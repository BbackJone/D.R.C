using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;   //this for NavMeshAgent

public class DevilZombieAI : MonoBehaviour
{
    private ZombieData m_Data;
    private ZombieInteraction m_Interaction;
    private Animator m_Ani;
    private NavMeshAgent m_Nav;     //for finding route or move zombie

    public GameObject obj_body;
    public GameObject obj_head;
    private BoxCollider body_col;
    private BoxCollider head_col;

    public Transform m_target { get; set; }
    public float m_TargetDistance { get; set; }

    void Awake()
    {
        m_Nav = GetComponent<NavMeshAgent>();
        m_Data = GetComponent<ZombieData>();
        m_Interaction = GetComponent<ZombieInteraction>();
        m_Ani = GetComponent<Animator>();

        body_col = obj_body.GetComponent<BoxCollider>();
        head_col = obj_head.GetComponent<BoxCollider>();
    }

    void OnEnable()
    {
        StageMgr.instance.AddSpecialZombieNumber(1);

        m_Nav.baseOffset = 0;
        m_Nav.enabled = true;
        m_Nav.baseOffset = Random.Range(15,26);

        StartCoroutine("FindTarget");
        StartCoroutine("TargetAttack");
        StartCoroutine("NavMove");
        StartCoroutine("DeathCheck");
    }

    // Use this for initialization
    void Start()
    {
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
                        transform.LookAt(new Vector3(m_target.position.x,
                            transform.position.y,
                            m_target.position.z));
                        gameObject.SendMessage("ShootFlame");
                    }
                }
            }

            yield return null;
        }
    }

    //Move toward target
    IEnumerator NavMove()
    {
        while (true)
        {
            if (m_target)
            {
                if (m_TargetDistance < m_Data.m_AttackRange * 0.75)
                {
                    m_Nav.speed = 0;
                }
                else
                {
                    m_Nav.speed = m_Data.m_Speed;
                    m_Nav.SetDestination(m_target.position);
                }
            }

            float move = m_Nav.desiredVelocity.magnitude;
            m_Ani.SetFloat("Speed", move);

            yield return new WaitForSeconds(0.5f);
        }
    }

    //FindTarget per 5 seconds.
    private IEnumerator FindTarget()
    {
        while (true)
        {
            m_target = GetTarget();
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
                else
                {
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
}
