using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxChecker : MonoBehaviour {

    public enum HitboxType { HEAD, BODY}

    //Set this on unity editor
    public HitboxType hitboxtype;
    public GameObject Object;       //The highest level of parent of this instance
    public BoxCollider collider;

    private void Start()
    {
        collider = this.gameObject.GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {
        collider.enabled = true;
    }

    public void GetDamage(int[] _power)
    {
        if(hitboxtype == HitboxType.BODY)
        {
            Object.SendMessage("GetDamage", _power[1]);
            Debug.Log("Body Hit!! : " + _power[1]);
        }
        else if (hitboxtype == HitboxType.HEAD)
        {
            Object.SendMessage("GetDamage", _power[0]);
            Debug.Log("Head Hit!!  :" + _power[0]);
        }
    }
}