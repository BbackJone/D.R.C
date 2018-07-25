using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieInteraction : MonoBehaviour {

	// Use this for initialization
    void Awake()
    {
       
    }

    void Start()
    {
        ObjListAdd();          //Put this object at ObjMgr.
    }

    //Set nearest enemy as target
    public Transform GetTarget(Transform _trans)
    {
        float MinDis = 100000f;
        PlayerInteraction target = null;
        if (ObjectManager.m_Inst.Objects.m_Playerlist.Count <= 0)
            return null;

        foreach (PlayerInteraction pm in ObjectManager.m_Inst.Objects.m_Playerlist)
        {
            if (pm == null) continue;
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
        ObjectManager.m_Inst.Objects.m_Enemylist.Add(this);
    }
}
