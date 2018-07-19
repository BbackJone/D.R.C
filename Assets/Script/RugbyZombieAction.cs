using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RugbyZombieAction : MonoBehaviour {

    Animator m_Ani;

    private void Awake()
    {
        m_Ani = GetComponent<Animator>();
    }

    void WaitAndRush()
    {
        StartCoroutine("WaitforsecondsAndRush", 5f);
    }

    IEnumerator WaitforsecondsAndRush(float _seconds)
    {
        m_Ani.SetTrigger("Waiting");
        yield return new WaitForSeconds(_seconds);
        m_Ani.SetTrigger("Rush");
        Rush();
    }

    IEnumerator Rush()
    {
        yield return null;
    }

    void StopRush()
    {
        gameObject.SendMessage("AttackFinish");
    }
}
