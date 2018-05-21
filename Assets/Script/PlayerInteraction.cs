using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {


    private ObjectManager m_ObjMgr;
    private PlayerData m_Data;

	// Use this for initialization
	void Awake () {
        m_ObjMgr = GameObject.FindGameObjectWithTag("GameController").GetComponent<ObjectManager>();
        m_Data = GetComponent<PlayerData>();
	}

    void OnEnable()
    {
        StartCoroutine("PlayerStateCheck");
    }

    void Start()
    {
        StartCoroutine("PlayerStateCheck");
        ObjListAdd();
    }

    IEnumerator PlayerStateCheck()
    {
        while (true)
        {
            if (m_Data == false)
            {
                m_ObjMgr.GameOverCheck();
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void ObjListAdd()
    {
        m_ObjMgr.Objects.m_Playerlist.Add(this);
    }

    public void GetDamage(int _damage)
    {
        m_Data.m_Hp -= _damage;
    }
}
