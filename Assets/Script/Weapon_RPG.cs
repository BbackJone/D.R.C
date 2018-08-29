using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon_RPG : Weapon
{
    private Camera m_Camera;
    public Animator m_GunAni;

    public Transform m_ShootPos;
    private int m_RaycastLayermask;       //Layer for raycast to ignore

    private RPGRocket RpgRocket = null;
    private Vector3 m_ShootDir;

    public override void Shoot()
    {
        if (m_AmmoBulletNum <= 0)
            return;

        if (RpgRocket != null) ShootRPG();
        else Debug.LogError("RPG: Failed to find rocket");
    }

    void Awake()
    {
        m_AimSystem = GetComponentInParent<AimSystem>();
        m_Camera = Camera.main;

        RpgRocket = transform.Find("SA_Wep_RPGLauncher_Rocket").GetComponent<RPGRocket>();

        Initialize();
    }

    private void OnEnable()
    {
        StartCoroutine("NarrowDownAim");
    }

    void Start()
    {
        m_AmmoBulletNum = 0;
        m_RaycastLayermask = ~((1 << 2) | (1 << 8)); //ignore second and eighth layer
    }

    private void Update()
    {
        RpgRocket.gameObject.SetActive(m_AmmoBulletNum != 0);
    }
    
    public void ShootRPG()
    {
        Vector3 Dir = m_ShootTarget.position - m_ShootPos.position;
        Dir = Dir / Dir.magnitude;

        var newRocket = ObjectPoolMgr.instance.CreatePooledObject("Rocket", m_ShootPos.transform.position,
            Quaternion.LookRotation(Dir)).GetComponent<RPGRocket>();
        newRocket.m_BodyDamage = m_BodyDamage;
        newRocket.m_HeadDamage = m_HeadDamage;
        newRocket.Fire();

        gameObject.SendMessage("PlaySound", 0);
        m_AmmoBulletNum -= 1;

        //SetAni
        m_IsShooting = true;
        m_GunAni.SetBool("Shoot_b", m_IsShooting);
        Invoke("SetIsShootingFalse", 0.1f);

        HandleRecoil();
    }

    public void SetIsShootingFalse()
    {
        m_IsShooting = false;
        m_GunAni.SetBool("Shoot_b", m_IsShooting);
    }
}
