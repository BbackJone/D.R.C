using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsWithTag
{
    public List<PlayerInteraction> m_Playerlist { get; set; }
    public List<GameObject> m_Colleaguelist { get; set; }
    public List<ZombieInteraction> m_Enemylist { get; set; }
    public List<Weapon> m_Weaponlist { get; set; }
    public List<Bullet> m_Bulletlist { get; set; }


    public ObjectsWithTag()
    {
        m_Playerlist = new List<PlayerInteraction>();
        m_Colleaguelist = new List<GameObject>();
        m_Enemylist = new List<ZombieInteraction>();
        m_Weaponlist = new List<Weapon>();
        m_Bulletlist = new List<Bullet>();
    }

    /*void Start()
    {
        / *GameObject[] _Players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < _Players.Length; i++ )
        {
            PlayerMove _obj = _Players[i].GetComponent<PlayerMove>();
            m_Playerlist.Add(_obj);
        }


        GameObject[] _Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < _Enemies.Length; i++)
        {
            ZomebieMove _obj = _Enemies[i].GetComponent<ZomebieMove>();
            m_Enemylist.Add(_obj);
        }* /
    }*/

    public void AddObject(GameObject _obj, string _tag)
    {
        if (_tag == "Player")
        {
            PlayerInteraction Player = _obj.GetComponent<PlayerInteraction>();
            m_Playerlist.Add(Player);
        }
        else if (_tag == "Enemy")
        {
            ZombieInteraction Enemy = _obj.GetComponent<ZombieInteraction>();
            m_Enemylist.Add(Enemy);
        }
    }
}