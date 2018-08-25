using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public struct WeaponDB
{
    public string Name;
    public int MaxBullet;
    public float Recoil;
    public int Weight;
    public float Shotrate;
    public string Bulletsort;
    public bool Autoshot;
    public int BodyDamage;
    public int HeadDamage;
    public float ReloadTime;
    public bool EnabledByDefault;
}

public enum Weapon_Type
{
    KATANA,
    HANDGUN,
    RIFLE,
    RPG,
    SNIPER,
    BOW,
    MINIGUN,
    FLAMETHROWER
}

//Weapon_Code in animator parameter
// 0 : katana // 1 : handgun // 2 : rifle // 3 : Minigun // 4 : RPG // 5 : Sniper // 6 : Flame Thrower

public abstract class Weapon : MonoBehaviour{

    protected AimSystem m_AimSystem;        //please caching this variable at awake.

    public ObjType m_Type { get; set; }
    public string m_ObjName { get; set; }

    public int m_MaxBulletNum { get; set; }        //한번에장전 할수있는 최대탄수
    public int m_AmmoBulletNum { get; set; }        //탄창에 남아있는 총알수
    public string m_BulletSort { get; set; }        //총알의 종류
    public float m_Recoil { get; set; }             //반동
    public float m_Weight { get; set; }             //중량
    public float m_ShotRate { get; set; }           //연사속도
    public bool m_Autoshot { get; set; }            //자동발사여부
    public int m_BodyDamage { get; set; }
    public int m_HeadDamage { get; set; }
    public float m_ReloadTime { get; set; }         //재장전 시간
    public bool m_EnabledByDefault { get; set; }
    public Weapon_Type m_WeaponType;

    public Transform m_ShootTarget { get; set; }
    public float m_StackedRecoil { get; set; }
    public float m_MaxRecoil = 5f;

    public Transform m_GrabPosRight;
    public Transform m_GrabPosLeft;

    public bool m_WeaponEnabled;

    //state(for animation)
    public bool m_IsShooting;

    public void Initialize()
    {
        WeaponDB DBData = ObjectManager.m_Inst.m_DBMgr.m_WeaponDB[gameObject.name];
        m_ObjName = DBData.Name;
        m_MaxBulletNum = DBData.MaxBullet;
        m_Recoil = DBData.Recoil;
        m_Weight = DBData.Weight;
        m_ShotRate = DBData.Shotrate;
        m_BulletSort = DBData.Bulletsort;
        m_Autoshot = DBData.Autoshot;
        m_BodyDamage = DBData.BodyDamage;
        m_HeadDamage = DBData.HeadDamage;
        m_ReloadTime = DBData.ReloadTime;
        m_EnabledByDefault = DBData.EnabledByDefault;

        m_WeaponEnabled = (m_EnabledByDefault ? true : (PlayerPrefs.GetInt("WeaponPurchased_" + m_ObjName, 0) == 1 ? true : false));

        m_Type = ObjType.OBJ_WEAPON;
    }

    public void ObjListAdd()
    {
        ObjectManager.m_Inst.Objects.m_Weaponlist.Add(this);
    }

    public void ChargeBullet()
    {
        m_AmmoBulletNum = m_MaxBulletNum;
    }

    public abstract void Shoot();

    public IEnumerator NarrowDownAim()
    {
        while(true)
        {
            m_StackedRecoil = Mathf.Max(0, m_StackedRecoil - (7 * Time.deltaTime));
            
            yield return null;
        }
    }

    public void HandleRecoil()
    {
        m_StackedRecoil += m_Recoil;
        m_StackedRecoil = Mathf.Min(m_MaxRecoil, m_StackedRecoil);        //Max Recoil : 5.0f
        m_AimSystem.RecoilUpward(m_Recoil);
    }
}