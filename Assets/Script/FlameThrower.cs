using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : Weapon {

    private ParticleSystem m_FlameParticleSystem;
    public AudioSource AudioSource;
    public AudioClip AudioClip;
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
            if(m_FlameParticleSystem.enableEmission == false)
            {
                m_FlameParticleSystem.enableEmission = true;
                AudioSource.PlayOneShot(AudioClip);
            }
            m_AmmoBulletNum -= 1;
            

            //If don't cancelInvoke in a few time, stop shoot function is exacuted.
            CancelInvoke("StopFire");
            Invoke("StopFire", m_ShotRate * 3);
        }
    }

    public void StopFire()
    {
        AudioSource.Stop();
        m_FlameParticleSystem.enableEmission = false;
    }

}
