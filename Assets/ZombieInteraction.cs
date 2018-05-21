using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieInteraction : MonoBehaviour {

    private ObjectManager m_ObjMgr;

	// Use this for initialization
    void Awake()
    {
        m_ObjMgr = GameObject.FindGameObjectWithTag("GameController").GetComponent<ObjectManager>();
        Debug.Log(m_ObjMgr.gameObject.name + "is cached");
    }

    void Start()
    {
        ObjListAdd();
    }

    public Transform GetTarget(Transform _trans)
    {
        float MinDis = 100000f;
        PlayerInteraction target = null;
        foreach (PlayerInteraction pm in m_ObjMgr.Objects.m_Playerlist)
        {
            float dis = Vector3.Distance(pm.transform.position, this.transform.position);
            if (MinDis > dis)
            {
                MinDis = dis;
                target = pm;
            }
        }
        return target.transform;
    }

    public void ObjListAdd()
    {
        m_ObjMgr.Objects.m_Enemylist.Add(this);
    }
}
