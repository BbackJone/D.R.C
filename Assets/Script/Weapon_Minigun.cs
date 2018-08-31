using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Minigun : Weapon {

    //this makes spark effect when bullet is fired.
    public MeshRenderer m_MuzzleFlash;
    public MeshRenderer m_MuzzleFlash2;

    public Transform m_ShootPos;

    public Animator m_GunAni;
    private Animator m_GunRotateAni;

    private float m_PreShootRotateTime = 0f;

    private GameObject soundObject;
    public float RecoilMultiplyer = 1;

    public override void Shoot()
    {
        if (m_AmmoBulletNum <= 0)
            return;

        CancelInvoke("CancelShoot");
        Invoke("CancelShoot", m_ShotRate * 3);
        if (m_PreShootRotateTime == 0f) {
            soundObject.SendMessage("PlaySound", 12);
}
        if (m_PreShootRotateTime < 0.5f)
        {
            m_GunRotateAni.SetBool("IsRotating", true);
            m_PreShootRotateTime += m_ShotRate;

            return;
        }

        ShootBullet();
    }

    void Awake()
    {
        soundObject = ObjectManager.m_Inst.m_Player.gameObject;

        m_AimSystem = GetComponentInParent<AimSystem>();
        m_GunRotateAni = GetComponent<Animator>();

        Initialize();
    }

    void OnEnable()
    {
        m_MuzzleFlash.enabled = false;
        m_MuzzleFlash2.enabled = false;

        m_StackedRecoil = 0;
        StartCoroutine("NarrowDownAim");
    }

    void Start()
    {
        m_AmmoBulletNum = 0;

        m_AmmoBulletNum = m_MaxBulletNum;
    }

    public void ShootBullet()
    {
        //Get Bullet Direction
        Vector3 Dir = m_ShootTarget.position - m_ShootPos.position;
        Dir = Dir / Dir.magnitude;

        GameObject bullet = ObjectPoolMgr.instance.CreatePooledObject(m_BulletSort, m_ShootPos.position, GetRecoiledDirection(Dir));
        bullet.SendMessage("SetBodyDamage", m_BodyDamage);
        bullet.SendMessage("SetHeadDamage", m_HeadDamage);

        m_AmmoBulletNum -= 1;
        soundObject.SendMessage("PlaySound", 9);
        Makeflash();

        //SetAni
        m_IsShooting = true;
        m_GunAni.SetBool("Shoot_b", m_IsShooting);
        Invoke("SetIsShootingFalse", 0.1f);

        HandleRecoil();
    }

    public void Makeflash()
    {
        m_MuzzleFlash.transform.eulerAngles += new Vector3(Random.Range(0, 90), Random.Range(0, 90), Random.Range(0, 90));
        m_MuzzleFlash2.transform.eulerAngles += new Vector3(Random.Range(0, 90), Random.Range(0, 90), Random.Range(0, 90));

        m_MuzzleFlash.enabled = true;
        m_MuzzleFlash2.enabled = true;

        Invoke("Cancelflash", 0.05f);
    }

    public void Cancelflash()
    {
        m_MuzzleFlash.enabled = false;
        m_MuzzleFlash2.enabled = false;
    }

    public void SetIsShootingFalse()
    {
        m_IsShooting = false;
        m_GunAni.SetBool("Shoot_b", m_IsShooting);
    }

    public void CancelShoot()
    {
        m_PreShootRotateTime = 0f;
        m_GunRotateAni.SetBool("IsRotating", false);
    }

    public Quaternion GetRecoiledDirection(Vector3 _dirVector)
    {
        //rotate dir vector at right axis with random angle.
        Vector3 RightTempVec = Quaternion.AngleAxis(Random.Range(-m_StackedRecoil, m_StackedRecoil), m_ShootPos.right) * _dirVector;

        //rotate RightTempVec at up axis with random angle.
        Vector3 retVector = Quaternion.AngleAxis(Random.Range(-m_StackedRecoil, m_StackedRecoil), m_ShootPos.up) * RightTempVec;

        //return (UpRot * RightRot);
        return (Quaternion.LookRotation(retVector));
    }
}
