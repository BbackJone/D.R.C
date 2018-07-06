using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Katana : Weapon {

    private string m_ParticleName;

	// Use this for initialization
    void Awake(){
        m_ObjMgr = GameObject.FindGameObjectWithTag("GameController").GetComponent<ObjectManager>();
        Initialize();
    }

	void Start () {
        m_ObjName = "Katana";
        m_ParticleName = "FX_BloodSplatter_Katana";

        ObjListAdd();
    }


    public override void Initialize()
    {
        WeaponDB DBData = m_ObjMgr.m_DBMgr.m_WeaponDB[gameObject.name];
        m_ObjName = DBData.Name;
        m_MaxBulletNum = DBData.MaxBullet;
        m_Recoil = DBData.Recoil;
        m_Weight = DBData.Weight;
        m_ShotRate = DBData.Shotrate;
        m_BulletSort = DBData.Bulletsort;
        m_Autoshot = DBData.Autoshot;
        m_AniTrigger = DBData.AniTrigger;
        m_BodyDamage = DBData.BodyDamage;
        m_HeadDamage = DBData.HeadDamage;

        m_Type = ObjType.OBJ_WEAPON;
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


    override public void ObjListAdd()
    {
        m_ObjMgr.Objects.m_Weaponlist.Add(this);
    }
}
