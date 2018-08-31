using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour {

    private float sevol;
    public AudioSource m_Audiosouce;

    public AudioClip[] m_AudioClipArr;

    // Use this for initialization
    private void Awake()
    {
        
        m_Audiosouce = GetComponent<AudioSource>();
    }
    private void Update()
    {
        sevol = PlayerPrefs.GetFloat("sevol");
    }

    public void PlaySound(int _soundclip)
    {
        m_Audiosouce.PlayOneShot(m_AudioClipArr[_soundclip],sevol);
    }

    public bool isPlaying()
    {
        return m_Audiosouce.isPlaying;
    }
}


//isPlaying 이 필요하다고 생각하면
//1. isPlaying 하려는 Script에서 이걸 Getcomponent 해오기
//2. 해당 스크립트에서 Coroutine 돌려서 Timer 이용하기.