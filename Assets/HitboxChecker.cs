using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxChecker : MonoBehaviour {

    public enum HitboxType { HEAD, BODY}

    //Set this on unity editor
    public HitboxType hitboxtype;
    public GameObject Object;       //The highest level of parent of this instance


	// Use this for initialization
	void Start () {
		
    }


    public void GetDamage(int _power)
    {

    }
}
