using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour {

    private float m_StayTime;
    private float Timer;

	// Use this for initialization
	void OnEnable () {
        m_StayTime = 3f;
        Timer = 0f;

        StartCoroutine("CountDown");
    }


    IEnumerator CountDown()
    {
        while(true)
        {
            Timer += Time.deltaTime;
            if (Timer > m_StayTime)
                gameObject.SetActive(false);

            yield return null;
        }
    }
}
