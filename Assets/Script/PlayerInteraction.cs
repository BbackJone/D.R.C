using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {

    public AudioSource AudioSource;
    public AudioClip AudioClip;
    private PlayerData m_Data;
    private float sevol;

	// Use this for initialization
	void Awake () {
       
        m_Data = GetComponent<PlayerData>();
        ObjectManager.m_Inst.m_Player = this;
    }
    private void Update()
    {
         sevol = PlayerPrefs.GetFloat("sevol");
    }

    public void GetDamage(int _damage)
    {
        m_Data.m_Hp -= _damage;
        AudioSource.PlayOneShot(AudioClip,sevol);
    }
}
