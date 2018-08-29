using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilZombieAction : MonoBehaviour {

    private Animator m_Ani;
    private DevilZombieAI m_AI;
    private ZombieData m_Data;

    public Transform m_WonderingPosParent;
    private float m_PatrolTimer = 5f;

    // Use this for initialization
    void Awake() {
        m_Ani = GetComponent<Animator>();
        m_AI = GetComponent<DevilZombieAI>();
        m_Data = GetComponent<ZombieData>();
    }

    private void OnEnable()
    {
        m_PatrolTimer = 5f;
        StartCoroutine("CoPatrol");
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
    }

    public IEnumerator CoPatrol()
    {
        while(true)
        {
            m_PatrolTimer += Time.deltaTime;
            Vector3 DirectionMove = Vector3.zero;
            if (m_PatrolTimer >= 5f)
            {
                m_PatrolTimer = 0f;

                //Make a new position to patrol
                int PositionToGoIndex = Random.Range(0, m_WonderingPosParent.childCount - 1);
                Transform Position = m_WonderingPosParent.GetChild(PositionToGoIndex);

                if(Position)
                    DirectionMove = Position.position - transform.position;
            }

            transform.Translate(DirectionMove * Time.deltaTime * m_Data.m_Speed);

            yield return null;
        }
    }
}
