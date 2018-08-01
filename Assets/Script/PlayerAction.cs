using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour {

    private PlayerData m_Data;
    private float m_AttackTimer;

    public bool m_Death { get; set; }
    public float m_DeathTimer { get; set; }     //time between player's death and player's extinction.

    public MyButton m_AttackButton;     //This is necessary to determine 
                                        //whether you will allow automatic firing of weapons.

    public Transform left_leg;
    public Transform right_leg;

    public Transform Look_target;

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

    private void Update()
    {
        left_leg.LookAt(Look_target);
        right_leg.LookAt(Look_target);
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


    public void Firebullet()
    {
        //FireBullet per Shotrate if bullet exist
        if (m_AttackTimer >= m_Data.m_WeaponInhand.m_ShotRate
            && m_Data.m_WeaponInhand.m_AmmoBulletNum > 0
            || m_Data.m_WeaponInhand.m_ObjName == "Katana") //(katana doesn't have buttlet)
        {
            m_AttackTimer = 0f;
            m_Data.m_Ani.SetTrigger(m_Data.m_WeaponInhand.m_AniTrigger);
            Shoot();
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

    public void SwapWeapon()
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
                    m_Data.m_Ani.SetInteger("Weapon_Code", 0);
                    gameObject.SendMessage("PlaySound", 0);

                }
                else
                {
                    m_Data.m_WeaponInhand = m_Data.m_Weapons[i + 1];
                    m_Data.m_Ani.SetInteger("Weapon_Code", i + 1);
                    gameObject.SendMessage("PlaySound", 0);

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
            m_Data.m_Ani.SetTrigger("WeaponReload");
            if (m_Data.m_WeaponInhand.m_WeaponType == Weapon_Type.RIFLE)
            {
                gameObject.SendMessage("PlaySound", (int)(SOUNDCLIP.RIFLERELOAD));
            }
            if (m_Data.m_WeaponInhand.m_WeaponType == Weapon_Type.HANDGUN)
            {
                gameObject.SendMessage("PlaySound", (int)(SOUNDCLIP.HANDGUNRELOAD));

            }

            m_Data.m_Reloading = true;
            m_Data.m_Ani.SetBool("WeaponReloadBool", m_Data.m_Reloading);
        }
    }

    //When reload motion is end, charge bullet in weapon.
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