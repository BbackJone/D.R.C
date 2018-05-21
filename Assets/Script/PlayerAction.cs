using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour {

    private PlayerData m_Data;
    private float m_AttackTimer;

    public bool m_Death { get; set; }
    public float m_DeathTimer { get; set; }

    public MyButton m_AttackButton;


	// Use this for initialization
    void Awake()
    {
        m_Data = GetComponent<PlayerData>();
    }

	void Start () {
        m_Death = false;
        m_DeathTimer = 0f;
        m_AttackTimer = 0f;

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
                    m_Data.m_Active = false;
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
            transform.Translate(Vector3.right * m_Data.m_Move.x * Time.deltaTime * m_Data.m_Speed);

            m_Data.m_Ani.SetFloat("Speed", m_Data.m_Move.z);
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


    void Firebullet()
    {
        //FireBullet per Shotrate if bullet exist
        if (m_AttackTimer >= m_Data.m_WeaponInhand.m_ShotRate
            && m_Data.m_WeaponInhand.m_AmmoBulletNum > 0
            || m_Data.m_WeaponInhand.m_ObjName == "Katana")
        {
            m_AttackTimer = 0f;
            m_Data.m_Ani.SetTrigger(m_Data.m_WeaponInhand.m_AniTrigger);
        }

        //Reload when there is no bullet
        if (m_Data.m_WeaponInhand.m_AmmoBulletNum <= 0)
        {
            if (!m_Data.m_Reloading)
            {
                Reload();
            }
            return;
        }
    }

    void SwapWeapon()
    {
        for(int i = 0; i < m_Data.m_Weapons.Count; i++)
        {
            Weapon _weap = m_Data.m_Weapons[i];
            if (_weap.gameObject.activeInHierarchy)
            {
                m_Data.m_Ani.SetTrigger("WeaponSwap");
                m_Data.m_WeaponInhand.gameObject.SetActive(false);
                if (i == m_Data.m_Weapons.Count - 1)
                {
                    m_Data.m_WeaponInhand = m_Data.m_Weapons[0];
                }
                else
                {
                    m_Data.m_WeaponInhand = m_Data.m_Weapons[i + 1];
                }
                m_Data.m_WeaponInhand.gameObject.SetActive(true);
                Check_WeaponisAuto();

                if (m_Data.m_Reloading == true)
                {
                    m_Data.m_Reloading = false;
                    m_Data.m_Ani.SetBool("WeaponReloadBool", false);
                }

                break;
            }
        }
    }

    void Check_WeaponisAuto()
    {
        if (m_Data.m_WeaponInhand.m_Autoshot)
        {
            m_AttackButton.Change_Continuous_Attrib(true);
        }
        else if (!m_Data.m_WeaponInhand.m_Autoshot)
        {
            m_AttackButton.Change_Continuous_Attrib(false);
        }
    }

    void Reload()
    {
        if (m_Data.m_WeaponInhand.m_AmmoBulletNum < m_Data.m_WeaponInhand.m_MaxBulletNum)
        {
            m_Data.m_Ani.SetTrigger("WeaponReload");
            m_Data.m_Reloading = true;
            m_Data.m_Ani.SetBool("WeaponReloadBool", m_Data.m_Reloading);
        }
    }

    void ChargeBullet_Inweapon()
    {
        if (m_Data.m_WeaponInhand)
        {
            m_Data.m_WeaponInhand.ChargeBullet();
            m_Data.m_Reloading = false;
            m_Data.m_Ani.SetBool("WeaponReloadBool", false);
        }
    }
}