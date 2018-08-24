using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {

    private PlayerData m_Data;

	// Use this for initialization
	void Awake () {
        ObjListAdd();       //Put this object at ObjMgr.
        m_Data = GetComponent<PlayerData>();
    }

    void OnEnable()
    {
        StartCoroutine("PlayerStateCheck");
    }

    void Start()
    {
        StartCoroutine("PlayerStateCheck");
    }

    //If player dead and become inactive, do game over check.
    IEnumerator PlayerStateCheck()
    {
        while (true)
        {
            if (m_Data == false)
            {
                ObjectManager.m_Inst.GameOverCheck();
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void ObjListAdd()
    {
        ObjectManager.m_Inst.Objects.m_Playerlist.Add(this);
    }

    public void GetDamage(int _damage)
    {
        m_Data.m_Hp -= _damage;
    }
}
