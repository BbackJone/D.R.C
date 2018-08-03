using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Weapon_Code in animator parameter
// 0 : katana, 1 : handgun, 2 : rifle
public class Weapon_RPG : Weapon
{
    private Camera m_Camera;
    
    private int m_RaycastLayermask;       //Layer for raycast to ignore

    private RPGRocket RpgRocket = null;
    private Vector3 Dir;
   
    void Awake()
    {
        //m_Light = transform.GetChild(1).GetComponent<Animator>();
        //m_MuzzleFlash = transform.GetChild(0).GetComponent<Animator>();
        //m_MuzzleFlash2 = transform.GetChild(2).GetComponent<Animator>();
        m_Camera = Camera.main;

        RpgRocket = transform.Find("SA_Wep_RPGLauncher_Rocket").GetComponent<RPGRocket>();

        Initialize();
    }

    void Start()
    {
        ObjListAdd();
        m_AmmoBulletNum = 0;
        m_RaycastLayermask = ~((1 << 2) | (1 << 8)); //ignore second and eighth layer
    }

    void OnEnable()
    {
        m_MuzzleFlash.enabled = false;
        m_MuzzleFlash2.enabled = false;
    }

    private void Update()
    {
        RpgRocket.gameObject.SetActive(m_AmmoBulletNum != 0);
    }
    
    public override void Shoot()
    {
        if (m_AmmoBulletNum <= 0)
            return;

        if (RpgRocket != null) ShootRPG();
        else Debug.LogError("RPG: Failed to find rocket");
    }

    public void ShootRPG()
    {
        var newRocket = ObjectPoolMgr.instance.CreatePooledObject("Rocket", RpgRocket.transform.position, m_Camera.transform.rotation).GetComponent<RPGRocket>();
        newRocket.m_BodyDamage = m_BodyDamage;
        newRocket.m_HeadDamage = m_HeadDamage;
        newRocket.Fire();
        m_AmmoBulletNum -= 1;
        return;
    }
}
