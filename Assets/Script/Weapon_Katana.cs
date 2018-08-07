using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Katana : Weapon
{
    private string m_ParticleName;

    public override void Shoot()
    {
        //Shoot of katana is Implemented at animation(onehand attack)
    }

    // Use this for initialization
    void Awake(){
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
