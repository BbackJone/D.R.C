using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAction : MonoBehaviour {

    public enum SOUNDCLIP
    {
        SWAP,
        SANDWALK,
        ASPHALTWALK,
        RIFLERELOAD,
        HANDGUNRELOAD,
    }

    private PlayerData m_Data;
    private float m_AttackTimer;
    private CameraMove m_CameraMove;
    private AimIK m_AimIK;
    private Animator m_Ani;

    public bool m_Death { get; set; }
    public float m_DeathTimer { get; set; }     //time between player's death and player's extinction.

    public MyButton m_AttackButton;     //This is necessary to determine 
                                        //whether you will allow automatic firing of weapons.
    public bool m_FireOnceBool = false;

    public Transform left_leg;
    public Transform right_leg;

    private UnityEngine.UI.Image curtain;
    private float curtainAlpha;

	// Use this for initialization
    void Awake()
    {
        m_Data = GetComponent<PlayerData>();
        m_CameraMove = Camera.main.GetComponent<CameraMove>();
        m_AimIK = GetComponent<AimIK>();
        m_Ani = GetComponent<Animator>();

        m_Data.m_MaxHp = 30;
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

        curtain = GameObject.Find("FadeCurtain").GetComponent<UnityEngine.UI.Image>();
        curtainAlpha = 0f;
	}
    

    void OnEnable()
    {
        m_Death = false;
        m_Ani.SetBool("DeathBool", m_Death);
        m_DeathTimer = 0f;
        var sdm = GameObject.Find("SaveDataManager");
        if (sdm != null)
        {
            if (sdm.GetComponent<SaveDataManager>().currentSaveData != null) m_Data.m_Hp = sdm.GetComponent<SaveDataManager>().currentSaveData.health;
            else m_Data.m_Hp = m_Data.m_MaxHp;
        }
        else
        {
            m_Data.m_Hp = m_Data.m_MaxHp;
        }

        StartCoroutine("DeadCheck");
        StartCoroutine("CountTime");
        StartCoroutine("Playermove");
        StartCoroutine("CheckIsShooting");
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
                    m_Ani.SetLayerWeight(1, 0f);
                    m_AimIK.enabled = false;
                    m_Ani.SetBool("DeathBool", m_Death);
                    m_Ani.SetTrigger("DeathTrigger");

                    StopCoroutine("Playermove");
                    StopCoroutine("CheckIsShooting");

                    var stageMgr = GameObject.Find("StageMgr").GetComponent<StageMgr>();

                    var rsc = GameObject.Find("ResultScoreContainer");
                    if (rsc != null) {
                        var rscs = rsc.GetComponent<ResultScoreContainerScript>();
                        rscs.SetResultsAndStopTime(
                            rscs.kills,
                            (stageMgr != null ? stageMgr.m_CurrentWave.Level : 0),
                            (stageMgr.m_CurrentWave.Level * 10) + rscs.kills,
                            false);
                    }
                }

                if (m_DeathTimer < 2f)
                    m_DeathTimer += Time.deltaTime;
                else
                {
                    curtain.enabled = true;
                    curtainAlpha += Time.deltaTime;
                    if (curtainAlpha > 1f) {
                        curtainAlpha = 1f;
                        curtain.color = new Color(0f, 0f, 0f, 1f);
                        SceneManager.LoadScene("Result");
                    }
                    curtain.color = new Color(0f, 0f, 0f, curtainAlpha);
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

            m_Ani.SetFloat("Speed_Horizontal", m_Data.m_Move.x);
            m_Ani.SetFloat("Speed_Vertical", m_Data.m_Move.z);
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
        if (m_Ani.GetBool("IsReloading"))
            return;

        //Reload when there is no bullet
        if (m_Data.m_WeaponInhand.m_AmmoBulletNum <= 0
            && m_Data.m_WeaponInhand.m_ObjName != "Katana")
        {
            if (!m_Data.m_isReloading)
            {
                Reload();
            }
            return;
        }

        //FireBullet per Shotrate if bullet exist
        if (m_AttackTimer >= m_Data.m_WeaponInhand.m_ShotRate
            && !m_Data.m_isSwaping && !m_Data.m_isReloading) //(katana doesn't have buttlet)
        {
            m_AttackTimer = 0f;
            Shoot();
        }
    }

    public void SwapWeapon()
    {
        m_Ani.SetBool("Minigun_Attack_Bool", false);

        for (int i = 0; i < m_Data.m_Weapons.Count; i++)
        {
            //find Weapon in hand in weapon list
            Weapon _weap = m_Data.m_Weapons[i];
            if (_weap.gameObject.activeInHierarchy)
            {
                m_Ani.SetTrigger("WeaponSwap");

                m_Data.m_WeaponInhand.gameObject.SetActive(false);

                //if index of weapon in hand is last index of weapon list, change current weapon to weapon with code 0.
                if (i == m_Data.m_Weapons.Count - 1)
                {
                    m_Data.m_WeaponInhand = m_Data.m_Weapons[0];

                    m_Ani.SetInteger("Weapon_Code", 0);
                    gameObject.SendMessage("PlaySound", (int)(SOUNDCLIP.SWAP));

                }
                //change current weapon to weapon with next code.
                else
                {
                    m_Data.m_WeaponInhand = m_Data.m_Weapons[i + 1];

                    m_Ani.SetInteger("Weapon_Code", i + 1);
                    gameObject.SendMessage("PlaySound", (int)(SOUNDCLIP.SWAP));

                }
                m_Data.m_WeaponInhand.gameObject.SetActive(true);
                Check_WeaponisAuto();

                //If the weapon is sniper, move camera position
                if(m_Data.m_WeaponInhand.m_ObjName == "Sniper")
                    m_CameraMove.CameraLerp(CAMERAPOS.SNIPER_SHOOTPOS);
                else
                    m_CameraMove.CameraLerp(CAMERAPOS.NORMALPOS);

                //if player was reloading, cancel relaoding.
                if (m_Data.m_isReloading == true)
                {
                    m_Data.m_isReloading = false;
                    m_Ani.SetBool("'Bool", false);
                }

                //Set bool.
                m_Data.m_isSwaping = true;
                m_Ani.SetBool("Swap_b", m_Data.m_isSwaping);
                CancelInvoke("SetSwapingFalse");
                Invoke("SetSwapingFalse", 1f);

                //Set IK Position
                m_AimIK.SetHandsIKPosition(m_Data.m_WeaponInhand.m_GrabPosRight, m_Data.m_WeaponInhand.m_GrabPosLeft);

                //Set Speed
                m_Data.m_Speed = m_Data.m_MaxSpeed - m_Data.m_WeaponInhand.m_Weight;

                break;
            }
        }
    }

    public void SwapWeaponTo(int index) {
        m_Ani.SetBool("Minigun_Attack_Bool", false);

        if (m_Data.m_WeaponInhand == m_Data.m_Weapons[index]) return;

        m_Ani.SetTrigger("WeaponSwap");
        m_Data.m_WeaponInhand.gameObject.SetActive(false);

        m_Data.m_WeaponInhand = m_Data.m_Weapons[index];
        m_Ani.SetInteger("Weapon_Code", index);
        gameObject.SendMessage("PlaySound", value: SOUNDCLIP.SWAP);

        m_Data.m_WeaponInhand.gameObject.SetActive(true);
        Check_WeaponisAuto();

        //If the weapon is sniper, move camera position
        if (m_Data.m_WeaponInhand.m_ObjName == "Sniper")
            m_CameraMove.CameraLerp(CAMERAPOS.SNIPER_SHOOTPOS);
        else
            m_CameraMove.CameraLerp(CAMERAPOS.NORMALPOS);

        //if player was reloading, cancel relaoding.
        if (m_Data.m_isReloading == true) {
            m_Data.m_isReloading = false;
            m_Ani.SetBool("WeaponReloadBool", false);
        }

        //Set bool.
        m_Data.m_isSwaping = true;
        m_Ani.SetBool("Swap_b", m_Data.m_isSwaping);
        Invoke("SetSwapingFalse", 1f);

        //Set IK Position
        m_AimIK.SetHandsIKPosition(m_Data.m_WeaponInhand.m_GrabPosRight, m_Data.m_WeaponInhand.m_GrabPosLeft);
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
        if (m_Data.m_WeaponInhand.m_AmmoBulletNum < m_Data.m_WeaponInhand.m_MaxBulletNum
            && m_Data.m_isReloading == false)
        {
            //m_Data.m_Ani.SetTrigger("WeaponReload");
            m_Data.m_isReloading = true;
            m_Ani.SetBool("Reload_b", m_Data.m_isReloading);
            CancelInvoke("SetRelaodingFalse");
            Invoke("SetRelaodingFalse", m_Data.m_WeaponInhand.m_ReloadTime);
            m_Ani.SetBool("Minigun_Attack_Bool", false);

            if (m_Data.m_WeaponInhand.m_WeaponType == Weapon_Type.RIFLE)
            {
                gameObject.SendMessage("PlaySound", (int)(SOUNDCLIP.RIFLERELOAD));
            }
            if (m_Data.m_WeaponInhand.m_WeaponType == Weapon_Type.HANDGUN)
            {
                gameObject.SendMessage("PlaySound", (int)(SOUNDCLIP.HANDGUNRELOAD));
            }
        }
    }

    public IEnumerator CheckIsShooting()
    {
        while(true)
        {
            if (m_Data.m_isShooting && m_Data.m_WeaponInhand.m_Autoshot)
            {
                Firebullet();
            }
            else if (m_Data.m_isShooting && !m_Data.m_WeaponInhand.m_Autoshot)
            {
                if (m_FireOnceBool == false)
                {
                    m_FireOnceBool = true;
                    Firebullet();
                }
            }
            else if (!m_Data.m_isShooting)
            {
                m_FireOnceBool = false;
            }
            yield return null;
        }
    }

    //When reload motion is end, charge bullet in weapon.
    void ChargeBullet_Inweapon()
    {
        if (m_Data.m_WeaponInhand)
        {
            m_Data.m_WeaponInhand.ChargeBullet();
            m_Data.m_isReloading = false;
            //m_Ani.SetBool("WeaponReloadBool", false);
        }
    }

    void SetShootingFalse()
    {
        m_Data.m_isShooting = false;
        m_Ani.SetBool("Shoot_b", m_Data.m_isShooting);
    }

    void SetRelaodingFalse()
    {
        m_Data.m_isReloading = false;
        m_Ani.SetBool("Reload_b", m_Data.m_isReloading);
    }

    void SetSwapingFalse()
    {
        m_Data.m_isSwaping = false;
        m_Ani.SetBool("Swap_b", m_Data.m_isSwaping);
    }
}