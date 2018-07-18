using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RugbyZombieAI : MonoBehaviour {

    private NavMeshAgent m_Nav;     //for finding route or move zombie

    private Transform m_target;


	// Use this for initialization
	void Start () {
		
	}

    void OnEnable()
    {
        StageMgr.instance.AddNormalZombieNumber(1);

        m_Nav.enabled = true;

        StartCoroutine("FindTarget");
        StartCoroutine("TargetAttack");
        StartCoroutine("NavMove");
        StartCoroutine("DeathCheck");
    }

    // Update is called once per frame
    void Update () {
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
    }

    public Transform GetTarget(Transform _trans)
    {
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
            m_target = GetTarget(this.transform);
            yield return new WaitForSeconds(5f);
        }
    }
}
