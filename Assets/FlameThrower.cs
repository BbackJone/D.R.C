using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : Weapon {

    private GameObject m_FlameParticle;

    private void Awake()
    {
        Initialize();

        m_FlameParticle = transform.GetChild(0).gameObject;
        m_FlameParticle.SetActive(false);
    }

    private void OnEnable()
    {
        m_StackedRecoil = 0;
        StartCoroutine("NarrowDownAim");
    }

    // Use this for initialization
    void Start () {
        ObjListAdd();
        m_AmmoBulletNum = m_MaxBulletNum;
    }
	
    public override void Shoot()
    {
        Debug.Log("Flame Shoot");
        if(m_AmmoBulletNum > 0)
        {
            StartCoroutine("ShootFlame");
        }
    }

    public IEnumerator ShootFlame()
    {
        while(true)
        {
            m_FlameParticle.SetActive(true);
            yield return null;
        }
    }
}
