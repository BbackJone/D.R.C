﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Please Set Layer of Object that contain this script "Enemy"
public class HitboxChecker : MonoBehaviour {

    public enum HitboxType { HEAD, BODY}

    //Set this on unity editor
    public HitboxType hitboxtype;
    public GameObject Object;       //The highest level of parent of this instance


    public void GetDamage(int[] _power)
    {
        if(hitboxtype == HitboxType.BODY)
        {
            Object.SendMessage("GetDamage", _power[1]);
        }
        else if (hitboxtype == HitboxType.HEAD)
        {
            Object.SendMessage("GetDamage", _power[0]);

            //Show RedAim for 0.1 seconds
            UIManager.m_Instance.ShowRedAim();
        }
    }
}