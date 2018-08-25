using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    //this makes spark effect when bullet is fired.
    public MeshRenderer m_MuzzleFlash;
    public MeshRenderer m_MuzzleFlash2;
    private float muzzleFlashMinimumSize = 0.05f;
    private float muzzleFlashMaximumSize = 0.225f;

    public Transform m_ShootPos;

    public Animator m_GunAni;

    public float RecoilMultiplyer = 1;

    public override void Shoot()
    {
        if (m_AmmoBulletNum <= 0)
            return;

        ShootBullet();
    }

    void Awake()
    {
        m_AimSystem = GetComponentInParent<AimSystem>();

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
        ObjListAdd();
        m_AmmoBulletNum = 0;

        m_AmmoBulletNum = m_MaxBulletNum;

        if (m_WeaponType == Weapon_Type.HANDGUN)
        {
            muzzleFlashMaximumSize = 0.125f;
        }
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
        gameObject.SendMessage("PlaySound", value: 0);
        Makeflash();

        //SetAni
        m_IsShooting = true;
        m_GunAni.SetBool("Shoot_b", m_IsShooting);
        Invoke("SetIsShootingFalse", 0.1f);

        HandleRecoil();
    }

    public void Makeflash()
    {
        //m_MuzzleFlash.transform.eulerAngles += new Vector3(Random.Range(0, 90), Random.Range(0, 90), Random.Range(0, 90));
        //m_MuzzleFlash2.transform.eulerAngles += new Vector3(Random.Range(0, 90), Random.Range(0, 90), Random.Range(0, 90));

        m_MuzzleFlash.transform.localRotation = Quaternion.Euler(Random.Range(0, 90), m_MuzzleFlash.transform.localRotation.eulerAngles.y, m_MuzzleFlash.transform.localRotation.eulerAngles.z);
        m_MuzzleFlash2.transform.localRotation = Quaternion.Euler(m_MuzzleFlash2.transform.localRotation.eulerAngles.x, Random.Range(0, 90), m_MuzzleFlash2.transform.localRotation.eulerAngles.z);

        m_MuzzleFlash.transform.localScale = new Vector3(Random.Range(muzzleFlashMinimumSize, muzzleFlashMaximumSize), 1f, Random.Range(muzzleFlashMinimumSize, muzzleFlashMaximumSize));
        m_MuzzleFlash2.transform.localScale = new Vector3(Random.Range(muzzleFlashMinimumSize, muzzleFlashMaximumSize), 1f, Random.Range(muzzleFlashMinimumSize, muzzleFlashMaximumSize));

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

