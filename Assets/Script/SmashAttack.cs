using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashAttack : MonoBehaviour {

    private ZombieData m_Data;
    private RugbyZombieAction m_Action;
    private BoxCollider m_Col;

    private bool m_CollidOnceCheck = false;     //It prevent from damaging player twice

    private void Awake()
    {
        m_Data = gameObject.GetComponentInParent<ZombieData>();
        m_Action = gameObject.GetComponentInParent<RugbyZombieAction>();
        m_Col = GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {
        StartCoroutine("ColliderActiveCheck");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !m_Data.m_Death
            && m_CollidOnceCheck == false)
        {
            m_CollidOnceCheck = true;
            Invoke("ResetColliderOnceCheck", 1f);

            other.GetComponent<Rigidbody>().AddForce(transform.forward * 40, ForceMode.Impulse);

            other.gameObject.SendMessage("GetDamage", m_Data.m_AttackDamage);
        }
    }

    //check if collider should be active.
    public IEnumerator ColliderActiveCheck()
    {
        while(true)
        {
            if(m_Action.m_State == RUGBY_ATTACK_STATE.WAIT)
            {
                m_Col.enabled = false;
            }
            else
            {
                m_Col.enabled = true;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void ResetColliderOnceCheck()
    {
        m_CollidOnceCheck = false;
    }
}