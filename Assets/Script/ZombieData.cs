using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is a variable for getting information from the database.
[System.Serializable]
public struct ZomebieDB
{
    public string Name;
    public int Hp;
    public float Speed;
    public int AttackDamage;
    public float AttackRange;
    public float AttackSpeed;
}

public class ZombieData : MonoBehaviour
{
    //this is information about this
    public int m_Hp { get; set; }
    public float m_Speed { get; set; }
    public ObjType m_Type { get; set; }
    public bool m_Death { get; set; }
    public float m_DeathTimer { get; set; }
    public string m_ObjName { get; set; }
    public int m_MaxHp { get; set; }

    public float m_AttackSpeed { get; set; }
    public float m_AttackTimer { get; set; }
    public int m_AttackDamage { get; set; }
    public float m_AttackRange { get; set; }
    public int m_Price { get; set; }       //Money that is got when player kill this.

    public string m_ZombieSort;     //This is required to get DB Info. 

    void Awake()
    {
        Initialize();
        Debug.Log("Data Awake m_Data.m_Hp : " + m_Hp);
    }

    void Start()
    {
        Debug.Log("Data Start m_Data.m_Hp : " + m_Hp);
        m_Price = 1;
    }

    void OnEnable()
    {
        Debug.Log("Data OnEnable m_Data.m_Hp : " + m_Hp);
        m_Hp = m_MaxHp;
        m_Death = false;
        m_DeathTimer = 0f;
        m_AttackTimer = 1.5f;
    }


    public void Initialize()
    {
        //DB에서 캐릭터의 능력치를 받아온다.
        ZomebieDB DBData = ObjectManager.m_Inst.m_DBMgr.m_ZomebieDB[m_ZombieSort];
        m_Speed = DBData.Speed;
        m_AttackRange = DBData.AttackRange;
        m_MaxHp = DBData.Hp;
        m_Hp = m_MaxHp;
        m_AttackDamage = DBData.AttackDamage;
        m_AttackSpeed = DBData.AttackSpeed;
        m_ObjName = DBData.Name;
       
        m_Type = ObjType.OBJ_ENEMY;
    }

    public void GetDamage(int _damage)
    {
        m_Hp -= _damage;
    }
}