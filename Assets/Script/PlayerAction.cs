using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour {

    private PlayerData m_Data;
    private float m_AttackTimer;
    private CameraMove m_CameraMove;

    public bool m_Death { get; set; }
    public float m_DeathTimer { get; set; }     //time between player's death and player's extinction.

    public MyButton m_AttackButton;     //This is necessary to determine 
                                        //whether you will allow automatic firing of weapons.

    public Transform left_leg;
    public Transform right_leg;

	// Use this for initialization
    void Awake()
    {
        m_Data = GetComponent<PlayerData>();
        m_CameraMove = Camera.main.GetComponent<CameraMove>();
    }

	void Start () {
        m_Death = false;
        m_DeathTimer = 0f;
        m_AttackTimer = 0f;

        if (gameObject.name == "Santa")
        {
            var gameController = GameObject.Find("GameController");
            if (gameController != null) gameController.GetComponent<SantaPositionPreserver>().LoadSantaPos(gameObject);
        }

        StartCoroutine("DeadCheck");
        StartCoroutine("CountTime");
        StartCoroutine("Playermove");
	}

    void OnEnable()
    {
        m_Death = false;
        m_DeathTimer = 0f;

        StartCoroutine("DeadCheck");
        StartCoroutine("CountTime");
        StartCoroutine("Playermove");
    }

    IEnumerator DeadCheck()
    {
        while (true)
        {
            if (m_Data.m_Hp <= 0)
            {
                if (m_Death != true)
                {
                    m_Death = true;
                    m_Data.m_Ani.SetBool("DeathBool", m_Death);
                    m_Data.m_Ani.SetTrigger("DeathTrigger");

                    StopCoroutine("MoveControl");
                    StopCoroutine("Getkey");
                }

                if (m_DeathTimer < 2f)
                    m_DeathTimer += Time.deltaTime;
                else
                {
                    this.gameObject.SetActive(false);
                }

            }

            yield return null;
        }
    }

    IEnumerator CountTime()
    {
        while (true)
        {
            if (m_AttackTimer < 10f)
                m_AttackTimer += Time.deltaTime;

            yield return null;
        }
    }

    IEnumerator Playermove()
    {
        while (true)
        {
            transform.Translate(Vector3.forward * m_Data.m_Move.z * Time.deltaTime * m_Data.m_Speed);
            transform.Translate(Vector3.right * m_Data.m_Move.x * Time.deltaTime * m_Data.m_Speed * 0.5f);

            m_Data.m_Ani.SetFloat("Speed_Horizontal", m_Data.m_Move.x);
            m_Data.m_Ani.SetFloat("Speed_Vertical", m_Data.m_Move.z);
            yield return null;
        }
    }

    public void Shoot()
    {
        if (m_Data.m_WeaponInhand)
        {
            m_Data.m_WeaponInhand.Shoot();
        }
    }


    public void Firebullet()
    {
        //Reload when there is no bullet
        if (m_Data.m_WeaponInhand.m_AmmoBulletNum <= 0)
        {
            if (!m_Data.m_isReloading)
            {
                Reload();
            }
            return;
        }

        //FireBullet per Shotrate if bullet exist
        if (m_AttackTimer >= m_Data.m_WeaponInhand.m_ShotRate
            && !m_Data.m_isReloading
            && m_Data.m_WeaponInhand.m_AmmoBulletNum > 0
            || m_Data.m_WeaponInhand.m_ObjName == "Katana") //(katana doesn't have buttlet)
        {
            m_AttackTimer = 0f;
            //m_Data.m_Ani.SetTrigger(m_Data.m_WeaponInhand.m_AniTrigger);
            m_Data.m_isShooting = true;
            m_Data.m_Ani.SetBool("Shoot_b", m_Data.m_isShooting);
            Invoke("SetShootingFalse", 0.05f);
            Shoot();
        }
    }

    public void SwapWeapon()
    {
        m_Data.m_Ani.SetBool("Minigun_Attack_Bool", false);
        for (int i = 0; i < m_Data.m_Weapons.Count; i++)
        {
            Weapon _weap = m_Data.m_Weapons[i];
            if (_weap.gameObject.activeInHierarchy)
            {
                m_Data.m_Ani.SetTrigger("WeaponSwap");
                m_Data.m_WeaponInhand.gameObject.SetActive(false);

                if (i == m_Data.m_Weapons.Count - 1)
                {
                    m_Data.m_WeaponInhand = m_Data.m_Weapons[0];
                    m_Data.m_Ani.SetInteger("Weapon_Code", 0);
                }
                else
                {
                    m_Data.m_WeaponInhand = m_Data.m_Weapons[i + 1];
                    m_Data.m_Ani.SetInteger("Weapon_Code", i + 1);
                }
                m_Data.m_WeaponInhand.gameObject.SetActive(true);
                Check_WeaponisAuto();

                //If the weapon is sniper, move camera position
                if(m_Data.m_WeaponInhand.m_ObjName == "Sniper")
                    m_CameraMove.CameraLerp(CAMERAPOS.SNIPER_SHOOTPOS);
                else
                    m_CameraMove.CameraLerp(CAMERAPOS.NORMALPOS);

                if (m_Data.m_isReloading == true)
                {
                    m_Data.m_isReloading = false;
                    m_Data.m_Ani.SetBool("WeaponReloadBool", false);
                }

                break;
            }
        }
    }

    //if weapon allows auto firing, change the property of attack button to "continuous"
    void Check_WeaponisAuto()
    {
        if (m_Data.m_WeaponInhand.m_Autoshot)
        {
            //m_AttackButton.Change_Continuous_Attrib(true);
        }
        else if (!m_Data.m_WeaponInhand.m_Autoshot)
        {
            //m_AttackButton.Change_Continuous_Attrib(false);
        }
    }

    public void Reload()
    {
        if (m_Data.m_WeaponInhand.m_AmmoBulletNum < m_Data.m_WeaponInhand.m_MaxBulletNum)
        {
            //m_Data.m_Ani.SetTrigger("WeaponReload");
            m_Data.m_isReloading = true;
            m_Data.m_Ani.SetBool("Reload_b", m_Data.m_isReloading);
            Invoke("SetRelaodingFalse", 2f);         //DB에 리로딩 시간 적어넣을것
            m_Data.m_Ani.SetBool("Minigun_Attack_Bool", false);
        }
    }

    //When reload motion is end, charge bullet in weapon.
    void ChargeBullet_Inweapon()
    {
        if (m_Data.m_WeaponInhand)
        {
            m_Data.m_WeaponInhand.ChargeBullet();
            m_Data.m_isReloading = false;
            m_Data.m_Ani.SetBool("WeaponReloadBool", false);
        }
    }

    void SetShootingFalse()
    {
        m_Data.m_isShooting = false;
        m_Data.m_Ani.SetBool("Shoot_b", m_Data.m_isShooting);
    }

    void SetRelaodingFalse()
    {
        m_Data.m_isReloading = false;
        m_Data.m_Ani.SetBool("Reload_b", m_Data.m_isReloading);
    }
}