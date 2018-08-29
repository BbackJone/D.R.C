using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {

    //Player's values
    public float m_MaxSpeed { get; set; }
    public float m_Speed { get; set; }
    public Vector3 m_Move { get; set; }  //current speed value
    public int m_Money { get; set; }
    public int m_Hp { get; set; }
    public int m_MaxHp { get; set; }
    public ObjType m_Type { get; set; }

    //About weapons
    public List<Weapon> m_Weapons { get; set; }
    public Weapon m_WeaponInhand { get; set; }      //플레이어가 현재 들고있는 무기
    public Transform rightHand { get; set; }
    public bool m_isReloading { get; set; }       //플레이어가 장전 중일때는 공격을 할수 없습니다.
    public bool m_isShooting { get; set; }
    public bool m_isSwaping { get; set; }

    //Other components
    public Camera m_Camera { get; set; }
    public Rigidbody m_Rigidbody { get; set; }
    private Animator m_Ani;
    private AimSystem m_AimSystem;
    private AimIK m_AimIK;

    void Awake()
    {
        m_Camera = Camera.main;
        m_Ani = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AimSystem = GetComponent<AimSystem>();
        m_AimIK = GetComponent<AimIK>();

        m_Weapons = new List<Weapon>();

        //Get transform of righthand
        rightHand = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0);

        //오른손의 자식으로 설정된 무기들을 m_Weapons에 추가하고, 비활성화 시킨다.
        for (int i = 0; i < rightHand.childCount; i++)
        {
            Weapon weap = rightHand.GetChild(i).GetComponent<Weapon>();
            weap.m_ShootTarget = m_AimSystem.m_RayTarget;
            m_Weapons.Add(weap);
            weap.gameObject.SetActive(false);
        }

        m_WeaponInhand = m_Weapons[0];
        m_WeaponInhand.gameObject.SetActive(true);
        m_AimIK.SetHandsIKPosition(m_WeaponInhand.m_GrabPosRight, m_WeaponInhand.m_GrabPosLeft);
    }

    void Start()
    {
        m_Type = ObjType.OBJ_PLAYER;
        m_MaxSpeed = 7f;
        m_Speed = m_MaxSpeed - m_WeaponInhand.m_Weight;
        m_isReloading = false;
        m_isShooting = false;
        m_isSwaping = false;
        m_Money = 0;
    }
}
