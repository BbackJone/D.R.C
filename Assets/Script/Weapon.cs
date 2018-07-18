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
    public string AniTrigger;
    public int BodyDamage;
    public int HeadDamage;
}

public class Weapon : MonoBehaviour
{
    public ObjType m_Type { get; set; }
    public string m_ObjName { get; set; }

    public int m_MaxBulletNum { get; set; }        //한번에장전 할수있는 최대탄수
    public int m_AmmoBulletNum { get; set; }        //탄창에 남아있는 총알수
    public string m_BulletSort { get; set; }        //총알의 종류
    public float m_Recoil { get; set; }             //반동
    public float m_Weight { get; set; }             //중량
    public float m_ShotRate { get; set; }           //연사속도
    public bool m_Autoshot { get; set; }            //자동발사여부
    public string m_AniTrigger { get; set; }        //실행할 애니메이션을 string 형으로 불러옴.
    public int m_BodyDamage { get; set; }
    public int m_HeadDamage { get; set; }

    private Camera m_Camera;

    //this makes spark effect when bullet is fired.
    private Animator m_Light;
    private Animator m_MuzzleFlash;
    private Animator m_MuzzleFlash2;


    virtual public void Initialize()
    {
        WeaponDB DBData = ObjectManager.m_Inst.m_DBMgr.m_WeaponDB[gameObject.name];
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


    void Awake()
    {
        m_Light = transform.GetChild(1).GetComponent<Animator>();
        m_MuzzleFlash = transform.GetChild(0).GetComponent<Animator>();
        m_MuzzleFlash2 = transform.GetChild(2).GetComponent<Animator>();
        m_Camera = Camera.main;

        Initialize();
    }

    void Start()
    {
        ObjListAdd();
        m_AmmoBulletNum = 0;
    }

    void OnEnable()
    {
        if (m_Light != null)
        {
            m_Light.gameObject.SetActive(false);
            m_MuzzleFlash.gameObject.SetActive(false);
            m_MuzzleFlash2.gameObject.SetActive(false);
        }
    }


    virtual public void ObjListAdd()
    {
       ObjectManager.m_Inst.Objects.m_Weaponlist.Add(this);
    }


    public void Shoot()
    {
      
        if (m_AmmoBulletNum <= 0)
            return;
        Vector3 RayStartPos = m_Camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));   //middle point of screen
        RaycastHit hit;
        if (Physics.Raycast(RayStartPos, m_Camera.transform.forward, out hit, 100f))    //raycast forward
        {
            Vector3 Dir = hit.point - transform.position;
            Dir = Dir / Dir.magnitude;
            GameObject bullet = ObjectPoolMgr.instance.CreatePooledObject(m_BulletSort, transform.position, Quaternion.LookRotation(Dir));
            bullet.SendMessage("SetBodyDamage", m_BodyDamage);
            bullet.SendMessage("SetHeadDamage", m_HeadDamage);
        }
        else    //if there is no point where the ray hit, set destination point as moderate forward at camera.
        {
            float Updis = m_Camera.transform.position.y - transform.position.y;
            Vector3 Dir = (m_Camera.transform.position + m_Camera.transform.forward * 30f) - transform.position;
            ObjectPoolMgr.instance.CreatePooledObject(m_BulletSort, transform.position, Quaternion.LookRotation(Dir));
        }
        
        m_MuzzleFlash.SetTrigger("On");
        m_MuzzleFlash2.SetTrigger("On");
        m_Light.SetTrigger("On");
        m_AmmoBulletNum -= 1;
    }

    public void ChargeBullet()
    {
        m_AmmoBulletNum = m_MaxBulletNum;
    }
}
