using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RugbyZombieAction : MonoBehaviour {

    Animator m_Ani;
    ZombieData m_Data;
    RugbyZombieAI m_AI;

    float m_RushTimer = 0f;

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
        yield return new WaitForSeconds(_seconds);
        transform.LookAt(m_AI.m_target);
        m_Ani.SetTrigger("Rush");
        StartCoroutine("Rush");
    }

    IEnumerator Rush()
    {
        while(true)
        {
            if (m_RushTimer < 5f && !m_Data.m_Death)
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
        Debug.Log("StopRush");
        m_Ani.SetTrigger("AttackFinish");
        gameObject.SendMessage("AttackFinish");
        StopCoroutine("Rush");
    }
}
