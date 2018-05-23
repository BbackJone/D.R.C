using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DBManager : MonoBehaviour {

    public Dictionary<string, ZomebieDB> m_ZomebieDB {get; set;}
    public Dictionary<string, WeaponDB> m_WeaponDB { get; set; }

	// Use this for initialization
    public DBManager()
    {
        m_ZomebieDB = new Dictionary<string, ZomebieDB>();
        m_WeaponDB = new Dictionary<string, WeaponDB>();
    }

    /*void Awake()
    {
        m_ObjPoolMgr = transform.parent.GetChild(0).GetComponent<ObjectPoolMgr>();
        m_ObjMgr = GameObject.FindGameObjectWithTag("GameController").GetComponent<ObjectManager>();
    }*/

    public void Initialize()
    {

    }

    void Start()
    {
        
    }
}
