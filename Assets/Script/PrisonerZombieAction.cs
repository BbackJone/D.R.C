using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonerZombieAction : MonoBehaviour {

    private Animator m_Ani;

    // Use this for initialization
    void Awake() {
        m_Ani = GetComponent<Animator>();
    }

    void AttackofTwohand() {
        m_Ani.SetTrigger("Attack");
    }
}
