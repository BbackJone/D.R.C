using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilZombieAction : MonoBehaviour {

    private Animator m_Ani;
    private DevilZombieAI m_AI;

    // Use this for initialization
    void Awake() {
        m_Ani = GetComponent<Animator>();
        m_AI = GetComponent<DevilZombieAI>();
    }

    // Update is called once per frame
    void Update() {

    }

    public void FlameAttack()
    {
        StartCoroutine("CoFlameAttack");
    }

    public IEnumerator CoFlameAttack()
    {
        for(int i = 0; i < 3; i++)
        {
            ShootFlame();
            yield return new WaitForSeconds(0.5f);
        }
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

    public void Patrol()
    {

    }
}
