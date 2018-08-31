using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {

    public AudioSource AudioSource;
    public AudioClip AudioClip;
    private PlayerData m_Data;

	// Use this for initialization
	void Awake () {
       
        m_Data = GetComponent<PlayerData>();
        ObjectManager.m_Inst.m_Player = this;
    }

    public void GetDamage(int _damage)
    {
        m_Data.m_Hp = Mathf.Max(m_Data.m_Hp - _damage, 0);
        AudioSource.PlayOneShot(AudioClip, VolumeHolderScript.instance.seVol);
    }
}
