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
    
    public PlayerInteraction GetPlayer(Transform _obj)
    {
        for (int i = 0; i < m_Playerlist.Count; i++)
        {
            if (m_Playerlist[i].transform == _obj)
            {
                return m_Playerlist[i];
            }
        }

        return null;
    }

    public ZombieInteraction Getzombie(Transform _obj)
    {
        for (int i = 0; i < m_Enemylist.Count; i++)
        {
            if (m_Enemylist[i].transform == _obj)
            {
                return m_Enemylist[i];
            }
        }

        return null;
    }

    public Bullet Getbullet(Transform _obj)
    {
        for (int i = 0; i < m_Bulletlist.Count; i++)
        {
            if (m_Bulletlist[i].transform == _obj)
            {
                return m_Bulletlist[i];
            }
        }

        return null;
    }
}