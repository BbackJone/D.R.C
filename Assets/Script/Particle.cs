using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour {

    public float m_StayTime;
    private float Timer;

	// Use this for initialization
	void OnEnable () {
        Timer = 0f;

        StartCoroutine("CountDown");
    }


    IEnumerator CountDown()
    {
        while(true)
        {
            Timer += Time.deltaTime;
            if (Timer > m_StayTime)
            {
                gameObject.SetActive(false);
                if (ObjectPoolMgr.instance.m_ParticleOrderList.ContainsKey(gameObject.name))
                    ObjectPoolMgr.instance.m_ParticleOrderList[gameObject.name].PopFront();
            }
                

            yield return null;
        }
    }
}
