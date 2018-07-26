﻿using System.Collections;
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

    public void ObjListAdd()
    {
        ObjectManager.m_Inst.Objects.m_Enemylist.Add(this);
    }
}
