using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DBManager : MonoBehaviour {

    public static DBManager instance;

    public Dictionary<string, ZomebieDB> m_ZomebieDB {get; set;}
    public Dictionary<string, WeaponDB> m_WeaponDB { get; set; }
    public Dictionary<int, WaveDB> m_WaveDB { get; set; }

    public bool loaded;

    public void Start()
    {
        loaded = false;
        instance = this;
    }

    // Use this for initialization
    public DBManager()
    {
        m_ZomebieDB = new Dictionary<string, ZomebieDB>();
        m_WeaponDB = new Dictionary<string, WeaponDB>();
        m_WaveDB = new Dictionary<int, WaveDB>();
    }
}
