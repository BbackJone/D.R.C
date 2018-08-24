using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower_Flame : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnParticleCollision(GameObject other)
    {
        if(other.CompareTag("Enemy"))
        {
            int[] DamageSet = new int[2] { 3, 3 };
            other.gameObject.SendMessage("GetDamage", DamageSet);
        }
    }
}
