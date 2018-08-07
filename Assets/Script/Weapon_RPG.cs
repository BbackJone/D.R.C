using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon_RPG : Weapon
{
    private Camera m_Camera;

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

    private void Update()
    {
        RpgRocket.gameObject.SetActive(m_AmmoBulletNum != 0);
    }
    
    public void ShootRPG()
    {
        Vector3 RayStartPos = m_Camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));   //middle point of screen
        RaycastHit hit; 
        if (Physics.Raycast(RayStartPos, m_Camera.transform.forward, out hit, 100f, m_RaycastLayermask))    //raycast forward
        {
            m_ShootDir = hit.point - m_ShootPos.position;
            m_ShootDir = m_ShootDir / m_ShootDir.magnitude;
        }
        else    //if there is no point where the ray hit, set destination point as moderate forward at camera.
        {
            m_ShootDir = (m_Camera.transform.position + m_Camera.transform.forward * 30f) - m_ShootPos.position;
        }

        var newRocket = ObjectPoolMgr.instance.CreatePooledObject("Rocket", m_ShootPos.transform.position, Quaternion.LookRotation(m_ShootDir)).GetComponent<RPGRocket>();
        newRocket.m_BodyDamage = m_BodyDamage;
        newRocket.m_HeadDamage = m_HeadDamage;
        newRocket.Fire();
        m_AmmoBulletNum -= 1;
        return;
    }
}
