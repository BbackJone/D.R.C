﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


[System.Serializable]
public class GameDB
{
    public ZomebieDB[] Zombie;
    public WeaponDB[] Weapon;
}

public class JsonManager : MonoBehaviour {

    public MyPath m_Path;
    private ObjectManager m_ObjMgr;
    public GameDB m_GameDB = new GameDB();


    // Use this for initialization
    void Awake()
    {
        m_Path = GetComponent<MyPath>();
        m_ObjMgr = GameObject.FindGameObjectWithTag("GameController").GetComponent<ObjectManager>();
    }


	void Start () {
        StartCoroutine(ReadJson());
	}

	IEnumerator ReadJson()
    {
        string Jsonstring = File.ReadAllText(m_Path.streamingPath);
        m_GameDB = JsonUtility.FromJson<GameDB>(Jsonstring);
        Debug.Log("ReadJson Complete!");

        SetDB();
        m_ObjMgr.NextScene("Stage");
        yield return null;
    }

    void SetDB()
    {
        ZomebieDB[] TempZomebie = m_GameDB.Zombie;
        WeaponDB[] TempWeapon = m_GameDB.Weapon;
        Debug.Log("SetDB : TempWeapon.Count == " + TempWeapon.Length);
        for (int i = 0; i < TempZomebie.Length; i++)
            m_ObjMgr.m_DBMgr.m_ZomebieDB.Add(TempZomebie[i].Name, TempZomebie[i]);
        for (int i = 0; i < TempWeapon.Length; i++)
            m_ObjMgr.m_DBMgr.m_WeaponDB.Add(TempWeapon[i].Name, TempWeapon[i]);

        Debug.Log("SetDB Complete!");
    }
}
