using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RUGBY_ATTACK_STATE
{
    WAIT,
    RUSH
}

public class RugbyZombieAction : MonoBehaviour {

    Animator m_Ani;
    ZombieData m_Data;
    RugbyZombieAI m_AI;

    float m_RushTimer = 0f;
    public RUGBY_ATTACK_STATE m_State { get; set; }

    private void Awake()
    {
        m_Ani = GetComponent<Animator>();
        m_Data = GetComponent<ZombieData>();
        m_AI = GetComponent<RugbyZombieAI>();
    }

    private void Start()
    {
        m_Data.m_AttackSpeed = 1f;
    }

    void WaitAndRush()
    {
        StartCoroutine("WaitforsecondsAndRush", 5f);
    }

    IEnumerator WaitforsecondsAndRush(float _seconds)
    {
        m_Ani.SetTrigger("Waiting");
        m_State = RUGBY_ATTACK_STATE.WAIT;
        yield return new WaitForSeconds(_seconds);
        transform.LookAt(m_AI.m_target);
        m_Ani.SetTrigger("Rush");
        StartCoroutine("Rush");
        m_State = RUGBY_ATTACK_STATE.RUSH;
    }

    IEnumerator Rush()
    {
        while(true)
        {
            if (m_RushTimer < 2f && !m_Data.m_Death)
            {
                m_RushTimer += Time.deltaTime;
                transform.Translate(Vector3.forward * Time.deltaTime * m_Data.m_Speed * 5);
            }
            else
            {
                m_RushTimer = 0f;
                StopRush();
            }
            
            yield return null;
        }
    }

    void StopRush()
    {
        m_AI.m_Attacking = false;
        m_Ani.SetBool("Attacking", m_AI.m_Attacking);
        StopCoroutine("Rush");
        m_State = RUGBY_ATTACK_STATE.WAIT;
    }
}
