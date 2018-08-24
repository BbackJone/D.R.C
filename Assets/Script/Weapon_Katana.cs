using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Katana : Weapon
{
    private string m_ParticleName;
    private Animator m_PlayerAni;

    public override void Shoot()
    {

        m_PlayerAni.SetTrigger("KatanaAttack");

        gameObject.SendMessage("PlaySound", 0);
    }

    // Use this for initialization
    void Awake(){
        m_AimSystem = GetComponentInParent<AimSystem>();
        m_PlayerAni = GetComponentInParent<Animator>();
        Initialize();
    }

	void Start () {
        m_ObjName = "Katana";
        m_ParticleName = "FX_BloodSplatter_Katana";

        ObjListAdd();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.CompareTag("Enemy"))
        {
            Vector3 CollsionPoint = col.ClosestPointOnBounds(this.transform.position);
            int[] DamageSet = new int[2] { m_HeadDamage, m_BodyDamage };
            col.gameObject.SendMessage("GetDamage", DamageSet);
            ObjectPoolMgr.instance.CreatePooledObject(m_ParticleName, CollsionPoint, this.transform.rotation);
        }
    }
}
