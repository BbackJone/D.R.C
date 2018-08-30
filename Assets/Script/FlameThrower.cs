using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : Weapon {

    private ParticleSystem m_FlameParticleSystem;

    private void Awake()
    {
        Initialize();

        m_FlameParticleSystem = transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        m_StackedRecoil = 0;
        m_FlameParticleSystem.enableEmission = false;
        StartCoroutine("NarrowDownAim");
    }

    // Use this for initialization
    void Start () {
        m_AmmoBulletNum = m_MaxBulletNum;
    }
	
    public override void Shoot()
    {
        if(m_AmmoBulletNum > 0)
        {
            m_FlameParticleSystem.enableEmission = true;

            m_AmmoBulletNum -= 1;
            gameObject.SendMessage("PlaySound", value: 0);

            //If don't cancelInvoke in a few time, stop shoot function is exacuted.
            CancelInvoke("StopFire");
            Invoke("StopFire", m_Shot
                
                * 3);
        }
    }

    public void StopFire()
    {
        m_FlameParticleSystem.enableEmission = false;
    }
}
